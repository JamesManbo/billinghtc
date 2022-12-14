CREATE PROCEDURE `GetTotalRevenueEnterpiseReport`(
		IN `skips` INT,
		IN `take` INT,
		OUT `totalRecords` INT,
        IN `marketAreaId` INT,
        IN `contractCode` VARCHAR(50),
        IN `customerCode` VARCHAR(50),
        IN `contractorFullName` VARCHAR(50),
        IN `effectiveStartDate` DATE,
		IN `effectiveEndDate` DATE,
        IN `serviceId` INT,
		IN `projectId` INT,
        IN `orderBy` VARCHAR(100),	
        IN `statusId` INT,
		IN `customerCategoryId` INT,
        OUT `sumAmountLost` DECIMAL(65, 30),
        OUT `sumOtherFee` DECIMAL(65, 30),
        OUT `sumInstallationFee` DECIMAL(65, 30),
        OUT `sumPackagePrice` DECIMAL(65, 30),
        OUT `sumTermTotal` DECIMAL(65, 30),
        OUT `sumLastYearSales` DECIMAL(65, 30),
        OUT `sumEachMonthTotal` MEDIUMTEXT,
        OUT `sumEachMonthNewTotal` MEDIUMTEXT,
        OUT `sumAllMonthsTotalNotVAT` DECIMAL(65, 30),
        OUT `sumLastYearSaleLost` DECIMAL(65, 30),
        OUT `sumNewRevenueTotal` DECIMAL(65, 30),
        OUT `sumCurrentYearSales` DECIMAL(65, 30),
        OUT `sumPaidTotal` DECIMAL(65, 30),
        OUT `sumClearingTotal` DECIMAL(65, 30),
        OUT `sumDebtTotal` DECIMAL(65, 30)
		)
