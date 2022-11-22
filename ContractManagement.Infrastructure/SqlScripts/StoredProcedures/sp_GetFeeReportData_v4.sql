CREATE PROCEDURE `sp_GetFeeReportData`(
IN sharingType INT,
IN marketAreaId INT,
IN projectId INT,
IN outCustomerCode MEDIUMTEXT,
IN contractorFullName MEDIUMTEXT,
IN inContractCode MEDIUMTEXT,
IN serviceId INT,
IN effectiveStartDate DATE,
IN effectiveEndDate DATE ,
IN currencyUnitCode MEDIUMTEXT ,
IN `skips` INT,
IN `take` INT,
OUT `totalRecords` INT,
OUT `sumInstallationFee` INT,
OUT `sumFeeOfMonthByOutContract` INT,
OUT `sumCostOfInstallFee` INT,
OUT `sumCostOfFeeInMonth` INT,
OUT `sumEachMonthVND` MEDIUMTEXT,
OUT `sumAllMonthsVND` INT,
OUT `sumEachMonthUSD` MEDIUMTEXT,
OUT `sumAllMonthsUSD` INT)
BEGIN
DROP TEMPORARY TABLE
	IF EXISTS temp_ReportFee;
	CREATE TEMPORARY TABLE temp_ReportFee 
		SELECT 
        t1.Id AS Id,
		oct.MarketAreaName AS MarketAreaName,
		cp.ContractorCategoryName AS CustomerCategoryName,
		ctor.ContractorFullName AS CustomerFullName,
		ict.ContractCode AS InContractCode,
		ctor.ContractorFullName AS OutContractorName, 
		oct.ContractCode AS OutContractCode,
        CASE
			oct.ContractStatusId
			WHEN 1 THEN 'Chờ ký' 
            WHEN 2 THEN 'Đã ký' 
			WHEN 5 THEN 'Đã thanh lý' 
			WHEN 9 THEN 'Hủy' 
            ELSE '' 
		END AS OutContractStatus, 
		csp.TimeLine_Signed AS TimeLineSigned,
		csp.TimeLine_Effective AS TimeLineEffective,
		oct.TimeLine_Liquidation AS TimeLineLiquidation,
		oct.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
		csp.TimeLine_PaymentPeriod AS TimeLinePaymentPeriod,
        CASE
			oct.Payment_Form 
            WHEN 1 THEN 'Trả trước' 
            ELSE 'Trả sau' 
		END AS PaymentForm,
		csp.ServiceName AS ServiceDetails,
		csp.CId AS CId,
		CONCAT(ocp1.InstallationAddress_Street,' - ' , ocp1.InstallationAddress_District, ' - ', ocp1.InstallationAddress_City) AS StartPoint,
		CONCAT(ocp2.InstallationAddress_Street, ' - ', ocp2.InstallationAddress_District, ' - ', ocp2.InstallationAddress_City) AS EndPoint,
		csp.InstallationFee AS InstallationFee,
		IF(t2.Month = 1 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS JanInVND,
		IF(t2.Month = 1 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS JanInUSD,
		IF(t2.Month = 2 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS FebInVND,
		IF(t2.Month = 2 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS FebInUSD,
		IF(t2.Month = 3 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS MarInVND,
		IF(t2.Month = 3 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS MarInUSD,
		IF(t2.Month = 4 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS AprInVND,
		IF(t2.Month = 4 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS AprInUSD,
		IF(t2.Month = 5 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS MayInVND,
		IF(t2.Month = 5 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS MayInUSD,
		IF(t2.Month = 6 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS JunInVND,
		IF(t2.Month = 6 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS JunInUSD,
		IF(t2.Month = 7 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS JulInVND,
		IF(t2.Month = 7 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS JulInUSD,
		IF(t2.Month = 8 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS AugInVND,
		IF(t2.Month = 8 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS AugInUSD,
		IF(t2.Month = 9 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS SepInVND,
		IF(t2.Month = 9 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS SepInUSD,
		IF(t2.Month = 10 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS OctInVND,
		IF(t2.Month = 10 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS OctInUSD,
		IF(t2.Month = 11 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS NovInVND,
		IF(t2.Month = 11 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS NovInUSD,
		IF(t2.Month = 12 AND t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0) AS DecInVND,
		IF(t2.Month = 12 AND t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0) AS DecInUSD,
		SUM(IF(t1.CurrencyUnitCode = 'VND', t2.SharingAmount, 0)) AS AllMonthsVND,
		SUM(IF(t1.CurrencyUnitCode = 'USD', t2.SharingAmount, 0)) AS AllMonthsUSD,
		CASE
			ict.ContractTypeId
			WHEN 1 THEN 'Thuê kênh' 
            WHEN 2 THEN 'Phân chia hoa hồng' 
			WHEN 3 THEN 'Phân chia doanh thu' 
			WHEN 4 THEN 'Bảo trì, bảo dưỡng' 
            ELSE '' 
		END AS SharingTypeName, --
		ictor.ContractorFullName AS AgentContractName, --
		oct.ProjectName AS ProjectName,
		CASE
			ictor.IsEnterprise 
			WHEN 1 THEN
			'Doanh nghiệp' ELSE 'Cá nhân' 
		END AS IsEnterprise,
		csp.GrandTotal AS FeeOfMonthByOutContract,
		CASE
			t1.SharingType
            WHEN 2 THEN 'Phân chia hoa hồng' 
			WHEN 3 THEN 'Doanh thu' 
            ELSE '' 
		END AS SharingServiceName,
        t1.InSharedInstallFeePercent AS InSharedInstallFeePercent,
        t1.OutSharedInstallFeePercent AS OutSharedInstallFeePercent,
        t1.InSharedPackagePercent AS InSharedPackagePercent,
        t1.OutSharedPackagePercent AS OutSharedPackagePercent,
		csp.HasStartAndEndPoint AS HasStartAndEndPoint,
        IF(csp.HasStartAndEndPoint = 0, 
			((ocp1.InstallationFee * t1.InSharedInstallFeePercent)/100 ) + ((ocp2.MonthlyCost * t1.OutSharedInstallFeePercent)/100 ), 
			(ocp2.InstallationFee * t1.OutSharedInstallFeePercent)/100)
		AS CostOfInstallFee,
		IF(csp.HasStartAndEndPoint = 0, 
		((ocp1.MonthlyCost * t1.InSharedPackagePercent)/100 ) +  ((ocp2.MonthlyCost * t1.OutSharedPackagePercent)/100 ), 
		(ocp2.MonthlyCost * t1.OutSharedPackagePercent)/100)
        AS CostOfFeeInMonth,
		oct.SignedUserName AS SignedUserName,
		oct.OrganizationUnitName AS OrganizationUnitName,
		oct.CustomerCareStaffUserId AS CustomerCareStaffUserName
		FROM 
		ContractSharingRevenueLines AS t1
		LEFT JOIN SharingRevenueLineDetails AS t2 ON t1.Id = t2.SharingLineId
		INNER JOIN InContracts AS ict ON ict.Id = t1.InContractId
		INNER JOIN Contractors AS ictor ON ictor.Id = ict.ContractorId
		INNER JOIN OutContractServicePackages AS csp ON csp.Id = t1.OutServiceChannelId
		INNER JOIN OutContracts AS oct ON oct.Id = t1.OutContractId
		INNER JOIN Contractors AS ctor ON ctor.Id = oct.ContractorId 
		INNER JOIN ContractorProperties cp ON ctor.Id = cp.ContractorId
		LEFT JOIN OutputChannelPoints ocp1 ON ocp1.Id = csp.StartPointChannelId
		INNER JOIN OutputChannelPoints ocp2 ON ocp2.Id = csp.EndPointChannelId
		WHERE 
		t1.IsDeleted = 0
		AND t1.IsActive = 1
        AND (CASE
			WHEN  `sharingType` = 1 THEN t1.SharingType = 1
			ELSE  t1.SharingType = 2 OR t1.SharingType = 3
		END)
        AND (effectiveStartDate IS NULL OR DATE(oct.TimeLine_Effective) >=  effectiveStartDate)
 		AND (effectiveEndDate IS NULL OR DATE(oct.TimeLine_Effective) <=  effectiveEndDate)
        AND (projectId = 0 OR ict.ProjectId = projectId)
		AND (outCustomerCode = '' OR ctor.ContractorCode  LIKE CONCAT('%', outCustomerCode, '%') )
        AND (currencyUnitCode = '' OR t1.CurrencyUnitCode LIKE CONCAT('%', currencyUnitCode, '%') )
        AND (contractorFullName = '' OR ctor.ContractorFullName  LIKE CONCAT('%', contractorFullName, '%') )
		AND (inContractCode = '' OR t1.InContractCode  LIKE CONCAT('%', inContractCode, '%') )	
		AND (serviceId = 0 OR csp.ServiceId = serviceId)
		GROUP BY
		t1.Id,
		t1.InContractId,
		t1.OutContractId
		ORDER BY t1.Id desc
		;
        
        -- 
        SELECT * FROM temp_ReportFee
		LIMIT take OFFSET skips;
		SELECT 
        COUNT(t1.Id), 
        SUM(InstallationFee),
        SUM(FeeOfMonthByOutContract),
        SUM(CostOfInstallFee),
        SUM(CostOfFeeInMonth),
        CONCAT(
        SUM(JanInVND), "/", SUM(FebInVND),
        "/", SUM(MarInVND), "/", SUM(AprInVND),
        "/", SUM(MayInVND), "/", SUM(JunInVND), 
        "/", SUM(JulInVND), "/", SUM(AugInVND), 
        "/", SUM(SepInVND), "/", SUM(OctInVND), 
        "/", SUM(NovInVND), "/", SUM(DecInVND)),
        SUM(AllMonthsVND),
        CONCAT(
        SUM(JanInUSD), "/", SUM(FebInUSD),
        "/", SUM(MarInUSD), "/", SUM(AprInUSD),
        "/", SUM(MayInUSD), "/", SUM(JunInUSD), 
        "/", SUM(JulInUSD), "/", SUM(AugInUSD), 
        "/", SUM(SepInUSD), "/", SUM(OctInUSD), 
        "/", SUM(NovInUSD), "/", SUM(DecInUSD)),
        SUM(AllMonthsUSD)
        INTO totalRecords, sumInstallationFee, sumFeeOfMonthByOutContract, sumCostOfInstallFee, sumCostOfFeeInMonth, sumEachMonthVND, sumAllMonthsVND, sumEachMonthUSD, sumAllMonthsUSD
		FROM temp_ReportFee AS t1;
END