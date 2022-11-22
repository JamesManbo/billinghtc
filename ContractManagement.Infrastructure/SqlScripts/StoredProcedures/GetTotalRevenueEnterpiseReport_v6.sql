CREATE PROCEDURE `GetTotalRevenueEnterpiseReport`(
				IN `skips` INT,
				IN `take` INT,
        IN `marketAreaId` INT,
        IN `contractCode` VARCHAR(256),
        IN `customerCode` VARCHAR(256),
        IN `contractorFullName` VARCHAR(256),
        IN `effectiveStartDate` DATE,
				IN `effectiveEndDate` DATE,
        IN `serviceId` INT,
				IN `projectId` INT,
        IN `orderBy` VARCHAR(100),	
        IN `statusId` INT,
				IN `customerCategoryId` INT,
				IN `reportYear` INT
		)
BEGIN

	IF reportYear IS NULL
		THEN SET reportYear = CAST(YEAR(CURRENT_DATE) AS SIGNED);
	END IF;	
	
	SET @january = DATE(reportYear * 10000 + 101);
	SET @dayOfJanuary = DAY(LAST_DAY(@january));
	SET @february = DATE(reportYear * 10000 + 201);
	SET @dayOfFebruary = DAY(LAST_DAY(@february));
	SET @march = DATE(reportYear * 10000 + 301);
	SET @dayOfMarch = DAY(LAST_DAY(@march));
	SET @april = DATE(reportYear * 10000 + 401);
	SET @dayOfApril = DAY(LAST_DAY(@april));
	SET @may = DATE(reportYear * 10000 + 501);
	SET @dayOfMay = DAY(LAST_DAY(@may));
	SET @june = DATE(reportYear * 10000 + 601);
	SET @dayOfJune = DAY(LAST_DAY(@june));
	SET @july = DATE(reportYear * 10000 + 701);
	SET @dayOfJuly = DAY(LAST_DAY(@july));
	SET @august = DATE(reportYear * 10000 + 801);
	SET @dayOfAugust = DAY(LAST_DAY(@august));
	SET @september = DATE(reportYear * 10000 + 901);
	SET @dayOfSeptember = DAY(LAST_DAY(@september));
	SET @october = DATE(reportYear * 10000 + 1001);
	SET @dayOfOctober = DAY(LAST_DAY(@october));
	SET @november = DATE(reportYear * 10000 + 1101);
	SET @dayOfNovember = DAY(LAST_DAY(@november));
	SET @december = DATE(reportYear * 10000 + 1201);
	SET @dayOfDecember = DAY(LAST_DAY(@december));
	
	SELECT
		ct.Id,
		csp.Id AS OutContractServicePackageId,
		reportYear AS ReportYear,
		ct.MarketAreaName,
		ct.OrganizationUnitName AS ContractDepartment,
		ct.CustomerCareStaffUserId AS CustomerCareStaffUser,
		ct.SignedUserName,
		CASE
			WHEN ePoint.InstallationAddress_CountryId = '9999999999' AND (sPoint.Id IS NULL OR sPoint.InstallationAddress_CountryId = '9999999999') THEN
				'Trong nước'
			ELSE
				'Quốc tế'
		END AS ChannelRange,
		cp.ContractorGroupNames AS ContractorGroupName,
		cp.ContractorTypeName,
		cp.ContractorClassName,
		cp.ContractorIndustryNames,
		ctor.ContractorFullName,
		ctor.ContractorCode AS CustomerCode,
		ct.ContractCode AS OutContractCode,
		ct.ContractCode,
		ct.ContractStatusId AS ContractStatus,
		csp.TimeLine_Signed AS TimeLineSigned,
		csp.TimeLine_Effective AS TimeLineEffective,
		csp.TimeLine_StartBilling AS TimeLineStartBillingDate,
		'' AS ImplementationDetails,
		csp.StatusId,
		'' AS ServiceDeliveryStatus,
		'' AS TransactionStatus,
		t8.EffectiveDate AS TransactionEffectedDate,
		t8.ReasonType AS TerminateReasonType,
		CASE
			WHEN csp.StatusId <> 2 OR csp.TimeLine_TerminateDate IS NULL OR csp.TimeLine_StartBilling IS NULL THEN ''
			WHEN csp.StatusId = 2 AND csp.TimeLine_TerminateDate < DATE_ADD(csp.TimeLine_StartBilling, INTERVAL 1 YEAR)
			THEN 'Trước hạn'
			WHEN csp.StatusId = 2 AND csp.TimeLine_TerminateDate >= DATE_ADD(csp.TimeLine_StartBilling, INTERVAL 1 YEAR)
			THEN 'Sau hạn'
			ELSE ''
		END IsEndBeforeExpriedDate,		
		IFNULL(t7.PackagePrice, csp.PackagePrice) * IF(ct.TimeLine_RenewPeriod < 12, ct.TimeLine_RenewPeriod, 12) AS TermTotal,		
		-- Doanh thu nền năm báo cáo:
		CASE
			WHEN (csp.StatusId = 2 AND YEAR(csp.TimeLine_StartBilling) < reportYear AND YEAR(csp.TimeLine_TerminateDate) >= reportYear)
				OR ((csp.StatusId = 0 OR csp.StatusId = 1) AND YEAR(csp.TimeLine_StartBilling) < reportYear)
			THEN IFNULL(t7.PackagePrice, csp.PackagePrice) * 12
			ELSE 0
		END AS LastYearSales,
		
		-- Doanh thu mất đi do thanh lý (theo term HĐ)
		CASE	
			WHEN csp.StatusId = 2 AND YEAR(csp.TimeLine_Signed) >= reportYear - 1 AND TIMESTAMPDIFF(YEAR, csp.TimeLine_StartBilling, csp.TimeLine_TerminateDate) = 0
			THEN IFNULL(t7.PackagePrice, csp.PackagePrice) * TIMESTAMPDIFF(DAY, csp.TimeLine_TerminateDate, DATE_ADD(csp.TimeLine_StartBilling, INTERVAL 12 MONTH)) /30
			ELSE 0 
		END AS AmountLost,
		-- Doanh thu hàng tháng trong năm báo cáo:
		-- Tháng 1:
		CASE					
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@january)
				OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @january)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 1, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@january)),
					IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 1
						, csp.TimeLine_StartBilling					
						, @january)) + 1)
				* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJanuary
			-- -------------------------------------------------------------------------------------------------------
		END AS JanuaryTotal,
		-- Tháng 2:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@february)
				OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @february)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 2, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@february)),
					IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 2
						, csp.TimeLine_StartBilling					
						, @february)) + 1)
				* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfFebruary
			-- -------------------------------------------------------------------------------------------------------
		END AS FebruaryTotal,
		-- Tháng 3:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@march)
				OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @march)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 3, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@march)),
					IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 3
						, csp.TimeLine_StartBilling					
						, @march)) + 1)
				* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMarch
			-- -------------------------------------------------------------------------------------------------------
		END AS MarTotal,
		-- Tháng 4:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@april)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @april)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 4, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@april)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 4
							, csp.TimeLine_StartBilling					
							, @april)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfApril
			-- -------------------------------------------------------------------------------------------------------			
		END AS AprTotal,
		-- Tháng 5:
		CASE	
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@may)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @may)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 5, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@may)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 5
							, csp.TimeLine_StartBilling					
							, @may)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMay
			-- -------------------------------------------------------------------------------------------------------				
		END AS MayTotal,
		-- Tháng 6:
		CASE	
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@june)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @june)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 6, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@june)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 6
							, csp.TimeLine_StartBilling					
							, @june)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJune
			-- -------------------------------------------------------------------------------------------------------
		END AS JunTotal,
		-- Tháng 7:
		CASE	
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@july)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @july)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 7, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@july)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 7
							, csp.TimeLine_StartBilling					
							, @july)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJuly
			-- -------------------------------------------------------------------------------------------------------
		END AS JulTotal,
		-- Tháng 8:
		CASE	
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@august)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @august)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 8, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@august)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 8
							, csp.TimeLine_StartBilling					
							, @august)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfAugust
			-- -------------------------------------------------------------------------------------------------------		
		END AS AugTotal,
		-- Tháng 9:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@september)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @september)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 9, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@september)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 9
							, csp.TimeLine_StartBilling					
							, @september)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfSeptember
			-- -------------------------------------------------------------------------------------------------------				
		END AS SepTotal,
		-- Tháng 10:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@october)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @october)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 10, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@october)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 10
							, csp.TimeLine_StartBilling					
							, @october)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfOctober
			-- -------------------------------------------------------------------------------------------------------
		END AS OctTotal,
		-- Tháng 11:
		CASE
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@november)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @november)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 11, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@november)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 11
							, csp.TimeLine_StartBilling					
							, @november)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfNovember
			-- -------------------------------------------------------------------------------------------------------
		END AS NovTotal,		
		-- Tháng 12:
		CASE	
			WHEN csp.TimeLine_StartBilling > LAST_DAY(@december)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @december)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 12, DATE_ADD(t9.EffectiveDate, INTERVAL -1 DAY), LAST_DAY(@december)),
						IF(YEAR(csp.TimeLine_StartBilling) = reportYear AND MONTH(csp.TimeLine_StartBilling) = 12
							, csp.TimeLine_StartBilling					
							, @december)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfDecember
			-- -------------------------------------------------------------------------------------------------------
		END AS DecTotal,
		-- Tổng doanh thu 12 tháng:
		CASE					
				WHEN csp.TimeLine_StartBilling >= LAST_DAY(@december)
					OR t9.EffectiveDate <= @january
				THEN 0
				ELSE
					-- Tính cước số ngày lẻ trong tháng bắt đầu tính cước nếu ngày tính cước trong năm báo cáo
					(IF(YEAR(csp.TimeLine_StartBilling) = reportYear,
					DATEDIFF(LAST_DAY(csp.TimeLine_StartBilling), csp.TimeLine_StartBilling) + 1,
					0) * IFNULL(t7.PackagePrice, csp.PackagePrice)/DAY(LAST_DAY(csp.TimeLine_StartBilling))) +
					-- Tính cước số ngày lẻ trong tháng thực hiện nâng cấp/hạ gói cước
					IF(t9.EffectiveDate IS NOT NULL, (DAY(t9.EffectiveDate) - 1) * IFNULL(t7.PackagePrice, csp.PackagePrice)/DAY(LAST_DAY(t9.EffectiveDate)) , 0) +
					-- Tính số cước tròn tháng từ lúc bắt đầu tính cước, cho đến lúc nâng cấp/hạ
					(IF(t9.EffectiveDate IS NULL, 13, MONTH(t9.EffectiveDate)) - IF(YEAR(csp.TimeLine_StartBilling) = reportYear, MONTH(csp.TimeLine_StartBilling), 0) - 1) * IFNULL(t7.PackagePrice, csp.PackagePrice)
			END AS AllMonthsTotalNotVAT,
		CASE					
				WHEN csp.TimeLine_StartBilling >= LAST_DAY(@december)
					OR t9.EffectiveDate <= @january
				THEN 0
				ELSE
					-- Tính cước số ngày lẻ trong tháng bắt đầu tính cước nếu ngày tính cước trong năm báo cáo
					(IF(YEAR(csp.TimeLine_StartBilling) = reportYear,
					DATEDIFF(LAST_DAY(csp.TimeLine_StartBilling), csp.TimeLine_StartBilling) + 1,
					0) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + IFNULL(t7.PackagePrice, csp.PackagePrice)*csp.TaxPercent/100)/DAY(LAST_DAY(csp.TimeLine_StartBilling))) +
					-- Tính cước số ngày lẻ trong tháng thực hiện nâng cấp/hạ gói cước
					IF(t9.EffectiveDate IS NOT NULL, (DAY(t9.EffectiveDate) - 1) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + IFNULL(t7.PackagePrice, csp.PackagePrice)*csp.TaxPercent/100)/DAY(LAST_DAY(t9.EffectiveDate)) , 0) +
					-- Tính số cước tròn tháng từ lúc bắt đầu tính cước, cho đến lúc nâng cấp/hạ
					(IF(t9.EffectiveDate IS NULL, 13, MONTH(t9.EffectiveDate)) - IF(YEAR(csp.TimeLine_StartBilling) = reportYear, MONTH(csp.TimeLine_StartBilling), 0) - 1) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + IFNULL(t7.PackagePrice, csp.PackagePrice)*csp.TaxPercent/100)
			END AS CurrentYearSales,		
		-- Doanh thu phát triển mới
