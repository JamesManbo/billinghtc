CREATE PROCEDURE `GetReceiptVoucherNumberByIssuedDate`
(IN startingIssuedDate date)
BEGIN

	SELECT DATE(IssuedDate) AS `IssuedDate`,
		COUNT(Id) AS `Count`
	FROM ReceiptVouchers
	WHERE startingIssuedDate IS NULL OR DATE(IssuedDate) >= DATE(startingIssuedDate)
	GROUP BY DATE(IssuedDate);	
END