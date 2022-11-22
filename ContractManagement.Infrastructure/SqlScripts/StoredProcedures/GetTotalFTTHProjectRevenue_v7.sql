CREATE PROCEDURE `GetTotalFTTHProjectRevenue`(	
		IN `skips` INT,
		IN `take` INT,
		IN `marketAreaId` INT,
		IN `contractCode` VARCHAR(50),
		IN `customerCode` VARCHAR(50),
    IN `contractorFullName` VARCHAR(50),
		IN `effectiveStartDate` DATE,
		IN `effectiveEndDate` DATE,
		IN `tvServiceIds` MEDIUMTEXT,
		IN `ftthServiceIds` MEDIUMTEXT,
		IN `projectId` INT,
		IN `reportYear` INT
        )
BEGIN

	DROP TEMPORARY TABLE IF EXISTS FTTHServiceIds;
	DROP TEMPORARY TABLE IF EXISTS TVServiceIds;
	
	CREATE TEMPORARY TABLE FTTHServiceIds( Id VARCHAR(4000) );
	INSERT INTO FTTHServiceIds VALUES(IF(ftthServiceIds IS NULL OR ftthServiceIds = '', '0', ftthServiceIds));
	
	CREATE TEMPORARY TABLE TVServiceIds( Id VARCHAR(4000) );
	INSERT INTO TVServiceIds VALUES(IF(tvServiceIds IS NULL OR tvServiceIds = '', '0', tvServiceIds));

	DROP TEMPORARY TABLE IF EXISTS FTTHServiceIds_Temp;
	CREATE TEMPORARY TABLE FTTHServiceIds_Temp( VAL INT );
	SET @insertToTempSql = CONCAT("INSERT INTO FTTHServiceIds_Temp (VAL) VALUES ('", REPLACE(( SELECT GROUP_CONCAT(DISTINCT Id) AS DATA FROM FTTHServiceIds), ",", "'),('"),"');");
	PREPARE STMT1 FROM @insertToTempSql;
	EXECUTE STMT1;
	
	DROP TEMPORARY TABLE IF EXISTS TVServiceIds_Temp;
	CREATE TEMPORARY TABLE TVServiceIds_Temp( VAL INT );
	SET @insertToTempSql = CONCAT("INSERT INTO TVServiceIds_Temp (VAL) VALUES ('", REPLACE(( SELECT GROUP_CONCAT(DISTINCT Id) AS DATA FROM TVServiceIds), ",", "'),('"),"');");
	PREPARE STMT1 FROM @insertToTempSql;
	EXECUTE STMT1;

	DROP TEMPORARY TABLE IF EXISTS FTTHServiceIds;
	DROP TEMPORARY TABLE IF EXISTS TVServiceIds;
	
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

	
	SELECT DISTINCT
		t1.Id,
		t1.MarketAreaName,
		t1.CityName,
		YEAR (t1.TimeLine_Signed) AS ContractYear,
		t1.ProjectName,
		t1.ContractStatusId, -- Lấy tên trạng thái hợp đồng bằng ValueObjects tại Application Tier
		t4.StatusId, -- Lấy tên trạng thái hợp đồng bằng ValueObjects tại Application Tier		
		IF(t3.IsEnterprise, 'Doanh nghiệp', 'Cá nhân') AS ContractorTypeName,
		t3.ContractorFullName AS ContractorFullName,			 
		t3.ContractorPhone AS PhoneNumber,
		t3.ContractorAddress AS Address,
		t3.ContractorCode AS CustomerCode,
		t1.ContractCode,
		t1.CustomerCareStaffUserId AS CustomerCareStaffUser,
		t1.SignedUserName AS SignedUserName,
		t4.ServiceName,
		t4.CId,
		t4.PackageName AS ServicePackageName,
		CASE
			WHEN IFNULL(t4.TimeLine_PrepayPeriod, 0) > 0 
			THEN t4.TimeLine_PrepayPeriod * t4.PackagePrice
			WHEN IFNULL(t4.TimeLine_PrepayPeriod, 0) = 0 AND t4.TimeLine_StartBilling IS NOT NULL AND t4.TimeLine_StartBilling < CURRENT_DATE 
			THEN t4.TimeLine_PaymentPeriod * t4.PackagePrice
			ELSE 0
		END	AS ChargesCollected1st,
		t1.TimeLine_Signed AS ContractSigningDate,
		t4.TimeLine_Effective AS DateOfAcceptance,
		t4.TimeLine_StartBilling AS DateOfBilling,
		DATE_ADD(t4.TimeLine_StartBilling, INTERVAL 1 YEAR) AS ExpirationDate,
		t4.TimeLine_TerminateDate AS LiquidationDate,
		CASE
			WHEN t4.StatusId <> 2 OR t4.TimeLine_TerminateDate IS NULL OR t4.TimeLine_StartBilling IS NULL THEN ''
			WHEN t4.StatusId = 2 AND t4.TimeLine_TerminateDate < DATE_ADD(t4.TimeLine_StartBilling, INTERVAL 1 YEAR)
			THEN 'Trước hạn'
			WHEN t4.StatusId = 2 AND t4.TimeLine_TerminateDate >= DATE_ADD(t4.TimeLine_StartBilling, INTERVAL 1 YEAR)
			THEN 'Sau hạn'
			ELSE '' 
		END IsAfterTerm,
		t8.Reason AS DetailReasonForLiquidation,
		t8.ReasonType AS LiquidationType,
		IFNULL(t6.OutSharedPackagePercent, 0) AS OutSharedPackagePercent,
		t1.ContractNote AS Note,
		CONCAT_WS('', IFNULL(t4.TimeLine_PrepayPeriod, 0), ' tháng') AS Prepay, -- Trả trước
		CASE
			WHEN pd.Quantity IS NOT NULL OR pd.Quantity > 0
			THEN
				CONCAT_WS('', CAST(FLOOR(12/ t4.TimeLine_PaymentPeriod) * pd.Quantity AS SIGNED), ' tháng')
			ELSE
				'0 tháng'
		END AS MonthsGiven, -- Số tháng được tặng/năm
		CONCAT_WS('', TIMESTAMPDIFF(MONTH, t4.TimeLine_StartBilling, CURRENT_DATE), ' tháng') AS MonthsUse,
		t4.PackagePrice AS MonthlyContractFee,
		CASE
			WHEN ftth.VAL IS NOT NULL THEN IFNULL(t4.PackagePrice, 0)
			ELSE 0
		END AS InternetMonthlyFee,
		CASE
			WHEN tv.VAL IS NOT NULL THEN IFNULL(t4.PackagePrice, 0)
			ELSE 0
		END AS TVMonthlyfee,
		CASE
			WHEN ftth.VAL IS NOT NULL THEN CAST(IFNULL(t6.OutSharedPackagePercent, 0) AS DECIMAL(5,2))
			ELSE 0
		END AS ShareRateForInternet,
		CASE
			WHEN tv.VAL IS NOT NULL THEN CAST(IFNULL(t6.OutSharedPackagePercent, 0) AS DECIMAL(5,2))
		ELSE 0
		END AS ShareRateForTV,
		-- Số tiền thực nhận sau khi phân chia đối tác:
		t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 AS HTCChargesReceivedMonthly,
		
		-- Số tiền thực nhận sau khi trừ khuyến mại:
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
		) AS HTCChargesReceivedAfterPromotion,
		
		-- Doanh thu nền năm báo cáo:
		CASE
			WHEN (t4.StatusId = 2 AND YEAR(t4.TimeLine_StartBilling) < reportYear AND YEAR(t4.TimeLine_TerminateDate) >= reportYear)
				OR ((t4.StatusId = 0 OR t4.StatusId = 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
			THEN (
				t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
				- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
			) * 12
			ELSE 0
		END AS PreYearEconomy,			
		-- Doanh thu mất đi do thanh lý (theo term HĐ)
		-- 
		CASE	
			WHEN t4.StatusId = 2 AND YEAR(t1.TimeLine_Signed) >= reportYear -1 AND TIMESTAMPDIFF(YEAR, t4.TimeLine_StartBilling, t4.TimeLine_TerminateDate) = 0
				 THEN (
						t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
						- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
					) * TIMESTAMPDIFF(DAY, t4.TimeLine_TerminateDate, DATE_ADD(t4.TimeLine_StartBilling, INTERVAL 12 MONTH)) /30
			ELSE 0 
		END LossOfRevenueDueToLiquidation,
		
		-- Doanh thu hàng tháng trong năm báo cáo:
		-- Tháng 1:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@january)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @january) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 1
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@january)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 1
						, t4.TimeLine_StartBilling
						, @january)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfJanuary
		END AS JanRevenue,
		-- Tháng 2:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@february)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @february) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 2
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@february)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 2
						, t4.TimeLine_StartBilling
						, @february)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfFebruary
			END AS FebRevenue,
		-- Tháng 3:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@march)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @march) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 3
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@march)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 3
						, t4.TimeLine_StartBilling
						, @march)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfMarch
		END AS MarRevenue,
		-- Tháng 4:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@april)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @april) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 4
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@april)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 4
						, t4.TimeLine_StartBilling
						, @april)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfApril
			END AS AprRevenue,
		-- Tháng 5:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@may)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @may) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 5
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@may)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 5
						, t4.TimeLine_StartBilling
						, @may)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfMay
		END AS MayRevenue,
		-- Tháng 6:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@june)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @june) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 6
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@june)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 6
						, t4.TimeLine_StartBilling
						, @june)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfJune
		END AS JunRevenue,
		-- Tháng 7:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@july)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @july) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 7
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@july)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 7
						, t4.TimeLine_StartBilling
						, @july)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfJuly
		END AS JulRevenue,
		-- Tháng 8:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@august)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @august) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 8
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@august)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 8
						, t4.TimeLine_StartBilling
						, @august)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfAugust
		END AS AugRevenue,
		-- Tháng 9:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@september)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @september) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 9
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@september)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 9
						, t4.TimeLine_StartBilling
						, @september)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfSeptember
		END AS SepRevenue,
		-- Tháng 10:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@october)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @october) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 10
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@october)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 10
						, t4.TimeLine_StartBilling
						, @october)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfOctober
		END AS OctRevenue,
		-- Tháng 11:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@november)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @november) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 11
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@november)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 11
						, t4.TimeLine_StartBilling
						, @november)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfNovember
		END AS NovRevenue,
		-- Tháng 12:
		CASE
			WHEN t4.TimeLine_StartBilling > LAST_DAY(@december)
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND t4.TimeLine_TerminateDate <= @december) THEN 0
			ELSE (DATEDIFF(
				IF(t4.StatusId = 2 AND t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 12
						, t4.TimeLine_TerminateDate
						, LAST_DAY(@december)),
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 12
						, t4.TimeLine_StartBilling
						, @december)
				) + 1) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ @dayOfDecember
		END AS DecRevenue,
		CASE					
			WHEN (t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
				OR (t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) > reportYear)
			THEN (
				t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
				- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
			) * 12
			WHEN YEAR(t4.TimeLine_StartBilling) = reportYear
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear)
			THEN
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear, DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling), t4.TimeLine_StartBilling) + 1, 0) * 
				(
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ DAY(LAST_DAY(t4.TimeLine_StartBilling)) +
				(CASE
					WHEN t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND YEAR(t4.TimeLine_StartBilling) = reportYear
					THEN MONTH(t4.TimeLine_TerminateDate) - MONTH(t4.TimeLine_StartBilling)
					WHEN t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND YEAR(t4.TimeLine_StartBilling) <> reportYear
					THEN MONTH(t4.TimeLine_TerminateDate) - 1
					ELSE
						12 - MONTH(t4.TimeLine_StartBilling)
				END	
				) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				) +
				IF(t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear
				, (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) *
				(
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))
				, 0)
			ELSE 0
		END AS TotalRevenue,
		
		-- Doanh thu phát triển mới:
		CASE
			WHEN YEAR (t4.TimeLine_Signed) <> reportYear THEN 0
			WHEN (t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
				OR (t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) > reportYear)
			THEN (
				t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
				- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
			) * 12
			WHEN YEAR(t4.TimeLine_StartBilling) = reportYear
				OR (t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear)
			THEN
				IF(YEAR(t4.TimeLine_StartBilling) = reportYear, DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling), t4.TimeLine_StartBilling) + 1, 0) * 
				(
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ DAY(LAST_DAY(t4.TimeLine_StartBilling)) +
				(CASE
					WHEN t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND YEAR(t4.TimeLine_StartBilling) = reportYear
					THEN MONTH(t4.TimeLine_TerminateDate) - MONTH(t4.TimeLine_StartBilling)
					WHEN t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND YEAR(t4.TimeLine_StartBilling) <> reportYear
					THEN MONTH(t4.TimeLine_TerminateDate) - 1
					ELSE
						12 - MONTH(t4.TimeLine_StartBilling)
				END	
				) * (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				) +
				IF(t4.TimeLine_TerminateDate IS NOT NULL AND YEAR(t4.TimeLine_TerminateDate) = reportYear
				, (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) *
				(
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
					- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 12
				)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))
				, 0)
			ELSE 0
		END AS NewGrowthRevenue,
		-- Giảm nền năm báo cáo:
		0 AS EconomyReductionLastYear
		FROM OutContracts AS t1
		INNER JOIN Contractors AS t3 ON t3.Id = t1.ContractorId
		INNER JOIN OutContractServicePackages AS t4 ON t4.OutContractId = t1.Id AND t4.ProjectId IS NOT NULL
		LEFT JOIN (
			SELECT t5.OutContractServicePackageId,
				SUM(pmd.Quantity) AS Quantity
			FROM PromotionForContracts AS t5
			INNER JOIN PromotionDetails AS pmd ON pmd.Id = t5.PromotionDetailId
			INNER JOIN OutContractServicePackages AS osp ON t5.OutContractServicePackageId = osp.Id AND osp.ProjectId IS NOT NULL
			WHERE
				t5.PromotionType = 2 AND t5.IsApplied = TRUE 
				AND t5.PromotionValueType = 3 AND t5.IsDeleted = FALSE	
				AND pmd.IsDeleted = FALSE AND pmd.MinPaymentPeriod <= osp.TimeLine_PaymentPeriod
				AND (IFNULL(projectId, 0) = 0 OR osp.ProjectId = projectId)
			GROUP BY t5.OutContractServicePackageId
		) AS pd ON pd.OutContractServicePackageId = t4.Id
		LEFT JOIN (
			SELECT OutServiceChannelId, SUM(IFNULL(OutSharedPackagePercent, 0)) AS OutSharedPackagePercent
			FROM ContractSharingRevenueLines
			WHERE SharingType IN (2, 3) AND IsDeleted = FALSE
			GROUP BY OutServiceChannelId
		) AS t6 ON t6.OutServiceChannelId = t4.Id
		LEFT JOIN (
			SELECT tsp.OutContractServicePackageId,
				ts.ReasonType,
				ts.Reason
			FROM TransactionServicePackages tsp
			INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
			WHERE (ts.Type = 8 OR ts.Type = 4)
				AND ts.IsDeleted = FALSE AND tsp.IsDeleted = FALSE
				AND ts.StatusId = 4
			GROUP BY tsp.OutContractServicePackageId
		) t8 ON t8.OutContractServicePackageId = t4.Id
		LEFT JOIN TVServiceIds_Temp tv ON tv.VAL = t4.ServiceId			
		LEFT JOIN FTTHServiceIds_Temp ftth ON ftth.VAL = t4.ServiceId
		WHERE (effectiveStartDate IS NULL OR DATE(t4.TimeLine_Effective) >=  effectiveStartDate)
			AND (effectiveEndDate IS NULL OR DATE(t4.TimeLine_Effective) <=  effectiveEndDate)
			AND (IFNULL(marketAreaId, 0) = 0 OR t1.MarketAreaId = marketAreaId)
			AND (IFNULL(projectId, 0)  = 0 OR t4.ProjectId = projectId )
			AND (IFNULL(contractCode, '') = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%'))
			AND (IFNULL(customerCode, '') = '' OR t4.CId LIKE CONCAT('%', customerCode, '%'))
			AND (ftth.VAL IS NOT NULL OR tv.VAL IS NOT NULL)
		ORDER BY t4.ProjectId DESC
		LIMIT take OFFSET skips;
	END