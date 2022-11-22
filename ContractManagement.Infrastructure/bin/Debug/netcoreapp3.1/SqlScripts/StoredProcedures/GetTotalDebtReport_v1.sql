CREATE PROCEDURE `GetTotalDebtReport`(
	IN p_OutContractId MEDIUMTEXT
)
BEGIN
	SELECT
	t1.OutContractId,
	SUM( t1.ClearingTotal ) AS ClearingTotal,
	SUM( t1.RemainingTotal ) AS DebtTotal,
	SUM( t1.PaidTotal ) AS PaidTotal,
	t2.OpeningDebtAmount 
	FROM
		ITC_FBM_Debts.ReceiptVouchers AS t1
		INNER JOIN ( 
			SELECT OutContractId, OpeningDebtAmount, MAX( CreatedDate ) AS CreatedDate 
			FROM ITC_FBM_Debts.ReceiptVouchers 
			GROUP BY OutContractId 
			) AS t2 
			ON t1.OutContractId = t2.OutContractId 
	WHERE t1.OutContractId IN (p_OutContractId)
	GROUP BY
	t1.OutContractId;

END