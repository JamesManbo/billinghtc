CREATE PROCEDURE `sp_GetReportIncreasementOutContract`(
	IN startDate date,
	IN endDate date ,
	IN startEffectDate date ,
	IN endEffectDate date ,
	IN reportType TINYINT,
	IN isEnterprise TINYINT,
	IN marketAreaId INT,
	IN serviceId INT,
	IN projectId INT,
	IN contractCode VARCHAR(50),
	IN customerCode VARCHAR(50),
	IN transactionCode VARCHAR(50),
	
	IN organizationUnitId VARCHAR(50),
	IN signedUserId VARCHAR(50),	
	IN orderBy VARCHAR(100),	
	IN skips INT,
	IN take INT ,
	OUT total INT
	)
BEGIN
#Routine body goes here...		
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
			t4.ContractorCategoryName AS ContractorCategoryName,
		CASE WHEN reportType = 1 THEN ROUND(SUM(t3.PackagePrice * t1.TimeLine_RenewPeriod ) ,0)
				ELSE ROUND(SUM(t3.PackagePrice / DAY(LAST_DAY(t3.TimeLine_Effective))
							* (DAY(LAST_DAY(t3.TimeLine_Effective)) - DAY(t3.TimeLine_Effective) + 1) 
							+ t3.PackagePrice * (12- MONTH(t3.TimeLine_Effective))) ,0)
		END AS TotalRevenueS
		
		FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 		
			LEFT JOIN ContractorProperties AS t4 ON t4.ContractorId = t2.Id
		WHERE 1 = 1
  		AND DATE(t1.TimeLine_Signed) BETWEEN DATE(startDate) AND DATE(endDate)
 			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)			
			AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId)
			AND (serviceId  = 0 OR t3.ServiceId = serviceId )
 			AND (projectId  = 0 OR t1.ProjectId = projectId )
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (customerCode = '' OR t2.ContractorCode LIKE CONCAT('%', customerCode, '%') )  			
 			AND (organizationUnitId = '' OR t1.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
  		AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND t3.StatusId = 0	
			
 			GROUP BY 	t1.Id,
 			t1.CurrencyUnitId
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
			t4.ContractorCategoryName AS ContractorCategoryName,
			
			CASE WHEN reportType = 1 THEN t6.PackagePrice * t6.TimeLine_PaymentPeriod 
			ELSE t6.PackagePrice / DAY(LAST_DAY(t1.EffectiveDate))
						* (DAY(LAST_DAY(t1.EffectiveDate)) - DAY(t1.EffectiveDate) + 1) 
						+ t6.PackagePrice * (12- MONTH(t1.EffectiveDate))
			END AS TotalRevenueS
		FROM Transactions AS t1
		INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
		LEFT JOIN ContractorProperties AS t4 ON t4.ContractorId = t2.Id
		INNER JOIN OutContracts AS t5 ON t5.Id = t1.OutContractId
		INNER JOIN TransactionServicePackages AS t6 ON t1.Id = t6.TransactionId
		
		WHERE 1 = 1
			AND t1.Type = 1			
  		AND DATE(t1.EffectiveDate) BETWEEN DATE(startEffectDate) AND DATE(endEffectDate)
 			AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId)
			AND (serviceId  = 0 OR t6.ServiceId = serviceId )
 			AND (projectId  = 0 OR t1.ProjectId = projectId )
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (customerCode = '' OR t2.ContractorCode LIKE CONCAT('%', customerCode, '%') ) 
 			AND (transactionCode = '' OR t1.`Code` LIKE CONCAT('%', transactionCode, '%') ) 
			AND (signedUserId = '' OR t5.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND (organizationUnitId = '' OR t5.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
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
			t4.ContractorCategoryName AS ContractorCategoryName,
			tb2.totalFee + (tb2.PackagePrice - tb1.PackagePrice)* tb2.TimeLine_PaymentPeriod  AS TotalRevenueS
	FROM  (  SELECT t1.Id,
				t1.OutContractId,
				t1.ContractorId,
				t1.`Code`,
				t1.Type,
				t1.StatusId,
				t2.ServiceId,
				t1.TransactionDate,
				t1.EffectiveDate,
				t2.PackageName,
				t2.PackagePrice,
				t2.CId,
				t2.IsOld	
				FROM Transactions AS t1 
				INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
				WHERE t1.Type = 2
					AND IsOld = 1
			) AS tb1
		INNER JOIN ( SELECT t1.Id,
						t1.ContractorId,
						t1.`Code`,
						t1.Type,
						t1.TransactionDate,
						t1.EffectiveDate,
						t2.PackageName,
						t2.PackagePrice,
						t2.TimeLine_PaymentPeriod,
						t2.CId,
						t2.IsOld	,
						t2.OtherFee + t2.InstallationFee AS totalFee
					FROM Transactions AS t1 
					INNER JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
					WHERE t1.Type = 2 	 AND IsOld = 0
					) AS tb2  ON tb1.CId = tb2.CId AND tb1.Id = tb2.Id
		INNER JOIN Contractors AS t2 ON t2.Id = tb1.ContractorId
		LEFT JOIN ContractorProperties AS t4 ON t4.ContractorId = t2.Id
		INNER JOIN OutContracts AS t5 ON t5.Id = tb1.OutContractId
		
		WHERE 1=1
			AND DATE(tb1.EffectiveDate) BETWEEN DATE(startEffectDate) AND DATE(endEffectDate)
			AND (tb2.totalFee > 0 OR tb2.PackagePrice > tb1.PackagePrice)
			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)			
			AND (marketAreaId = 0 OR t5.MarketAreaId = marketAreaId)
			AND (serviceId  = 0 OR tb1.ServiceId = serviceId )
 			AND (projectId  = 0 OR t5.ProjectId = projectId )
 			AND (contractCode = '' OR t5.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (customerCode = '' OR t2.ContractorCode LIKE CONCAT('%', customerCode, '%') ) 
 			AND (transactionCode = '' OR tb1.`Code` LIKE CONCAT('%', transactionCode, '%') )  			
 			AND (organizationUnitId = '' OR t5.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
  		AND (signedUserId = '' OR t5.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND tb1.StatusId = 0	
		;
		
			SET @sqlQuery = CONCAT( '
				SELECT t1.Id,
					t1.CurrencyUnitId,
					t1.CurrencyUnitCode,
					t1.ContractorName,
					t1.ContractCode, 
					t1.TransactionCode,		
					t1.TimeLineSigned,
					t1.TimeLineRenewPeriod,
					t1.TimeLineExpiration,
					t1.TimeLineEffective,
					t1.ContractorCategoryName,
					ExchangeMoney(t1.TotalRevenueS,t1.CurrencyUnitCode) AS TotalRevenue
				FROM
				Temp AS t1 ',orderBy, '	LIMIT ',take,' OFFSET ',skips, '; ' );
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		 SELECT COUNT(*) FROM Temp INTO total;
		DEALLOCATE PREPARE stmt;
END