BEGIN
    DROP TEMPORARY TABLE
	IF EXISTS temp;

	CREATE TEMPORARY TABLE temp 
		SELECT
			ct.Id,
			ct.MarketAreaName,
			ct.OrganizationUnitName AS ContractDepartment,
			ct.SignedUserName,
			cp.ContractorCategoryName AS ChannelRange,
			cp.ContractorGroupNames AS ContractorGroupName,
			cp.ContractorTypeName,
			cp.ContractorClassName,
			cp.ContractorIndustryNames,
			ctor.ContractorFullName,
			ctor.ContractorCode AS CustomerCode,
			ct.ContractCode AS OutContractCode,
			ct.ContractCode,
			ct.TimeLine_Signed AS TimeLineSigned,
			csp.TimeLine_Effective AS TimeLineEffective,
			csp.TimeLine_StartBilling AS TimeLineStartBillingDate,
			t2.`Code` AS TransactionCode,
			t2.TransactionDate,
			t4.`Name` AS ContractSatus,
			t2.Note AS ImplementationDetails,
			t3.`Name` AS ServiceDeliveryStatus,
			t3.`Name` AS TransactionStatus,
			CASE
				WHEN t2.Type IN ( 3, 4, 8 ) THEN
					t2.EffectiveDate ELSE t2.EffectiveDate 
				END AS TransactionEffectedDate,
			t2.ReasonCancelAcceptance,
			CASE
				WHEN t2.Type = 8 AND DATE( ct.TimeLine_Expiration ) - DATE( t2.EffectiveDate ) > 0 
				THEN 'Trước hạn' ELSE 'Sau hạn' 
				END IsEndBeforeExpriedDate,
			TIMESTAMPDIFF( MONTH, ct.TimeLine_Expiration, t2.EffectiveDate ) * csp.PackagePrice AS AmountLost,
			ct.TimeLine_Expiration AS TimeLineExpiration,
			ctor.ContractorCode AS CID,
			sPoint.InstallationAddress_City AS StartPoint,
			ePoint.InstallationAddress_City AS EndPoint,
			CONCAT( csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom ) AS DomesticBandwidth, 
			CONCAT( csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS InternationalBandwidth,
			t6.GroupName AS ServiceGroupName,
			csp.ServiceName,
			csp.OtherFee,
			csp.InstallationFee,
			csp.PackagePrice,
			csp.InstallationFee + csp.PackagePrice * 12 AS TermTotal,
			CASE  WHEN YEAR(csp.TimeLine_StartBilling) < YEAR(CURRENT_DATE) THEN csp.PackagePrice * 12
				ELSE 0
			END AS LastYearSales,
			
            0 AS AllMonthsTotalNotVAT,
            0 AS LastYearSaleLost,
            0 AS NewRevenueTotal,
            0 AS CurrentYearSales,
            0 AS PaidTotal,
            0 AS ClearingTotal,
            0 AS DebtTotal

			FROM OutContracts AS ct
			LEFT JOIN Contractors AS ctor ON ctor.Id = ct.ContractorId
			LEFT JOIN ContractorProperties AS cp ON cp.ContractorId = ctor.Id
			LEFT JOIN Contractors AS ctor2 ON ct.AgentId = ctor.IdentityGuid
			LEFT JOIN ContractStatus AS t4 ON t4.Id = ct.ContractStatusId
			LEFT JOIN OutContractServicePackages AS csp ON ct.Id = csp.OutContractId
			LEFT JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
			LEFT JOIN OutputChannelPoints AS ePoint ON csp.EndPointChannelId = ePoint.Id
			LEFT JOIN Transactions AS t2 ON t2.OutContractId = ct.id
			LEFT JOIN TransactionStatuses AS t3 ON t3.Id = t2.StatusId
			LEFT JOIN Services AS t5 ON t5.Id = csp.ServiceId
			LEFT JOIN ServiceGroups AS t6 ON t6.Id = t5.GroupId
            WHERE IFNULL(ct.ProjectId,0) = 0
            AND (effectiveStartDate IS NULL OR DATE(csp.TimeLine_Effective) >=  effectiveStartDate)
			AND (effectiveEndDate IS NULL OR DATE(csp.TimeLine_Effective) <=  effectiveEndDate)
			AND (marketAreaId = 0 OR ct.MarketAreaId = marketAreaId)				
			AND (projectId  = 0 OR ct.ProjectId = projectId)
			AND (contractCode = '' OR ct.ContractCode LIKE CONCAT('%', contractCode, '%'))
			AND (customerCode = '' OR ctor.ContractorCode LIKE CONCAT('%', customerCode, '%'))	
            AND (contractorFullName = '' OR ctor.ContractorFullName LIKE CONCAT('%', contractorFullName, '%'));
        
        -- 
        SELECT * FROM temp
		LIMIT take OFFSET skips;
		SELECT 
        COUNT(t1.Id),
        SUM(IFNULL(AmountLost, 0)), SUM(IFNULL(OtherFee, 0)), SUM(IFNULL(InstallationFee, 0)), SUM(IFNULL(PackagePrice, 0)),
        SUM(IFNULL(TermTotal, 0)), SUM(IFNULL(LastYearSales, 0)), 
        CONCAT(
        0, "/", 0,
        "/", 0, "/", 0,
        "/", 0, "/", 0, 
        "/", 0, "/", 0, 
        "/", 0, "/", 0, 
        "/", 0, "/", 0),
		CONCAT(
        0, "/", 0,
        "/", 0, "/", 0,
        "/", 0, "/", 0, 
        "/", 0, "/", 0, 
        "/", 0, "/", 0, 
        "/", 0, "/", 0),
        SUM(IFNULL(AllMonthsTotalNotVAT, 0)), SUM(IFNULL(LastYearSaleLost, 0)), SUM(IFNULL(NewRevenueTotal, 0)), 
        SUM(IFNULL(CurrentYearSales, 0)), SUM(IFNULL(PaidTotal, 0)), SUM(IFNULL(ClearingTotal, 0)), SUM(IFNULL(DebtTotal, 0))
        INTO totalRecords, sumAmountLost, sumOtherFee, sumInstallationFee, sumPackagePrice, sumTermTotal, sumLastYearSales,
        sumEachMonthTotal, sumEachMonthNewTotal, sumAllMonthsTotalNotVAT, sumLastYearSaleLost, sumNewRevenueTotal, sumCurrentYearSales,
        sumPaidTotal, sumClearingTotal, sumDebtTotal
		FROM temp AS t1;
END