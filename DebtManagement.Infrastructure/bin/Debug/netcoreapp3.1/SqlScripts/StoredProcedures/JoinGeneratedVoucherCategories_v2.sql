CREATE PROCEDURE `JoinGeneratedVoucherCategories`()
BEGIN
	UPDATE ReceiptVouchers rv
	INNER JOIN TemporaryGeneratingVouchers tgv ON rv.IdentityGuid = tgv.ReceiptVoucherId
	INNER JOIN VoucherTargets vt ON vt.IdentityGuid = tgv.VoucherTargetId
	SET rv.TargetId = vt.Id
	WHERE rv.TargetId IS NULL OR rv.TargetId = 0;	
	
	UPDATE ReceiptVoucherDetails rvd
	INNER JOIN TemporaryGeneratingVouchers tgv ON rvd.IdentityGuid = tgv.ReceiptVoucherDetailId
	INNER JOIN ReceiptVouchers rv ON rv.IdentityGuid = tgv.ReceiptVoucherId
	SET rvd.ReceiptVoucherId = rv.Id
	WHERE rvd.ReceiptVoucherId IS NULL OR rvd.ReceiptVoucherId = 0;	
	
	UPDATE ReceiptVoucherDebtHistories rdh
	INNER JOIN TemporaryGeneratingVouchers tgv ON rdh.IdentityGuid = tgv.DebtHistoryId
	INNER JOIN ReceiptVouchers rv ON rv.IdentityGuid = tgv.ReceiptVoucherId
	SET rdh.ReceiptVoucherId = rv.Id
	WHERE rdh.ReceiptVoucherId IS NULL OR rdh.ReceiptVoucherId = 0;
	
	UPDATE ReceiptVoucherLineTaxes rlt
	INNER JOIN TemporaryGeneratingVouchers tgv ON rlt.IdentityGuid = tgv.VoucherTaxId
	INNER JOIN ReceiptVoucherDetails rvd ON rvd.IdentityGuid = tgv.ReceiptVoucherDetailId
	SET rlt.VoucherLineId = rvd.Id
	WHERE rlt.VoucherLineId IS NULL OR rlt.VoucherLineId = 0;
	
	UPDATE PromotionForReceiptVoucher prv
	INNER JOIN TemporaryGeneratingVouchers tgv ON prv.IdentityGuid = tgv.PromotionForVoucherId
	INNER JOIN ReceiptVoucherDetails rvd ON rvd.IdentityGuid = tgv.ReceiptVoucherDetailId
	SET prv.ReceiptVoucherDetailId = rvd.Id, prv.ReceiptVoucherId = rvd.ReceiptVoucherId
	WHERE (prv.ReceiptVoucherDetailId IS NULL OR prv.ReceiptVoucherDetailId = 0) AND (prv.ReceiptVoucherId IS NULL OR prv.ReceiptVoucherId = 0);
	
	DELETE FROM TemporaryGeneratingVouchers;
END