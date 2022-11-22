CREATE PROCEDURE `CancelledClearingIdInVoucher`(
IN 	clearingId varchar(68),
	updatedBy varchar(255))
BEGIN
		SET SQL_SAFE_UPDATES = 0;
        
		UPDATE receiptvouchers rv
		INNER JOIN clearingreceiptdetails crd ON rv.Id = crd.ReceiptVoucherId AND crd.ClearingId = clearingId AND crd.IsDeleted = 0
		SET rv.IsLock = 0,
        rv.UpdatedBy = updatedBy,
        rv.UpdatedDate = NOW()
        WHERE rv.IsDeleted = 0;
        
        
		UPDATE paymentvouchers pv
		INNER JOIN clearingpaymentdetails cpd ON pv.Id = cpd.PaymentVoucherId AND cpd.ClearingId = clearingId AND cpd.IsDeleted = 0
		SET pv.IsLock = 0,
        pv.UpdatedBy = updatedBy,
        pv.UpdatedDate = NOW()
        WHERE pv.IsDeleted = 0;
        
		SET SQL_SAFE_UPDATES = 1;
END