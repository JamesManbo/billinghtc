CREATE PROCEDURE `sp_GetReportIncreasementOutContract`(
	IN reportType INT,
	IN startDate date,
	IN endDate date,
	IN isEnterprise TINYINT,
	IN contractCode VARCHAR(100),
	IN organizationUnitId INT,
	IN signedUserId VARCHAR(100)
	)
BEGIN
	#Routine body goes here...
-- 		DROP TEMPORARY TABLE IF EXISTS tbContracts,tbTransaction;
-- 		CREATE TEMPORARY TABLE tbContracts 
SELECT
			t1.Id AS OutContractId,
			t2.ContractorFullName AS ContractorName,
			t1.ContractCode,
			t1.TimeLine_Signed AS TimeLineSigned,
			t1.TimeLine_RenewPeriod AS TimeLineRenewPeriod,
			t1.TimeLine_Expiration AS TimeLineExpiration,			
			t3.TimeLine_Effective AS TimeLineEffective,
			t5.`Name` AS UserCategoryName,
		CASE WHEN reportType = 1 THEN t3.PackagePrice * t1.TimeLine_RenewPeriod 
				ELSE t3.PackagePrice / DAY(LAST_DAY(t3.TimeLine_Effective))
							* (DAY(LAST_DAY(t3.TimeLine_Effective)) - DAY(t3.TimeLine_Effective) + 1) 
							+ t3.PackagePrice * (12- MONTH(t3.TimeLine_Effective))
		END AS TotalRevenue
		
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
 			AND (organizationUnitId = 0 OR t1.OrganizationUnitId = organizationUnitId )
  		AND (signedUserId = '' OR t1.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			
		UNION ALL

	#	CREATE TEMPORARY TABLE tbTransaction 
		SELECT 
			t1.Id AS OutContractId,
			t2.ContractorFullName AS ContractorName,
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
			END AS TotalRevenue
		FROM Transactions AS t1
		INNER JOIN Contractors AS t2 ON t2.Id = t1.ContractorId
		INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t3 ON t2.IdentityGuid = t3.IdentityGuid
		INNER JOIN ITC_FBM_CRM.CustomerCategories AS t4 ON t4.Id = t3.CustomerCategoryId
		INNER JOIN OutContracts AS t5 ON t5.Id = t1.OutContractId
		INNER JOIN TransactionServicePackages AS t6 ON t1.Id = t6.TransactionId
		
		WHERE 1 = 1
  		AND t5.TimeLine_Signed BETWEEN startDate AND endDate
 			AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
 			AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 			AND (organizationUnitId = 0 OR t1.OrganizationUnitId = organizationUnitId )
  		AND (signedUserId = '' OR t5.SignedUserId LIKE CONCAT('%', signedUserId, '%') ) 
			
		;

		
END