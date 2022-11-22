CREATE PROCEDURE `sp_GetReportIncreasementOutContract`(
	IN reportType INT,
	IN startDate date,
	IN endDate date,
	IN isEnterprise TINYINT,
	IN contractCode VARCHAR(100),
	IN organizationUnitId VARCHAR(100),
	IN signedUserId VARCHAR(100),
	IN orderBy VARCHAR(100),	
	IN take INT,
	IN skips INT,
	OUT total INT
	)
BEGIN
	#Routine body goes here...
-- 		DROP TEMPORARY TABLE IF EXISTS tbContracts,tbTransaction;
-- 		CREATE TEMPORARY TABLE tbContracts 
		
        DROP TEMPORARY TABLE IF EXISTS Temp;
        CREATE TEMPORARY TABLE Temp
        
        SELECT
			t1.Id,
			t1.CurrencyUnitId,
			t1.CurrencyUnitCode,
			t2.ContractorFullName AS ContractorName,
			t1.ContractCode,
			'Hợp đồng ký mới' AS TransactionCode,
			t1.TimeLine_Signed AS TimeLineSigned,
			t1.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
			t1.TimeLine_Expiration AS TimeLineExpiration,			
			t3.TimeLine_Effective AS TimeLineEffective,
			t5.`Name` AS UserCategoryName,
		CASE WHEN reportType = 1 THEN t3.PackagePrice * t1.TimeLine_RenewPeriod 
				ELSE t3.PackagePrice / DAY(LAST_DAY(t3.TimeLine_Effective))
							* (DAY(LAST_DAY(t3.TimeLine_Effective)) - DAY(t3.TimeLine_Effective) + 1) 
							+ t3.PackagePrice * (12- MONTH(t3.TimeLine_Effective))
		END AS TotalRevenueS
		
		FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 		
			INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t4 ON t2.IdentityGuid = t4.IdentityGuid
			INNER JOIN ITC_FBM_CRM.CustomerCategories AS t5 ON t5.Id = t4.CustomerCategoryId
		WHERE 1 = 1
  		AND t1.TimeLine_Signed BETWEEN startDate AND endDate
 			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (organizationUnitId = '' OR t1.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
  		AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND t3.StatusId = 0	
		UNION ALL

	#	CREATE TEMPORARY TABLE tbTransaction 
		SELECT 
			t1.OutContractId AS Id ,
			t5.CurrencyUnitId,
			t5.CurrencyUnitCode,
			t2.ContractorFullName AS ContractorName,
			t5.ContractCode,
			t1.`Code` AS TransactionCode,
			t5.TimeLine_Signed AS TimeLineSigned,
			t5.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
			t5.TimeLine_Expiration AS TimeLineExpiration,
			t1.EffectiveDate AS TimeLineEffective,
			t4.`Name` AS UserCategoryName,
			
			CASE WHEN reportType = 1 THEN t6.PackagePrice * t6.TimeLine_PaymentPeriod 
			ELSE t6.PackagePrice / DAY(LAST_DAY(t1.EffectiveDate))
						* (DAY(LAST_DAY(t1.EffectiveDate)) - DAY(t1.EffectiveDate) + 1) 
						+ t6.PackagePrice * (12- MONTH(t1.EffectiveDate))
			END AS TotalRevenueS
		FROM Transactions AS t1
		INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
		INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t3 ON t2.IdentityGuid = t3.IdentityGuid
		INNER JOIN ITC_FBM_CRM.CustomerCategories AS t4 ON t4.Id = t3.CustomerCategoryId
		INNER JOIN OutContracts AS t5 ON t5.Id = t1.OutContractId
		INNER JOIN TransactionServicePackages AS t6 ON t1.Id = t6.TransactionId
		
		WHERE 1 = 1
			AND t1.Type = 1			
  		AND t1.EffectiveDate BETWEEN startDate AND endDate
 			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (organizationUnitId = '' OR t1.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
  		AND (signedUserId = '' OR t5.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 			
		
		UNION ALL

	SELECT 
		tb1.OutContractId AS Id,
		t5.CurrencyUnitId,
		t5.CurrencyUnitCode,
		t2.ContractorFullName AS ContractorName,
		t5.ContractCode,
		tb1.`Code`	AS TransactionCode,		
			t5.TimeLine_Signed AS TimeLineSigned,
			t5.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
			t5.TimeLine_Expiration AS TimeLineExpiration,
			tb1.EffectiveDate AS TimeLineEffective,
			t4.`Name` AS UserCategoryName,
		(tb2.PackagePrice - tb1.PackagePrice)* tb2.TimeLine_PaymentPeriod  AS TotalRevenueS
	FROM 
	(
		 SELECT t1.Id,
			t1.OutContractId,
			t1.ContractorId,
			t1.`Code`,
			t1.Type,
			t1.TransactionDate,
			t1.EffectiveDate,
			t2.PackageName,
			t2.PackagePrice,
			t2.CId,
			t2.IsOld	
			FROM Transactions AS t1 
			INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
			WHERE t1.Type = 2
				AND IsOld = 1 ) AS tb1
	INNER JOIN
		( SELECT t1.Id,
				t1.ContractorId,
				t1.EffectiveDate,
				t1.`Code`,
				t1.Type,
				t1.TransactionDate,
				t2.PackageName,
				t2.PackagePrice,
				t2.TimeLine_PaymentPeriod,
				t2.CId,
				t2.IsOld	
			FROM Transactions AS t1 
			INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
			WHERE t1.Type = 2 	
				AND IsOld = 0
		) AS tb2  ON tb1.CId = tb2.CId AND tb1.Id = tb2.Id
		INNER JOIN Contractors AS t2 ON t2.Id = tb1.ContractorId
		INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t3 ON t2.IdentityGuid = t3.IdentityGuid
		INNER JOIN ITC_FBM_CRM.CustomerCategories AS t4 ON t4.Id = t3.CustomerCategoryId
		INNER JOIN OutContracts AS t5 ON t5.Id = tb1.OutContractId;
		
			SET @sqlQuery = CONCAT( '
				SELECT * ,	ExchangeMoney(t1.TotalRevenueS,t1.CurrencyUnitCode) AS TotalRevenue
				FROM
				Temp AS t1 ',orderBy, ';'	);
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		 SELECT COUNT(*) FROM Temp INTO total;
		DEALLOCATE PREPARE stmt;

END