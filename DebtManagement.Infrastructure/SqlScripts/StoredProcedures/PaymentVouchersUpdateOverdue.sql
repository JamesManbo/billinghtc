CREATE PROCEDURE `PaymentVouchersUpdateOverdue` ()
BEGIN

	UPDATE PaymentVouchers pv
    SET pv.StatusId = 5 -- Đã quá hạn thanh toán
    , pv.NumberDaysOverdue = DATEDIFF(CURDATE(), DATE(ADDDATE(pv.InvoiceDate, pv.NumberBillingLimitDays)))
    WHERE pv.StatusId IN (1,2) 
    AND DATE(ADDDATE(pv.InvoiceDate, pv.NumberBillingLimitDays + 1)) <= CURDATE();
    
END
