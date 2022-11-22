CREATE PROCEDURE `sp_GetDailyRevenueByServiceGroup`()
BEGIN
	SELECT  IFNULL(svg.GroupName,'') AS GroupName,
		SUM(IFNULL(rvd.GrandTotal,0)) AS GrandTotal
	FROM ReceiptVoucherDetails AS rvd
	INNER JOIN ReceiptVouchers AS rv ON rvd.ReceiptVoucherId = rv.Id
	INNER JOIN ITC_FBM_Contracts.Services AS sv ON sv.Id = rvd.ServiceId
	INNER JOIN ITC_FBM_Contracts.ServiceGroups AS svg ON sv.GroupId = svg.Id

	WHERE 
		rvd.CreatedDate > CURDATE()
		AND  rv.StatusId = 4
	GROUP BY svg.GroupName;
END