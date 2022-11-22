CREATE DEFINER=`sa`@`%` PROCEDURE `sp_GetCollectedAndUnCollectedVoucherByMonth`( IN `month` INT, IN `year` INT, IN `cashierUserId` text )
BEGIN
	SELECT month AS `Month`,
		SUM(CASE
			WHEN t1.Status in (1,5,8,9)
			THEN 1
			ELSE 0
			END
		) AS UnCollectedVouchers,
		SUM(CASE
			WHEN t1.Status not in (1,5,8,9)
			THEN 1
			ELSE 0
			END
		) AS CollectedVouchers
	FROM
		ReceiptVoucherDebtHistories t1
	INNER JOIN ReceiptVouchers rv ON t1.ReceiptVoucherId = rv.Id
	WHERE
		t1.CashierUserId = cashierUserId
	AND rv.IsActive = TRUE
	AND rv.StatusId NOT IN(5)
	AND rv.IsDeleted = FALSE
	AND rv.InvalidIssuedDate = FALSE
	AND  MONTH(t1.PaymentDate) = month AND YEAR(t1.PaymentDate) = year;
		
-- 		UNION ALL
-- 		
-- 		SELECT
-- 			'Total' AS `month` ,
-- 			SUM( IF ( StatusId IN ( 4, 5 ), 1, 0 )) AS CollectedVouchers,
-- 			SUM( IF ( StatusId NOT IN ( 4, 5 ), 1, 0 )) AS UnCollectedVouchers			
-- 		FROM
-- 			ReceiptVouchers 
-- 		WHERE
-- 			CashierUserId = cashierUserId
-- 			AND IsActive = TRUE
-- 		AND  MONTH(PaymentDate) = month AND YEAR(PaymentDate)  = year;

END