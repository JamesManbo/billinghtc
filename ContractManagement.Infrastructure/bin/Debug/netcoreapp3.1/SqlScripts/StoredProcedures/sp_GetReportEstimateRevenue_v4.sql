CREATE PROCEDURE `sp_GetReportEstimateRevenue`(	
	IN startDate date,
	IN endDate date ,
	IN reportType TINYINT,
	IN isEnterprise TINYINT,
	IN organizationUnitId VARCHAR(50),
	IN contractCode VARCHAR(50),
	IN signedUserId VARCHAR(50),	
	IN orderBy VARCHAR(100),	
	IN skips INT,
	IN take INT ,
	OUT total INT
	)
BEGIN
		
		#Routine body goes here...
		DROP TEMPORARY TABLE 
        IF EXISTS Temp;
        
    CREATE TEMPORARY TABLE Temp
			SELECT
			t1.Id,
			t1.CurrencyUnitCode,
			t2.ContractorFullName AS ContractorName,
			t1.ContractCode,
			t1.TimeLine_Signed AS TimeLineSigned,
			t1.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
			t1.TimeLine_Expiration AS TimeLineExpiration,			
			t3.ServiceId,
			t3.ServiceName,
			t3.PackageName,
			t3.TimeLine_Effective AS TimeLineEffective,			
		CASE WHEN reportType = 1 THEN t3.PackagePrice * t1.TimeLine_RenewPeriod 
				ELSE t3.PackagePrice / DAY(LAST_DAY(t3.TimeLine_Effective))
							* (DAY(LAST_DAY(t3.TimeLine_Effective)) - DAY(t3.TimeLine_Effective) + 1) 
							+ t3.PackagePrice * (12- MONTH(t3.TimeLine_Effective)) 
		END AS TotalRevenueS
		
		FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 
		WHERE 1 = 1
			AND DATE(t1.TimeLine_Signed) BETWEEN  DATE(startDate) AND  DATE(endDate)
			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
			AND (organizationUnitId = '' OR t1.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
 			AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND t3.StatusId = 0	
			LIMIT take OFFSET skips
			;     
			
			SET @sqlQuery = CONCAT( '
				SELECT * ,	ExchangeMoney(t1.TotalRevenueS,t1.CurrencyUnitCode) AS TotalRevenue
				FROM
				Temp AS t1 ',orderBy, ';'	);
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		 SELECT COUNT(*) FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 
		WHERE 1 = 1
 			AND DATE(t1.TimeLine_Signed) BETWEEN  DATE(startDate) AND  DATE(endDate)
 			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
  			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (organizationUnitId = '' OR t1.OrganizationUnitId LIKE CONCAT('%', organizationUnitId, '%') ) 
  			AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			INTO total; 
    DEALLOCATE PREPARE stmt;
			 
END