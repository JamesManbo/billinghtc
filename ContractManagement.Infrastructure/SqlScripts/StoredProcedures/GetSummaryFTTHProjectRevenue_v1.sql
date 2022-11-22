CREATE PROCEDURE `GetSummaryFTTHProjectRevenue`(
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
	
	CREATE TEMPORARY TABLE FTTHServiceIds( Id NVARCHAR(10) );
	INSERT INTO FTTHServiceIds VALUES(IF(ftthServiceIds IS NULL OR ftthServiceIds = '', '0', ftthServiceIds));
	
	CREATE TEMPORARY TABLE TVServiceIds( Id NVARCHAR(10) );
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
	
	SELECT
	COUNT(t1.Id) AS TotalRecords,
	SUM(t4.PackagePrice) AS SumMonthlyContractFee,
	SUM(CASE
		WHEN ftth.VAL IS NOT NULL THEN IFNULL(t4.PackagePrice, 0)
		ELSE 0
	END) AS SumInternetMonthlyFee,
	SUM(CASE
		WHEN tv.VAL IS NOT NULL THEN IFNULL(t4.PackagePrice, 0)
		ELSE 0
	END) AS SumTVMonthlyfee,
	-- Số tiền thực nhận sau khi phân chia đối tác:
	SUM(t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100) AS SumHTCChargesReceivedMonthly,		
	-- Số tiền thực nhận sau khi trừ khuyến mại:
	SUM(
		t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
		- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
	) AS SumHTCChargesReceivedAfterPromotion,
	SUM(CASE
			WHEN IFNULL(t4.TimeLine_PrepayPeriod, 0) > 0 
			THEN t4.TimeLine_PrepayPeriod * t4.PackagePrice
			WHEN IFNULL(t4.TimeLine_PrepayPeriod, 0) = 0 AND t4.TimeLine_StartBilling IS NOT NULL AND t4.TimeLine_StartBilling < CURRENT_DATE 
			THEN t4.TimeLine_PaymentPeriod * t4.PackagePrice
			ELSE 0
		END) AS SumChargesCollected1st,				
	-- Doanh thu mất đi do thanh lý (theo term HĐ)
	-- 
	SUM(CASE	
		WHEN t4.StatusId = 2 AND YEAR(t1.TimeLine_Signed) >= reportYear -1 AND TIMESTAMPDIFF(YEAR, t4.TimeLine_StartBilling, t4.TimeLine_TerminateDate) = 0
			 THEN (
					t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
						- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
				) * TIMESTAMPDIFF(DAY, t4.TimeLine_TerminateDate, DATE_ADD(t4.TimeLine_StartBilling, INTERVAL 12 MONTH)) /30
		ELSE 0 
	END) SumLossOfRevenueDueToLiquidation,
	-- Doanh thu nền năm báo cáo:
	SUM(CASE
		WHEN (t4.StatusId = 2 AND YEAR(t4.TimeLine_StartBilling) < reportYear AND YEAR(t4.TimeLine_TerminateDate) >= reportYear)
			OR ((t4.StatusId = 0 OR t4.StatusId = 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		) * 12
		ELSE 0
	END) AS SumPreYearEconomy,
	0 AS SumEconomyReductionLastYear,
	-- Giảm nền năm báo cáo:
	0 AS SumEconomyReductionLastYear,
	-- Doanh thu phát triển mới:
	SUM(CASE
		WHEN YEAR (t1.TimeLine_Signed) <> reportYear THEN 0
		WHEN (t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
			OR (t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) > reportYear)
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		) * 12
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear
		THEN
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		) * (12 - MONTH(t4.TimeLine_StartBilling)) +
		((DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling)))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear
		THEN
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
				- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		) * (MONTH(t4.TimeLine_TerminateDate) - 1) +
		((DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate)))
		ELSE 0
	END) AS SumNewGrowthRevenue,
	SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) < reportYear)
			OR (t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) > reportYear)
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		) * 12
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear
		THEN
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		) * (12 - MONTH(t4.TimeLine_StartBilling)) +
		((DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling)))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear
		THEN
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
				- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		) * (MONTH(t4.TimeLine_TerminateDate) - 1) +
		((DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate)))
		ELSE 0
	END) AS SumTotalRevenue,
	-- Doanh thu hàng tháng trong năm báo cáo:
	-- Tháng 1:
	CONCAT_WS('',
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 1))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 1)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 1
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 1
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 2:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 2))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 2)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 2
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 2
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 3:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 3))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 3)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 3
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 3
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 4:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 4))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 4)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 4
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 4
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 5:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 5))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 5)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 5
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 5
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 6:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 6))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 6)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 6
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 6
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 7:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 7))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 7)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 7
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 7
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 8:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 8))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 8)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 8
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 8
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 9:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 9))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 9)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 9
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 9
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 10:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 10))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 10)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 10
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 10
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 11:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 11))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 11)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 11
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 11
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2)),
	 "/",
	-- Tháng 12:
	CAST(SUM(CASE					
		WHEN (t4.StatusId IN (0, 1) AND (YEAR(t4.TimeLine_StartBilling) < reportYear) OR (YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) < 12))
			OR (t4.StatusId = 2 AND (YEAR(t4.TimeLine_TerminateDate) > reportYear OR (YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) > 12)))
		THEN (
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1
		)
		WHEN t4.StatusId IN (0, 1) AND YEAR(t4.TimeLine_StartBilling) = reportYear AND MONTH(t4.TimeLine_StartBilling) = 12
		THEN (DATEDIFF(LAST_DAY(t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_StartBilling))
		WHEN t4.StatusId = 2 AND YEAR(t4.TimeLine_TerminateDate) = reportYear AND MONTH(t4.TimeLine_TerminateDate) = 12
		THEN (DATEDIFF(t4.TimeLine_TerminateDate, FIRST_DAY(t4.TimeLine_TerminateDate)) + 1) * 
		(
			t4.PackagePrice * CAST((100 - IFNULL(t6.OutSharedPackagePercent, 0)) AS DECIMAL(5,2))/ 100 
			- FLOOR(12/ t4.TimeLine_PaymentPeriod) * IFNULL(pd.Quantity, 0) * t4.PackagePrice / 1 
		)/ DAY(LAST_DAY(t4.TimeLine_TerminateDate))	
		ELSE 0 
	END) AS DECIMAL(18,2))
	) AS SumJoinedEachMonthRevenue
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
				AND pmd.IsDeleted = FALSE
				AND pmd.MinPaymentPeriod <= osp.TimeLine_PaymentPeriod
				AND (IFNULL(projectId, 0) = 0 OR osp.ProjectId = projectId)
			GROUP BY t5.OutContractServicePackageId
		) AS pd ON pd.OutContractServicePackageId = t4.Id
	LEFT JOIN (
		SELECT OutServiceChannelId, SUM(IFNULL(OutSharedPackagePercent, 0)) AS OutSharedPackagePercent
		FROM ContractSharingRevenueLines  
		WHERE SharingType IN (2, 3) AND IsDeleted = FALSE
		GROUP BY OutServiceChannelId
	) AS t6 ON t6.OutServiceChannelId = t4.Id
	LEFT JOIN TVServiceIds_Temp tv ON tv.VAL = t4.ServiceId			
	LEFT JOIN FTTHServiceIds_Temp ftth ON ftth.VAL = t4.ServiceId
	WHERE (effectiveStartDate IS NULL OR DATE(t4.TimeLine_Effective) >=  effectiveStartDate)
		AND (effectiveEndDate IS NULL OR DATE(t4.TimeLine_Effective) <=  effectiveEndDate)
		AND (IFNULL(marketAreaId, 0) = 0 OR t1.MarketAreaId = marketAreaId)
		AND (IFNULL(projectId, 0)  = 0 OR t4.ProjectId = projectId )
		AND (IFNULL(contractCode, '') = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%'))
		AND (IFNULL(customerCode, '') = '' OR t4.CId LIKE CONCAT('%', customerCode, '%'))
		AND (ftth.VAL IS NOT NULL OR tv.VAL IS NOT NULL);
END