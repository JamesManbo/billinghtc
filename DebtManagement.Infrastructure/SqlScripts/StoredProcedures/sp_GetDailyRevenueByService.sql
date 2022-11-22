CREATE  PROCEDURE `sp_GetDailyRevenueByService`()
BEGIN
		SELECT rvd.ServiceName,
		SUM(rvd.GrandTotal) AS GrandTotal
		FROM ReceiptVoucherDetails AS rvd
		INNER JOIN ReceiptVouchers AS rv ON rvd.ReceiptVoucherId = rv.Id

		WHERE rvd.CreatedDate > CURDATE()
			AND  rv.StatusId = 4
		GROUP BY rvd.ServiceName;
END