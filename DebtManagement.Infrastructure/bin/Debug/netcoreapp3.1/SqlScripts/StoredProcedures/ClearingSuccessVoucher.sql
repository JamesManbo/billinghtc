CREATE PROCEDURE `ClearingSuccessVoucher`(
IN voucherIds mediumtext,
    isReceipt bool,
	updatedBy varchar(255),
    clearingId varchar(255)
)
BEGIN
	CALL SplitReturnTemp(voucherIds);
    
	IF (isReceipt = true) THEN 
    
		SET SQL_SAFE_UPDATES = 0;
        
		UPDATE ReceiptVouchers rv
		SET rv.StatusId = 4,
        rv.Payment_Method = 2,
        rv.ClearingTotal = rv.GrandTotal,
        rv.UpdatedBy = updatedBy,
        rv.UpdatedDate = NOW(),
        rv.ClearingId = clearingId
        WHERE rv.IsDeleted = 0 AND rv.StatusId = 1 AND rv.Id IN (select distinct(val) from temp);
        
        SET SQL_SAFE_UPDATES = 1;
        
	ELSEIF (isReceipt = false) THEN
    
		SET SQL_SAFE_UPDATES = 0;
        
		UPDATE PaymentVouchers pv
		SET pv.StatusId = 4,
        pv.Payment_Method = 2,
        pv.ClearingTotal = pv.GrandTotal,
        pv.UpdatedBy = updatedBy,
        pv.UpdatedDate = NOW(),
        pv.ClearingId = clearingId
        WHERE pv.IsDeleted = 0 AND pv.StatusId = 1 AND pv.Id IN (select distinct(val) from temp);
        
        SET SQL_SAFE_UPDATES = 1;
    
    END IF;
END