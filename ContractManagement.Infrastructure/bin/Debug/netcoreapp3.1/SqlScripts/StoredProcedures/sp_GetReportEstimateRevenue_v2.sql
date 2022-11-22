CREATE PROCEDURE `sp_GetReportEstimateRevenue`(	
	IN startDate date,
	IN endDate date ,
	IN reportType TINYINT,
	IN isEnterprise TINYINT,
	IN organizationUnitId INT,
	IN contractCode VARCHAR(50),
	IN signedUserId VARCHAR(50),
	
	
	IN skips INT,
	IN take INT
	)
BEGIN
		
		#Routine body goes here...
		SELECT
			t1.Id,
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
		END AS TotalRevenue
		
		FROM
			OutContracts AS t1
			INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
			INNER JOIN OutContractServicePackages AS t3 ON t3.OutContractId = t1.Id 
		WHERE 1 = 1
			AND DATE(t1.TimeLine_Signed) BETWEEN  DATE(startDate) AND  DATE(endDate)
			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
			AND (organizationUnitId = 0 OR t1.OrganizationUnitId = organizationUnitId )
			AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			AND t3.StatusId = 0			
;
END