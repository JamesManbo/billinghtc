
CREATE PROCEDURE `UpdateCurrentDebtOfTarget` ()
BEGIN
	UPDATE VoucherTargets v
	INNER JOIN (
		SELECT rv.TargetId, SUM(rv.TargetDebtRemaningTotal) AS DebtAmount
		FROM ReceiptVouchers rv
		WHERE rv.StatusId NOT IN (5,7)
			AND rv.IsDeleted <> TRUE
			AND rv.IsActive = TRUE
			AND rv.InvalidIssuedDate <> TRUE
		GROUP BY rv.TargetId
	) d ON d.TargetId = v.Id
	SET v.CurrentDebt = d.DebtAmount
	WHERE v.IsDeleted <> TRUE;
END