-- 		CASE
-- 		WHEN 
-- 			YEAR(csp.TimeLine_StartBilling) <> reportYear
-- 				OR (csp.TimeLine_TerminateDate IS NOT NULL AND YEAR(csp.TimeLine_TerminateDate) < reportYear)
-- 			THEN 0
-- 			WHEN t9.EffectiveDate IS NOT NULL
-- 			THEN
-- 				(DAY(t9.EffectiveDate) - 1) * IFNULL(t7.PackagePrice, csp.PackagePrice)/DAY(LAST_DAY(t9.EffectiveDate))
-- 				+ (MONTH(t9.EffectiveDate) - 1) * IFNULL(t7.PackagePrice, csp.PackagePrice)/DAY(LAST_DAY(t9.EffectiveDate))
-- 			ELSE
-- 				IFNULL(t7.PackagePrice, csp.PackagePrice) * 12
-- 			END	
		0 AS NewRevenueTotal,
		CONCAT_WS('', ct.TimeLine_RenewPeriod, ' tháng') AS TimeLineExpiration,
		csp.CId AS CID,
		CONCAT_WS('', csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom ) AS InternationalBandwidth,
		CONCAT_WS('', csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS DomesticBandwidth,
		t6.GroupName AS ServiceGroupName,
		csp.ServiceName,
		csp.OtherFee,
		csp.InstallationFee,
		IFNULL(t7.PackagePrice, csp.PackagePrice) AS PackagePrice,	
		t8.Reason AS DetailReasonForLiquidation,
		t8.ReasonType AS LiquidationType,
		
		ePoint.InstallationAddress_Street AS Street,
		ePoint.InstallationAddress_District AS District,
		ePoint.InstallationAddress_City AS City,
		ePoint.InstallationAddress_Country AS Country,
		
		sPoint.InstallationAddress_Street AS Street, 
		sPoint.InstallationAddress_District AS District,
		sPoint.InstallationAddress_City AS City, 
		sPoint.InstallationAddress_Country AS Country
	FROM OutContracts AS ct
	INNER JOIN Contractors AS ctor ON ctor.Id = ct.ContractorId
	INNER JOIN OutContractServicePackages AS csp ON ct.Id = csp.OutContractId AND csp.ProjectId IS NULL
	INNER JOIN OutputChannelPoints AS ePoint ON csp.EndPointChannelId = ePoint.Id
	LEFT JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
	LEFT JOIN ContractorProperties AS cp ON cp.ContractorId = ctor.Id
	LEFT JOIN Services AS t5 ON t5.Id = csp.ServiceId
	LEFT JOIN ServiceGroups AS t6 ON t6.Id = t5.GroupId
	-- Phụ lục nâng/hạ gói cước mới nhất tại năm trước năm báo cáo	
	LEFT JOIN (
		SELECT 
			tsp.PackagePrice,
			MAX(ts.EffectiveDate) AS EffectiveDate,
			tsp.OutContractServicePackageId
		FROM TransactionServicePackages tsp
		INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
		WHERE ts.IsDeleted = FALSE AND tsp.IsDeleted = FALSE
		AND ts.StatusId = 4 AND ts.Type = 2
		AND YEAR(ts.EffectiveDate) < reportYear
		AND IFNULL(tsp.IsOld, FALSE) = FALSE
		GROUP BY tsp.OutContractServicePackageId
	) AS t7 ON t7.OutContractServicePackageId = csp.Id
	-- Phụ lục hủy dịch vụ/hợp đồng
	LEFT JOIN (
			SELECT tsp.OutContractServicePackageId,
				ts.ReasonType,
				ts.Reason,
				ts.EffectiveDate
			FROM TransactionServicePackages tsp
			INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
			WHERE (ts.Type = 8 OR ts.Type = 4)
				AND ts.IsDeleted = FALSE AND tsp.IsDeleted = FALSE
				AND ts.StatusId = 4
			GROUP BY tsp.OutContractServicePackageId
		) t8 ON t8.OutContractServicePackageId = csp.Id		
	-- Phụ lục nâng/hạ gói cước trong năm báo cáo gần nhất, hủy dịch vụ hoặc hủy hợp đồng
	LEFT JOIN (
		SELECT
			tsp.PackagePrice AS PackagePrice,
			tsp.OutContractServicePackageId,
			MIN(ts.EffectiveDate) AS EffectiveDate
		FROM TransactionServicePackages tsp
		INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
		WHERE ts.IsDeleted = FALSE AND tsp.IsDeleted = FALSE
		AND ts.StatusId = 4 AND ts.Type IN (2, 4, 8)
		AND YEAR(ts.EffectiveDate) = reportYear
		GROUP BY tsp.OutContractServicePackageId
	) AS t9 ON t9.OutContractServicePackageId = csp.Id
	WHERE (effectiveStartDate IS NULL OR DATE(csp.TimeLine_Effective) >=  effectiveStartDate)
		AND (effectiveEndDate IS NULL OR DATE(csp.TimeLine_Effective) <=  effectiveEndDate)
		AND (IFNULL(marketAreaId, 0) = 0 OR ct.MarketAreaId = marketAreaId)				
		AND (IFNULL(projectId, 0) = 0 OR ct.ProjectId = projectId)
		AND (IFNULL(contractCode, '') = '' OR ct.ContractCode LIKE CONCAT('%', contractCode, '%'))
		AND (IFNULL(customerCode, '') = '' OR csp.CId LIKE CONCAT('%', customerCode, '%'))	
		AND (IFNULL(contractorFullName, '') = '' OR ctor.ContractorFullName LIKE CONCAT('%', contractorFullName, '%'))
		AND csp.ProjectId IS NULL
	ORDER BY ct.Id DESC
	LIMIT take OFFSET skips;
END