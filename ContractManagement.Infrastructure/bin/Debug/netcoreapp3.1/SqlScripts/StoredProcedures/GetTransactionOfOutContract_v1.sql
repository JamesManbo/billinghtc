CREATE PROCEDURE `GetTransactionOfOutContract`()
BEGIN
	SELECT
	t1.Id,
		t2.Id AS TransactionId,
		t2.OutContractId,			
		t2.`Code`,
		t2.Type,
		t2.EffectiveDate ,
		t3.Id AS TransactionServicePackageId,			
		t3.IsOld,
		t3.TimeLine_Effective,
		t3.TimeLine_TerminateDate,
		t3.InstallationFee,
		t3.PackagePrice			
		FROM  OutContracts AS t1 
		LEFT JOIN Transactions AS t2  ON t1.Id = t2.OutContractId
		LEFT JOIN TransactionServicePackages AS t3 ON t3.TransactionId = t2.Id			
		WHERE 1 = 1 				
			AND t2.Type IN (1,2,3,8,11)
			AND t2.StatusId = 4
		ORDER BY t2.OutContractId,t2.Type,t2.EffectiveDate;
END