CREATE PROCEDURE `sp_GetReportEstimateRevenue`(	
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
	IN currencyUnitId INT,
	IN organizationUnitId VARCHAR(50),
	IN signedUserId VARCHAR(50),	
	IN orderBy VARCHAR(100),	
	IN skips INT,
	IN take INT ,
	OUT total INT
	)
BEGIN
			DECLARE sqlQue LONGTEXT;
			SET sqlQue = CONCAT(
				'SELECT t1.Id, t1.CurrencyUnitCode, t2.ContractorFullName AS ContractorName, t1.ContractCode,t1.TimeLine_Signed AS TimeLineSigned, t1.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
						t1.TimeLine_Expiration AS TimeLineExpiration,t3.ServiceId,t3.ServiceName,t3.PackageName,t3.TimeLine_Effective AS TimeLineEffective,			
					CASE WHEN ',reportType,' = 1 THEN ROUND(ExchangeMoney(t3.PackagePrice, t1.CurrencyUnitCode) * t1.TimeLine_RenewPeriod,0)
							ELSE ROUND(ExchangeMoney(t3.PackagePrice, t1.CurrencyUnitCode) / DAY(LAST_DAY(t3.TimeLine_Effective))* (DAY(LAST_DAY(t3.TimeLine_Effective)) - DAY(t3.TimeLine_Effective) + 1)+ ExchangeMoney(t3.PackagePrice, t1.CurrencyUnitCode) * (12- MONTH(t3.TimeLine_Effective)) ,0)
					END AS TotalRevenue
					FROM OutContracts AS t1
						INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
						INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 
					WHERE 1 = 1
						AND DATE(t1.TimeLine_Signed) BETWEEN  \'',startDate,'\' AND  \'',endDate,'\'
						AND ( ',startEffectDate,' IS NULL OR DATE(t3.TimeLine_Effective) BETWEEN  \'',startEffectDate,'\' AND  \'',endDate,'\' )
 						AND (',marketAreaId,' = 0 OR t1.MarketAreaId = ',marketAreaId,')
 						AND (',currencyUnitId,' = 0 OR t1.CurrencyUnitId = ',currencyUnitId,')
 						AND (',serviceId,' = 0 OR t3.ServiceId = ',serviceId,')
 						AND (',projectId,' = 0 OR t1.ProjectId = ',projectId,')
 						AND (\'',contractCode,'\' = \'\' OR  t1.ContractCode LIKE \'%',contractCode,'%\'',')   					
 						AND (\'',customerCode,'\' = \'\' OR  t2.ContractorCode LIKE \'%',customerCode,'%\'',')   
						AND (',isEnterprise,' = 2 OR t2.IsEnterprise = ',isEnterprise,')					
 						AND (\'',organizationUnitId,'\' = \'\' OR  t1.OrganizationUnitId LIKE \'%',organizationUnitId,'%\'',')   					
 						AND (\'',signedUserId,'\' = \'\' OR  t1.SignedUserId LIKE \'%',signedUserId,'%\'',')   					
						AND t3.StatusId = 0	
						',orderBy, '
						LIMIT ',take,' OFFSET ',skips, '; '    
			);
		SET @sqlQuery := sqlQue; 		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		 SELECT COUNT(*) FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 
		WHERE 1 = 1
 			AND DATE(t1.TimeLine_Signed) BETWEEN  startDate AND endDate
						AND (startEffectDate IS NULL OR DATE(t3.TimeLine_Effective) BETWEEN startEffectDate AND endDate )
 						AND ( marketAreaId = 0 OR t1.MarketAreaId = marketAreaId)
 						AND ( currencyUnitId  = 0 OR t1.CurrencyUnitId = currencyUnitId )
 						AND ( serviceId  = 0 OR t3.ServiceId = serviceId )
 						AND ( projectId  = 0 OR t1.ProjectId = projectId )
 						AND (contractCode = '' OR  t1.ContractCode 		LIKE CONCAT('%', contractCode, '%') ) 
 						AND (customerCode = '' OR  t2.ContractorCode 	LIKE CONCAT('%', ContractorCode, '%') ) 
						AND (isEnterprise  = 2 OR t2.IsEnterprise = isEnterprise )					
 						AND (organizationUnitId = '' OR  t1.OrganizationUnitId LIKE CONCAT('%', OrganizationUnitId, '%') ) 		
 						AND (signedUserId = '' OR  t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 					
						AND t3.StatusId = 0	
		INTO total;
    DEALLOCATE PREPARE stmt;
			 
END