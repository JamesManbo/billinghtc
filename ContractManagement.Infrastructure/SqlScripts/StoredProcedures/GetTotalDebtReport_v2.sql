CREATE PROCEDURE `GetTotalDebtReport`(
	IN `marketAreaId` INT,
		IN `contractCode` VARCHAR(256),
		IN `customerCode` VARCHAR(256),
		IN `contractorFullName` VARCHAR(256),
		IN `serviceId` INT,
		IN `projectId` INT,
		IN `statusId` INT,
		IN `customerCategoryId` INT,
		IN `reportYear` INT,
		IN `outContractIds` MEDIUMTEXT,
		OUT totalDebt DECIMAL(18,2),
		OUT totalPaid DECIMAL(18,2),
		OUT totalClearing DECIMAL(18,2)
)
BEGIN

	DROP TEMPORARY TABLE IF EXISTS DebtOutContractIds_Temp;
	CREATE TEMPORARY TABLE DebtOutContractIds_Temp( VAL INT );
	
	IF IFNULL(outContractIds, '') = ''
	THEN
		BEGIN		
			INSERT INTO DebtOutContractIds_Temp(VAL)
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
		
		SET @insertToTempSql = CONCAT("INSERT INTO DebtOutContractIds_Temp (VAL) VALUES ('", REPLACE(( SELECT GROUP_CONCAT(DISTINCT Id) AS DATA FROM OutContractIds), ",", "'),('"),"');");
		PREPARE STMT1 FROM @insertToTempSql;
		EXECUTE STMT1;
		
		DROP TEMPORARY TABLE IF EXISTS OutContractIds;
		END;
	END IF;
	
	IF reportYear IS NULL
		THEN SET reportYear = CAST(YEAR(CURRENT_DATE) AS SIGNED);
	END IF;
		
	SELECT
		t3.OutContractServicePackageId,
		SUM(IF(t1.StatusId = 4 AND t5.Id IS NOT NULL AND t5.StatusId = 3, t3.GrandTotal, 0)) AS ClearingTotal,
		SUM(IF(t1.StatusId = 4, t3.GrandTotal, 0)) AS PaidTotal,
		SUM(t3.GrandTotal - IF(t1.StatusId = 4, t3.GrandTotal, 0)) AS DebtTotal
	FROM ITC_FBM_Debts.ReceiptVouchers AS t1
	INNER JOIN DebtOutContractIds_Temp AS t2 ON t1.OutContractId = t2.VAL 
	INNER JOIN ITC_FBM_Debts.ReceiptVoucherDetails t3 ON t3.ReceiptVoucherId = t1.Id
	LEFT JOIN ITC_FBM_Debts.Clearings t5 ON t5.Id = t1.ClearingId 
		AND t5.IsDeleted = FALSE
	WHERE 
		t1.IsDeleted = FALSE 
		AND t3.IsDeleted = FALSE
		AND YEAR(t3.EndBillingDate) = reportYear
	GROUP BY
		t3.OutContractServicePackageId;		
		
	SELECT
		SUM(IF(t1.StatusId = 4 AND t5.Id IS NOT NULL AND t5.StatusId = 3, t3.GrandTotal, 0)),
		SUM(IF(t1.StatusId = 4, t3.GrandTotal, 0)),
		SUM(t3.GrandTotal - IF(t1.StatusId = 4, t3.GrandTotal, 0))
	INTO totalClearing, totalPaid, totalDebt
	FROM ITC_FBM_Debts.ReceiptVouchers AS t1
	INNER JOIN DebtOutContractIds_Temp AS t2 ON t1.OutContractId = t2.VAL 
	INNER JOIN ITC_FBM_Debts.ReceiptVoucherDetails t3 ON t3.ReceiptVoucherId = t1.Id
	LEFT JOIN ITC_FBM_Debts.Clearings t5 ON t5.Id = t1.ClearingId 
		AND t5.IsDeleted = FALSE
	WHERE 
		t1.IsDeleted = FALSE 
		AND t3.IsDeleted = FALSE
		AND YEAR(t3.EndBillingDate) = reportYear;

END