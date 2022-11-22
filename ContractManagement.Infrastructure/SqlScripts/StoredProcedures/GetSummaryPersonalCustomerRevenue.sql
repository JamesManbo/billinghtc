CREATE PROCEDURE `GetSummaryPersonalCustomerRevenue`(	
		IN `marketAreaId` INT,
		IN `contractCode` VARCHAR(256),
		IN `customerCode` VARCHAR(256),
		IN `contractorFullName` VARCHAR(256),
		IN `serviceId` INT,
		IN `projectId` INT,
		IN `statusId` INT,
		IN `customerCategoryId` INT,
		IN `reportYear` INT,
		IN `outContractIds` TEXT
)
BEGIN

	DROP TEMPORARY TABLE IF EXISTS OutContractIds_Temp;
	DROP TEMPORARY TABLE IF EXISTS OutContractIds_Dup;
	CREATE TEMPORARY TABLE OutContractIds_Temp( VAL INT );
	CREATE TEMPORARY TABLE OutContractIds_Dup( VAL INT );
	
	IF IFNULL(outContractIds, '') = ''
	THEN
		BEGIN		
			INSERT INTO OutContractIds_Temp(VAL)
			SELECT t1.Id
			FROM OutContracts AS t1
			INNER JOIN OutContractServicePackages AS t2 ON t1.Id = t2.OutContractId AND t2.IsDeleted = FALSE AND t2.ProjectId IS NULL
			INNER JOIN Contractors ctor ON t1.ContractorId = ctor.Id
			WHERE (IFNULL(marketAreaId, 0) = 0 OR t1.MarketAreaId = marketAreaId)				
				AND (IFNULL(projectId, 0) = 0 OR t1.ProjectId = projectId)
				AND (IFNULL(contractCode, '') = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%'))
				AND (IFNULL(customerCode, '') = '' OR t2.CId LIKE CONCAT('%', customerCode, '%'))	
				AND (IFNULL(contractorFullName, '') = '' OR ctor.ContractorFullName LIKE CONCAT('%', contractorFullName, '%'))
				AND t2.ProjectId IS NULL;				
		END;
	ELSE
		BEGIN
		DROP TEMPORARY TABLE IF EXISTS OutContractIds;	
		
		CREATE TEMPORARY TABLE OutContractIds(Id TEXT);
		INSERT INTO OutContractIds VALUES(IF(outContractIds IS NULL OR outContractIds = '', '0', outContractIds));	
		
		SET @insertToTempSql = CONCAT("INSERT INTO OutContractIds_Temp (VAL) VALUES ('", REPLACE(( SELECT GROUP_CONCAT(DISTINCT Id) AS DATA FROM OutContractIds), ",", "'),('"),"');");
		PREPARE STMT1 FROM @insertToTempSql;
		EXECUTE STMT1;
		
		DROP TEMPORARY TABLE IF EXISTS OutContractIds;
		END;
	END IF;

	INSERT INTO OutContractIds_Dup(VAL)
	SELECT VAL FROM OutContractIds_Temp;

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
	
-- 	WITH cte(TotalRecords,SumOtherFee,SumInstallationFee,SumPackagePrice,SumTermTotal,SumLastYearSales,JanRevenue,JanNewRevenue,FebRevenue,FebNewRevenue,MarRevenue,MarNewRevenue,AprRevenue,AprNewRevenue,MayRevenue,MayNewRevenue,JunRevenue,JunNewRevenue,JulRevenue,JulNewRevenue,AugRevenue,AugNewRevenue,SepRevenue,SepNewRevenue,OctRevenue,OctNewRevenue,NovRevenue,NovNewRevenue,DecRevenue,DecNewRevenue,AllMonthsTotalNotVAT,SumNewRevenueTotal,SumCurrentYearSales) AS
-- 	
	SELECT 
			SUM(cte.TotalRecords) AS TotalRecords,
			SUM(cte.SumOtherFee) AS SumOtherFee,
			SUM(cte.SumInstallationFee) AS SumInstallationFee,
			SUM(cte.SumPackagePrice) AS SumPackagePrice,
			SUM(cte.SumTermTotal) AS SumTermTotal,
			SUM(cte.SumLastYearSales) AS SumLastYearSales,
 			CONCAT_WS("/", CAST(SUM(cte.JanRevenue) AS DECIMAL(18,2)),CAST(SUM(cte.FebRevenue) AS DECIMAL(18,2)),CAST(SUM(cte.MarRevenue) AS DECIMAL(18,2)),CAST(SUM(AprRevenue) AS DECIMAL(18,2)),CAST(SUM(MayRevenue) AS DECIMAL(18,2)),CAST(SUM(JunRevenue) AS DECIMAL(18,2)),CAST(SUM(JulRevenue) AS DECIMAL(18,2)),CAST(SUM(AugRevenue) AS DECIMAL(18,2)),CAST(SUM(SepRevenue) AS DECIMAL(18,2)),CAST(SUM(OctRevenue) AS DECIMAL(18,2)),CAST(SUM(NovRevenue) AS DECIMAL(18,2)),CAST(SUM(DecRevenue) AS DECIMAL(18,2))) AS SumJoinedEachMonthRevenue,
			
 			CONCAT_WS("/", CAST(SUM(JanNewRevenue) AS DECIMAL(18,2)),CAST(SUM(FebNewRevenue) AS DECIMAL(18,2)),CAST(SUM(MarNewRevenue) AS DECIMAL(18,2)),CAST(SUM(AprNewRevenue) AS DECIMAL(18,2)),CAST(SUM(MayNewRevenue) AS DECIMAL(18,2)),CAST(SUM(JunNewRevenue) AS DECIMAL(18,2)),CAST(SUM(JulNewRevenue) AS DECIMAL(18,2)),CAST(SUM(AugNewRevenue) AS DECIMAL(18,2)),CAST(SUM(SepNewRevenue) AS DECIMAL(18,2)),CAST(SUM(OctNewRevenue) AS DECIMAL(18,2)),CAST(SUM(NovNewRevenue) AS DECIMAL(18,2)),CAST(SUM(DecNewRevenue) AS DECIMAL(18,2))) AS SumJoinedEachMonthNewRevenue,
			SUM(AllMonthsTotalNotVAT) AS SumAllMonthsTotalNotVAT,
			SUM(SumNewRevenueTotal) AS SumNewRevenueTotal,
			SUM(SumCurrentYearSales) AS SumCurrentYearSales
	FROM (
		SELECT
			COUNT(t1.Id) AS TotalRecords,
			SUM(csp.OtherFee) AS SumOtherFee,
			SUM(csp.InstallationFee) AS SumInstallationFee,
			SUM(IFNULL(t7.PackagePrice, csp.PackagePrice)) AS SumPackagePrice,
			SUM(IFNULL(t7.PackagePrice, csp.PackagePrice) * IF(t1.TimeLine_RenewPeriod < 12, t1.TimeLine_RenewPeriod, 12)) AS SumTermTotal,	
			SUM(-- Doanh thu nền năm báo cáo:
			CASE
				WHEN (csp.StatusId = 2 AND YEAR(csp.TimeLine_StartBilling) < reportYear AND YEAR(csp.TimeLine_TerminateDate) >= reportYear)
					OR ((csp.StatusId = 0 OR csp.StatusId = 1) AND YEAR(csp.TimeLine_StartBilling) < reportYear)
				THEN IFNULL(t7.PackagePrice, csp.PackagePrice) * 12
				ELSE 0
			END) AS SumLastYearSales,
			-- Tháng 1:
			SUM(CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@january)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @january)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 1, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@january)),
						IF(csp.TimeLine_StartBilling >= @january, csp.TimeLine_StartBilling, @january)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJanuary
			END) AS JanRevenue,
			SUM(CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@january)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @january)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear OR MONTH(csp.TimeLine_Signed) > 1)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 1, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@january)),
						IF(csp.TimeLine_StartBilling >= @january, csp.TimeLine_StartBilling, @january)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJanuary
			END) AS JanNewRevenue,
			-- Tháng 2:
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@february) 
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @february)
				THEN 0
				ELSE
				(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 2, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@february)),
						IF(csp.TimeLine_StartBilling >= @february, csp.TimeLine_StartBilling, @february)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfFeburary
			END) AS FebRevenue,
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@february)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @february)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 2)
				THEN 0
				ELSE
				(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 2, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@february)),
						IF(csp.TimeLine_StartBilling >= @february, csp.TimeLine_StartBilling, @february)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfFeburary
			END) AS FebNewRevenue,
			-- Tháng 3:
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@march)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @march)
				THEN 0
				ELSE
				(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 3, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@march)),
						IF(csp.TimeLine_StartBilling >= @march, csp.TimeLine_StartBilling, @march)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMarch
			END) AS MarRevenue,
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@march) 
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @march)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 3)
				THEN 0
				ELSE					
				(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 3, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@march)),
						IF(csp.TimeLine_StartBilling >= @march, csp.TimeLine_StartBilling, @march)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMarch
			END) AS MarNewRevenue,
			-- Tháng 4:
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@april) 
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @april)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 4, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@april)),
						IF(csp.TimeLine_StartBilling >= @april, csp.TimeLine_StartBilling, @april)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfApril
			END) AS AprRevenue,
			SUM(
				CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@april)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @april)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 4)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 4, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@april)),
						IF(csp.TimeLine_StartBilling >= @april, csp.TimeLine_StartBilling, @april)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfApril
			END) AS AprNewRevenue,
			-- Tháng 5:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@may)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @may)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 5, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@may)),
						IF(csp.TimeLine_StartBilling >= @may, csp.TimeLine_StartBilling, @may)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMay
			END) AS MayRevenue,
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@may)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @may)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 5)
				THEN 0
				ELSE					
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 5, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@may)),
						IF(csp.TimeLine_StartBilling >= @may, csp.TimeLine_StartBilling, @may)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfMay
			END) AS MayNewRevenue,
			-- Tháng 6:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@june)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @june)
				THEN 0
				ELSE					
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 6, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@june)),
						IF(csp.TimeLine_StartBilling >= @june, csp.TimeLine_StartBilling, @june)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJune
				END) AS JunRevenue,
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@june)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @june)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 6)
				THEN 0
				ELSE					
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 6, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@june)),
						IF(csp.TimeLine_StartBilling >= @june, csp.TimeLine_StartBilling, @june)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJune
				END) AS JunNewRevenue,
			-- Tháng 7:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@july)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @july)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 7, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@july)),
						IF(csp.TimeLine_StartBilling >= @july, csp.TimeLine_StartBilling, @july)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJuly
			END) AS JulRevenue,
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@july)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @july)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 7)
				THEN 0
				WHEN t9.EffectiveDate IS NOT NULL AND MONTH(t9.EffectiveDate) = 7
				THEN
					DATEDIFF(t9.EffectiveDate, @july) * t9.PackagePrice/@dayOfJuly
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 7, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@july)),
						IF(csp.TimeLine_StartBilling >= @july, csp.TimeLine_StartBilling, @july)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfJuly
			END) AS JulNewRevenue,
			-- Tháng 8:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@august)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @august)
				THEN 0
				ELSE					
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 8, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@august)),
						IF(csp.TimeLine_StartBilling >= @august, csp.TimeLine_StartBilling, @august)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfAugust
			END) AS AugRevenue,
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@august)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @august)
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 8)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 8, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@august)),
						IF(csp.TimeLine_StartBilling >= @august, csp.TimeLine_StartBilling, @august)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfAugust
			END) AS AugNewRevenue,
			-- Tháng 9:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@september)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @september) 
					OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 9)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 9, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@september)),
						IF(csp.TimeLine_StartBilling >= @september, csp.TimeLine_StartBilling, @september)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfSeptember
			END) AS SepRevenue,
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling > LAST_DAY(@september)
						OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @september) 
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MONTH(t9.EffectiveDate) = 9, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@september)),
							IF(csp.TimeLine_StartBilling >= @september, csp.TimeLine_StartBilling, @september)) + 1)
						* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfSeptember
				END) AS SepNewRevenue,
			-- Tháng 10:
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling > LAST_DAY(@october)
						OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @october)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MONTH(t9.EffectiveDate) = 10, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@october)),
							IF(csp.TimeLine_StartBilling >= @october, csp.TimeLine_StartBilling, @october)) + 1)
						* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfOctober
				END) AS OctRevenue,
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling > LAST_DAY(@october)
						OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @october)
						OR (YEAR(csp.TimeLine_Signed) <> reportYear AND MONTH(csp.TimeLine_Signed) > 10)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MONTH(t9.EffectiveDate) = 10, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@october)),
							IF(csp.TimeLine_StartBilling >= @october, csp.TimeLine_StartBilling, @october)) + 1)
						* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfOctober
				END) AS OctNewRevenue,
			-- Tháng 11:
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling > LAST_DAY(@november)
						OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @november)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MONTH(t9.EffectiveDate) = 11, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@november)),
							IF(csp.TimeLine_StartBilling >= @november, csp.TimeLine_StartBilling, @november)) + 1)
						* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfNovember
				END) AS NovRevenue,
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling > LAST_DAY(@november)
						OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @november)
						OR MONTH(csp.TimeLine_Signed) > 11
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MONTH(t9.EffectiveDate) = 11, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@november)),
							IF(csp.TimeLine_StartBilling >= @november, csp.TimeLine_StartBilling, @november)) + 1)
						* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfNovember
				END) AS NovNewRevenue,
			-- Tháng 12:
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@december)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @december)
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 12, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@december)),
						IF(csp.TimeLine_StartBilling >= @december, csp.TimeLine_StartBilling, @december)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfDecember
			END) AS DecRevenue,
			SUM(
			CASE					
				WHEN csp.TimeLine_StartBilling > LAST_DAY(@december)
					OR (t9.EffectiveDate IS NOT NULL AND t9.EffectiveDate <= @december)
					OR MONTH(csp.TimeLine_Signed) > 12
				THEN 0
				ELSE
					(DATEDIFF(
						IF(MONTH(t9.EffectiveDate) = 12, DATE_ADD(t9.EffectiveDate,INTERVAL -1 DAY), LAST_DAY(@december)),
						IF(csp.TimeLine_StartBilling >= @december, csp.TimeLine_StartBilling, @december)) + 1)
					* IFNULL(t7.PackagePrice, csp.PackagePrice)/@dayOfDecember
			END) AS DecNewRevenue,
			-- Tổng doanh thu 12 tháng chưa VAT
			SUM(
				CASE					
					WHEN csp.TimeLine_StartBilling >= LAST_DAY(@december)
						OR csp.TimeLine_TerminateDate <= @january
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
			END) AS AllMonthsTotalNotVAT,
			SUM(CASE
					WHEN csp.TimeLine_StartBilling >= LAST_DAY(@december)
						OR csp.TimeLine_TerminateDate <= @january
						OR t9.EffectiveDate <= @january
						OR csp.TimeLine_StartBilling < @january
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
			END) AS SumNewRevenueTotal,
			SUM(CASE
				WHEN csp.TimeLine_StartBilling >= LAST_DAY(@december)
					OR csp.TimeLine_TerminateDate <= @january
					OR t9.EffectiveDate <= @january
				THEN 0
				ELSE
					-- Tính cước số ngày lẻ trong tháng bắt đầu tính cước nếu ngày tính cước trong năm báo cáo
					(IF(YEAR(csp.TimeLine_StartBilling) = reportYear,
					DATEDIFF(LAST_DAY(csp.TimeLine_StartBilling), csp.TimeLine_StartBilling) + 1,
					0) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + (IFNULL(t7.PackagePrice, csp.PackagePrice * csp.TaxPercent)/100))/DAY(LAST_DAY(csp.TimeLine_StartBilling))) +
					-- Tính cước số ngày lẻ trong tháng thực hiện nâng cấp/hạ gói cước
					IF(t9.EffectiveDate IS NOT NULL, (DAY(t9.EffectiveDate) - 1) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + (IFNULL(t7.PackagePrice, csp.PackagePrice * csp.TaxPercent)/100))/DAY(LAST_DAY(t9.EffectiveDate)) , 0) +
					-- Tính số cước tròn tháng từ lúc bắt đầu tính cước, cho đến lúc nâng cấp/hạ
					(IF(t9.EffectiveDate IS NULL, 13, MONTH(t9.EffectiveDate)) - IF(YEAR(csp.TimeLine_StartBilling) = reportYear, MONTH(csp.TimeLine_StartBilling), 0) - 1) * (IFNULL(t7.PackagePrice, csp.PackagePrice) + (IFNULL(t7.PackagePrice, csp.PackagePrice * csp.TaxPercent)/100))
			END) AS SumCurrentYearSales
		FROM OutContracts AS t1
		INNER JOIN OutContractIds_Temp AS tmp ON tmp.VAL = t1.Id
		INNER JOIN OutContractServicePackages AS csp ON csp.OutContractId = t1.Id -- AND csp.ProjectId IS NOT NULL
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
		-- Phụ lục nâng/hạ gói cước trong năm báo cáo gần nhất
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
		-- -------------------------------------------------------------------------------------------------------------------------------------------------
		UNION ALL		
		-- -------------------------------------------------------------------------------------------------------------------------------------------------
		SELECT 
			0 AS SumOtherFee,			
			0 AS TotalRecords,
			0 AS SumInstallationFee,
			0 AS SumPackagePrice,
			0 AS SumTermTotal,
			0 AS SumLastYearSales,
			
			-- Tháng 1
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@january)
						OR t1.EffectiveDate > LAST_DAY(@january)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @january)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@january), LAST_DAY(@january), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @january, t1.EffectiveDate, @january)) + 1)
						* t2.PackagePrice/@dayOfJanuary
				END
			AS JanRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@january)
						OR t1.EffectiveDate > LAST_DAY(@january)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @january)
					THEN 0
					ELSE						
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@january), LAST_DAY(@january), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @january, t1.EffectiveDate, @january)) + 1)
						* t2.PackagePrice/@dayOfJanuary
				END
			AS JanNewRevenue,
			-- Tháng 2
				CASE					
				WHEN osp.TimeLine_StartBilling > LAST_DAY(@february)
					OR t1.EffectiveDate > LAST_DAY(@february)
					OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @february)
				THEN 0
				ELSE					
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@february), LAST_DAY(@feburary), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @feburary, t1.EffectiveDate, @feburary)) + 1)
						* t2.PackagePrice/@dayOfFebruary
			END AS FebRevenue,
				CASE					
				WHEN osp.TimeLine_StartBilling > LAST_DAY(@february)
					OR t1.EffectiveDate > LAST_DAY(@february)
					OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @february)
				THEN 0
				ELSE
					(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@february), LAST_DAY(@feburary), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @feburary, t1.EffectiveDate, @feburary)) + 1)
						* t2.PackagePrice/@dayOfFebruary
			END AS FebNewRevenue,
			-- Tháng 3
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@march)
						OR  t1.EffectiveDate > LAST_DAY(@march)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @march)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@march), LAST_DAY(@march), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @march, t1.EffectiveDate, @march)) + 1)
						* t2.PackagePrice/@dayOfMarch
				END AS MarRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@march)
						OR  t1.EffectiveDate > LAST_DAY(@march)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @march)
						OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 3)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@march), LAST_DAY(@march), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @march, t1.EffectiveDate, @march)) + 1)
						* t2.PackagePrice/@dayOfMarch
				END AS MarNewRevenue,
			-- Tháng 4
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@april)
						OR t1.EffectiveDate > LAST_DAY(@april)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @april)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@april), LAST_DAY(@april), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @april, t1.EffectiveDate, @april)) + 1)
						* t2.PackagePrice/@dayOfApril
				END AS AprRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@april)
						OR t1.EffectiveDate > LAST_DAY(@april)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @april)
						OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 4)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@april), LAST_DAY(@april), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @april, t1.EffectiveDate, @april)) + 1)
						* t2.PackagePrice/@dayOfApril
				END AS AprNewRevenue,
			-- Tháng 5
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@may)
						OR t1.EffectiveDate > LAST_DAY(@may)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @may)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@may), LAST_DAY(@may), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @may, t1.EffectiveDate, @may)) + 1)
						* t2.PackagePrice/@dayOfMay
				END AS MayRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@may)
						OR t1.EffectiveDate > LAST_DAY(@may)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @may)
						OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 5)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@may), LAST_DAY(@may), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @may, t1.EffectiveDate, @may)) + 1)
						* t2.PackagePrice/@dayOfMay
				END AS MayNewRevenue,
			-- Tháng 6
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@june)
						OR t1.EffectiveDate > LAST_DAY(@june)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @june)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@june), LAST_DAY(@june), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @june, t1.EffectiveDate, @june)) + 1)
						* t2.PackagePrice/@dayOfJune
					END
			AS JunRevenue,
				CASE					
				WHEN osp.TimeLine_StartBilling > LAST_DAY(@june)
					OR t1.EffectiveDate > LAST_DAY(@june)
					OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @june)
					OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 6)
				THEN 0
				ELSE
					(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@june), LAST_DAY(@june), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @june, t1.EffectiveDate, @june)) + 1)
						* t2.PackagePrice/@dayOfJune
				END AS JunNewRevenue,
			-- Tháng 7
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@july)
						OR t1.EffectiveDate > LAST_DAY(@july)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @july)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@july), LAST_DAY(@july), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @july, t1.EffectiveDate, @july)) + 1)
						* t2.PackagePrice/@dayOfJuly
				END AS JulRevenue,
				CASE					
						WHEN osp.TimeLine_StartBilling > LAST_DAY(@july)
							OR t1.EffectiveDate > LAST_DAY(@july)
							OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @july)
							OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 7)
						THEN 0
						ELSE
							(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@july), LAST_DAY(@july), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @july, t1.EffectiveDate, @july)) + 1)
						* t2.PackagePrice/@dayOfJuly
					END AS JulNewRevenue,
			-- Tháng 8
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@august)
						OR t1.EffectiveDate > LAST_DAY(@august)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @august)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@august), LAST_DAY(@august), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @august, t1.EffectiveDate, @august)) + 1)
						* t2.PackagePrice/@dayOfAugust
				END AS AugRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@august) 
						OR t1.EffectiveDate > LAST_DAY(@august)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @august)
						OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 8)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@august), LAST_DAY(@august), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @august, t1.EffectiveDate, @august)) + 1)
						* t2.PackagePrice/@dayOfAugust
				END AS AugNewRevenue,
			-- Tháng 9
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@september)
						OR t1.EffectiveDate > LAST_DAY(@september)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @september)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@september), LAST_DAY(@september), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @september, t1.EffectiveDate, @september)) + 1)
						* t2.PackagePrice/@dayOfSeptember
				END AS SepRevenue,
				CASE					
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@september) 
						OR t1.EffectiveDate > LAST_DAY(@september)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @september)
						OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 9)
					THEN 0
					ELSE
						(DATEDIFF(
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@september), LAST_DAY(@september), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
							IF(t1.EffectiveDate >= @september, t1.EffectiveDate, @september)) + 1)
						* t2.PackagePrice/@dayOfSeptember
				END AS SepNewRevenue,
			-- Tháng 10
					CASE					
						WHEN osp.TimeLine_StartBilling > LAST_DAY(@october)
							OR t1.EffectiveDate > LAST_DAY(@october)
							OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @october)
						THEN 0
						ELSE
							(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@october), LAST_DAY(@october), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @october, t1.EffectiveDate, @october)) + 1)
							* t2.PackagePrice/@dayOfOctober
					END AS OctRevenue,
					CASE					
						WHEN osp.TimeLine_StartBilling > LAST_DAY(@october)
							OR t1.EffectiveDate > LAST_DAY(@october)
							OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @october)
							OR (YEAR(t1.EffectiveDate) <> reportYear OR MONTH(t1.EffectiveDate) > 10)
						THEN 0
						ELSE
							(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@october), LAST_DAY(@october), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @october, t1.EffectiveDate, @october)) + 1)
							* t2.PackagePrice/@dayOfOctober
					END AS OctNewRevenue,
			-- Tháng 11
					CASE
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@november)
						OR t1.EffectiveDate > LAST_DAY(@november)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @november)
					THEN 0
					ELSE
						(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@november), LAST_DAY(@november), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @november, t1.EffectiveDate, @november)) + 1)
							* t2.PackagePrice/@dayOfNovember
					END AS NovRevenue,
					CASE
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@november) 
						OR t1.EffectiveDate > LAST_DAY(@november)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @november)
						OR MONTH(t1.EffectiveDate) > 11
					THEN 0
					ELSE
						(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@november), LAST_DAY(@november), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @november, t1.EffectiveDate, @november)) + 1)
							* t2.PackagePrice/@dayOfNovember
					END AS NovNewRevenue,
			-- Tháng 12
					CASE
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@december)
						OR t1.EffectiveDate > LAST_DAY(@december)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @december)
					THEN 0
					ELSE
						(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@december), LAST_DAY(@december), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @december, t1.EffectiveDate, @december)) + 1)
							* t2.PackagePrice/@dayOfDecember
					END AS DecRevenue,			
					CASE
					WHEN osp.TimeLine_StartBilling > LAST_DAY(@december) 
						OR t1.EffectiveDate > LAST_DAY(@december)
						OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @december)
						OR MONTH(t1.EffectiveDate) > 12
					THEN 0
					ELSE
						(DATEDIFF(
								IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL OR MIN(TRAN_TIMELINE.NearestEffectiveDate) > LAST_DAY(@december), LAST_DAY(@december), DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate),INTERVAL -1 DAY)),
								IF(t1.EffectiveDate >= @december, t1.EffectiveDate, @december)) + 1)
							* t2.PackagePrice/@dayOfDecember
					END AS DecNewRevenue,
					
				-- Tổng doanh thu 12 tháng chưa VAT
				CASE
						WHEN osp.TimeLine_StartBilling >= LAST_DAY(@december)
						THEN 0
						ELSE
							(DATEDIFF(LAST_DAY(t1.EffectiveDate), t1.EffectiveDate) + 1) * t2.PackagePrice/DAY(LAST_DAY(t1.EffectiveDate)) + 
							(IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 13, MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate))) - MONTH(t1.EffectiveDate) - 1) * t2.PackagePrice +
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 0, (DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate)) - 1) * t2.PackagePrice / DAY(LAST_DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate))))	
						
				END AS AllMonthsTotalNotVAT,
				CASE
						WHEN t1.EffectiveDate < @january
						THEN 0
						ELSE
							(DATEDIFF(LAST_DAY(t1.EffectiveDate), t1.EffectiveDate) + 1) * t2.PackagePrice/DAY(LAST_DAY(t1.EffectiveDate)) + 
							(IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 13, MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate))) - MONTH(t1.EffectiveDate) - 1) * t2.PackagePrice +
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 0, (DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate)) - 1) * t2.PackagePrice / DAY(LAST_DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate))))
				END AS SumNewRevenueTotal,
				CASE
						WHEN osp.TimeLine_StartBilling >= LAST_DAY(@december)
						THEN 0
						ELSE
							(DATEDIFF(LAST_DAY(t1.EffectiveDate), t1.EffectiveDate) + 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100)/DAY(LAST_DAY(t1.EffectiveDate)) + 
							(IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 13, MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate))) - MONTH(t1.EffectiveDate) - 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100) +
							IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 0, (DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate)) - 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100)/ DAY(LAST_DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate))))	
						
				END AS SumCurrentYearSales
		FROM Transactions AS t1
		INNER JOIN OutContractIds_Dup oc ON oc.VAL = t1.OutContractId
		INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
		INNER JOIN OutContractServicePackages osp ON osp.Id = t2.OutContractServicePackageId	
		LEFT JOIN(
			SELECT ts.EffectiveDate AS NearestEffectiveDate,
			tsp.OutContractServicePackageId
			FROM Transactions AS ts
			INNER JOIN TransactionServicePackages tsp ON tsp.TransactionId = ts.Id
			WHERE ts.StatusId = 4 AND ts.Type IN (2,4,8)
				AND ts.EffectiveDate IS NOT NULL 
				AND YEAR(ts.EffectiveDate) = reportYear
		) TRAN_TIMELINE ON TRAN_TIMELINE.OutContractServicePackageId = osp.Id AND TRAN_TIMELINE.NearestEffectiveDate > t1.EffectiveDate
		WHERE t1.StatusId = 4
			AND t1.Type = 2
			AND t2.IsOld = 0
			AND t1.EffectiveDate IS NOT NULL AND YEAR(t1.EffectiveDate) = reportYear
		GROUP BY t1.Id
	) AS cte;

END