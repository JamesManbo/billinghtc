CREATE PROCEDURE `ReceiptVouchersUpdateOverdueAndBadDebt`(
IN numberDaysBadDebtInReceipt INT)
BEGIN
    
	UPDATE ReceiptVouchers rv
    INNER JOIN VoucherTargets vt ON vt.Id = rv.TargetId AND vt.IsDeleted = FALSE 
    SET rv.StatusId = 9 -- Đã quá hạn thanh toán
    , rv.NumberDaysOverdue = CASE WHEN vt.IsEnterprise = FALSE THEN DATEDIFF(CURDATE(), DATE(ADDDATE(rv.IssuedDate, rv.NumberBillingLimitDays)))
    ELSE DATEDIFF(CURDATE(), DATE(ADDDATE(rv.InvoiceDate, CASE WHEN rv.NumberBillingLimitDays > 30 THEN rv.NumberBillingLimitDays ELSE 30 END))) END
    WHERE rv.StatusId IN (1,2) 
    AND 
    (vt.IsEnterprise = FALSE AND DATE(ADDDATE(rv.IssuedDate, rv.NumberBillingLimitDays + 1)) <= CURDATE())
    OR 
    (vt.IsEnterprise = TRUE AND rv.InvoiceDate IS NOT NULL AND DATE(ADDDATE(rv.InvoiceDate, (CASE WHEN rv.NumberBillingLimitDays > 30 THEN rv.NumberBillingLimitDays ELSE 30 END) + 1)) <= CURDATE());
    
	UPDATE ReceiptVouchers rv
    INNER JOIN VoucherTargets vt ON vt.Id = rv.TargetId AND vt.IsDeleted = FALSE AND vt.IsEnterprise = FALSE 
    SET rv.StatusId = 8 -- Nợ xấu
    , rv.NumberDaysOverdue = 0, IsBadDebt = TRUE
    WHERE DATE(ADDDATE(rv.IssuedDate, numberDaysBadDebtInReceipt)) <= CURDATE() AND rv.StatusId IN (1,2,9);
    
END