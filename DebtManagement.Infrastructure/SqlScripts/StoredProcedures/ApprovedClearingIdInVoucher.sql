CREATE PROCEDURE `ApprovedClearingIdInVoucher`(
IN 	clearingId varchar(68),
	updatedBy varchar(255))
BEGIN
	SET SQL_SAFE_UPDATES = 0;
        
	UPDATE receiptvouchers rv
	INNER JOIN clearingreceiptdetails crd ON rv.Id = crd.ReceiptVoucherId AND crd.ClearingId = clearingId AND crd.IsDeleted = 0
	SET rv.StatusId = 4, -- Đã thanh toán
    rv.IsLock = 0,
    rv.ClearingTotal = rv.ClearingTotal + crd.ClearingAmount,
    rv.UpdatedBy = updatedBy,
    rv.UpdatedDate = NOW()
    WHERE rv.IsDeleted = 0;
        
        
	UPDATE paymentvouchers pv
	INNER JOIN clearingpaymentdetails cpd ON pv.Id = cpd.PaymentVoucherId AND cpd.ClearingId = clearingId AND cpd.IsDeleted = 0
	SET pv.StatusId = 4, -- Đã thanh toán
    pv.IsLock = 0,
    pv.ClearingTotal = pv.ClearingTotal + cpd.ClearingAmount,
    pv.UpdatedBy = updatedBy,
    pv.UpdatedDate = NOW()
    WHERE pv.IsDeleted = 0;
        
	SET SQL_SAFE_UPDATES = 1;
END