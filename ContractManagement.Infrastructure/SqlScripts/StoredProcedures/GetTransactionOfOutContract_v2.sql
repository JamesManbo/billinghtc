CREATE PROCEDURE `GetTransactionOfOutContract`(
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
	CREATE TEMPORARY TABLE OutContractIds_Temp( VAL INT );
	
	IF IFNULL(outContractIds, '') = ''
	THEN
		BEGIN		
			INSERT INTO OutContractIds_Temp(VAL)
			SELECT t1.Id
			FROM OutContracts AS t1
			INNER JOIN OutContractServicePackages AS t2 ON t1.Id = t2.OutContractId AND t2.IsDeleted = FALSE
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
		t1.OutContractId AS Id,
		t1.Id AS TransactionId,
		reportYear AS ReportYear,
		t1.Type AS TransactionType,
		t1.OutContractId,			
		t1.`Code` AS TransactionCode,
		t1.TransactionDate AS TransactionDate,
		t1.EffectiveDate AS TimeLineEffective,
		t1.ChangingPackageFee,
		t2.PackagePrice	AS PackagePrice,
		MIN(TRAN_TIMELINE.NearestEffectiveDate) AS MIN_NEXT_TRAN_EFFECTIVE,
		-- Doanh thu hàng tháng trong năm báo cáo:
		-- Tháng 1:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@january)
				OR t1.EffectiveDate > LAST_DAY(@january)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @january)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 1
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@january)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 1
						, t1.EffectiveDate
						, @january)) + 1)
				* t2.PackagePrice/@dayOfJanuary
		END AS JanuaryTotal,
		-- Tháng 2:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@february)
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @february)
				OR t1.EffectiveDate > LAST_DAY(@february)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @february)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 2
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@february)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 2
						, t1.EffectiveDate
						, @february)) + 1)
				* t2.PackagePrice/@dayOfFebruary
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS FebruaryTotal,
		-- Tháng 3:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@march)
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @march) 
				OR  t1.EffectiveDate > LAST_DAY(@march)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @march)
			THEN 0
			ELSE				
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 3
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@march)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 3
						, t1.EffectiveDate
						, @march)) + 1)
				* t2.PackagePrice/@dayOfMarch
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS MarTotal,
		-- Tháng 4:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@april) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @april)
				OR t1.EffectiveDate > LAST_DAY(@april)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @april)
			THEN 0
			ELSE		
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 4
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@april)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 4
						, t1.EffectiveDate
						, @april)) + 1)
				* t2.PackagePrice/@dayOfApril
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS AprTotal,
		-- Tháng 5:
		CASE
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@may)
				OR t1.EffectiveDate > LAST_DAY(@may)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @may)
			THEN 0
			ELSE			
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 5
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@may)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 5
						, t1.EffectiveDate
						, @may)) + 1)
				* t2.PackagePrice/@dayOfMay
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS MayTotal,
		-- Tháng 6:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@june) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @june)
				OR t1.EffectiveDate > LAST_DAY(@june)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @june)
			THEN 0
			ELSE	
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 6
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@june)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 6
						, t1.EffectiveDate
						, @june)) + 1)
				* t2.PackagePrice/@dayOfJune
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS JunTotal,
		-- Tháng 7:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@july) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @july)
				OR t1.EffectiveDate > LAST_DAY(@july)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @july)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 7
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@july)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 7
						, t1.EffectiveDate
						, @july)) + 1)
				* t2.PackagePrice/@dayOfJuly
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS JulTotal,
		-- Tháng 8:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@august) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @august)
				OR t1.EffectiveDate > LAST_DAY(@august)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @august)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 8
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@august)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 8
						, t1.EffectiveDate
						, @august)) + 1)
				* t2.PackagePrice/@dayOfAugust
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS AugTotal,
		-- Tháng 9:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@september) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @september)
				OR t1.EffectiveDate > LAST_DAY(@september)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @september)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 9
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@september)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 9
						, t1.EffectiveDate
						, @september)) + 1)
				* t2.PackagePrice/@dayOfSeptember
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS SepTotal,
		-- Tháng 10:
		CASE					
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@october) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @october)
				OR t1.EffectiveDate > LAST_DAY(@october)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @october)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 10
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@october)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 10
						, t1.EffectiveDate
						, @october)) + 1)
				* t2.PackagePrice/@dayOfOctober
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS OctTotal,
		-- Tháng 11:
		CASE
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@november) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @november)
				OR t1.EffectiveDate > LAST_DAY(@november)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @november)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 11
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@november)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 11
						, t1.EffectiveDate
						, @november)) + 1)
				* t2.PackagePrice/@dayOfNovember
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS NovTotal,
		-- Tháng 12:
		CASE
			WHEN osp.TimeLine_StartBilling > LAST_DAY(@december) 
				OR (osp.TimeLine_TerminateDate IS NOT NULL AND osp.TimeLine_TerminateDate <= @december)
				OR t1.EffectiveDate > LAST_DAY(@december)
				OR (MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MIN(TRAN_TIMELINE.NearestEffectiveDate) <= @december)
			THEN 0
			ELSE
				(DATEDIFF(
					IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NOT NULL AND MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate)) = 12
						, DATE_ADD(MIN(TRAN_TIMELINE.NearestEffectiveDate), INTERVAL -1 DAY)
						, LAST_DAY(@december)),
					IF(YEAR(t1.EffectiveDate) = reportYear AND MONTH(t1.EffectiveDate) = 12
						, t1.EffectiveDate
						, @december)) + 1)
				* t2.PackagePrice/@dayOfDecember
				-- ---------------------------------------------------------------------------------------------------------------------
		END AS DecTotal,
		CASE
			WHEN osp.TimeLine_StartBilling >= LAST_DAY(@december)
			THEN 0
			ELSE
				(DATEDIFF(LAST_DAY(t1.EffectiveDate), t1.EffectiveDate) + 1) * t2.PackagePrice/DAY(LAST_DAY(t1.EffectiveDate)) + 
				(IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 13, MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate))) - MONTH(t1.EffectiveDate) - 1) * t2.PackagePrice +
				IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 0, (DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate)) - 1) * t2.PackagePrice / DAY(LAST_DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate))))
		END AS AllMonthsTotalNotVAT,
		CASE
			WHEN osp.TimeLine_StartBilling >= LAST_DAY(@december)
			THEN 0
			ELSE
				(DATEDIFF(LAST_DAY(t1.EffectiveDate), t1.EffectiveDate) + 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100)/DAY(LAST_DAY(t1.EffectiveDate)) + 
				(IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 13, MONTH(MIN(TRAN_TIMELINE.NearestEffectiveDate))) - MONTH(t1.EffectiveDate) - 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100) +
				IF(MIN(TRAN_TIMELINE.NearestEffectiveDate) IS NULL, 0, (DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate)) - 1) * (t2.PackagePrice + t2.PackagePrice * osp.TaxPercent/100) / DAY(LAST_DAY(MIN(TRAN_TIMELINE.NearestEffectiveDate))))
		END AS CurrentYearSales
	FROM Transactions AS t1
	INNER JOIN OutContractIds_Temp oc ON oc.VAL = t1.OutContractId
	INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
	INNER JOIN OutContractServicePackages osp ON osp.Id = t2.OutContractServicePackageId	
	LEFT JOIN (
		SELECT ts.EffectiveDate AS NearestEffectiveDate,
			tsp.OutContractServicePackageId
		FROM Transactions AS ts
		INNER JOIN TransactionServicePackages tsp ON tsp.TransactionId = ts.Id
		WHERE ts.StatusId = 4 AND ts.Type IN (2, 4, 8)
			AND ts.EffectiveDate IS NOT NULL 
			AND YEAR(ts.EffectiveDate) = reportYear
	) TRAN_TIMELINE ON TRAN_TIMELINE.OutContractServicePackageId = osp.Id AND TRAN_TIMELINE.NearestEffectiveDate > t1.EffectiveDate
	WHERE t1.StatusId = 4
		AND t1.Type = 2
		AND t2.IsOld = 0
		AND t1.EffectiveDate IS NOT NULL AND YEAR(t1.EffectiveDate) = reportYear
	GROUP BY t1.Id
	ORDER BY t1.OutContractId, t1.EffectiveDate;
END