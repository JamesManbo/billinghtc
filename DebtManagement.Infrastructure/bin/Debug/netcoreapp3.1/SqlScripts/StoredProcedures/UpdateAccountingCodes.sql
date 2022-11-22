CREATE PROCEDURE `UpdateAccountingCodes`()
BEGIN
	SET SQL_SAFE_UPDATES = 0;
        
	UPDATE ITC_FBM_Contracts.OutContracts t1
	INNER JOIN ITC_FBM_Contracts.Contractors t2 ON t2.Id = t1.ContractorId
	INNER JOIN ITC_FBM_CRM.ApplicationUsers t3 ON t3.IdentityGuid = t2.ApplicationUserIdentityGuid
	SET t1.AccountingCustomerCode = t3.AccountingCustomerCode
	WHERE t3.AccountingCustomerCode IS NOT NULL 
		AND t3.AccountingCustomerCode <> '' 
		AND (t1.AccountingCustomerCode IS NULL OR t1.AccountingCustomerCode LIKE '');


	UPDATE ITC_FBM_Debts.ReceiptVouchers t1
	INNER JOIN ITC_FBM_Contracts.OutContracts oc ON t1.OutContractId = oc.Id
	SET t1.AccountingCode = oc.AccountingCustomerCode
	WHERE t1.AccountingCode IS NULL OR t1.AccountingCode = '';
        
	SET SQL_SAFE_UPDATES = 1;
END