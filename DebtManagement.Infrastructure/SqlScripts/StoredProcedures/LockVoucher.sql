CREATE PROCEDURE `LockVoucher`(
IN voucherIds mediumtext,
    isReceipt bool,
	updatedBy varchar(255)
)
BEGIN
	CALL SplitReturnTemp(voucherIds);
    
	IF (isReceipt = true) THEN 
    
		SET SQL_SAFE_UPDATES = 0;
        
		UPDATE ReceiptVouchers rv
		SET rv.IsLock = 1,
        rv.UpdatedBy = updatedBy,
        rv.UpdatedDate = NOW()
        WHERE rv.IsDeleted = 0 AND rv.IsLock = 0 AND rv.Id IN (select distinct(val) from temp);
        
        SET SQL_SAFE_UPDATES = 1;
        
	ELSEIF (isReceipt = false) THEN
    
		SET SQL_SAFE_UPDATES = 0;
        
		UPDATE PaymentVouchers pv
		SET pv.IsLock = 1,
        pv.UpdatedBy = updatedBy,
        pv.UpdatedDate = NOW()
        WHERE pv.IsDeleted = 0 AND pv.IsLock = 0 AND pv.Id IN (select distinct(val) from temp);
        
        SET SQL_SAFE_UPDATES = 1;
    
    END IF;
END