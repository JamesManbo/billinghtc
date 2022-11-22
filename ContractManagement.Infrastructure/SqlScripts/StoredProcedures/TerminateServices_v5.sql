CREATE PROCEDURE `TerminateServices`(
IN transactionIds VARCHAR(255),
	transactionType INT,
	acceptanceStaff VARCHAR(255),
	INOUT isSuccess BOOL
)
BEGIN

IF (transactionType = 4) THEN -- Hủy dịch vụ

	SET SQL_SAFE_UPDATES = 0;
	
	UPDATE OutContractServicePackages ocsp
	INNER JOIN TransactionServicePackages tsp ON tsp.OutContractServicePackageId = ocsp.Id
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
	SET 
			ocsp.StatusId = 2, -- Hủy dịch vụ 2 
			ocsp.UpdatedBy = ts.CreatedBy,
			ocsp.UpdatedDate = NOW(),
			ocsp.TimeLine_TerminateDate = IFNULL(tsp.TimeLine_TerminateDate, NOW())
	WHERE tsp.IsDeleted = 0 
	AND ts.IsDeleted = 0
	AND ts.`Type` = transactionType 
	AND FIND_IN_SET(ts.Id, transactionIds);
	
	-- Cập nhật trạng thái thiết bị của hợp đồng thành "Đã thu hồi"
	UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON ce.Id = te.ContractEquipmentId
	INNER JOIN TransactionChannelPoints tcp ON tcp.Id = te.OutputChannelPointId
	INNER JOIN TransactionServicePackages tsp ON (tsp.StartPointChannelId = tcp.Id OR tsp.EndPointChannelId = tcp.Id)
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
	SET 
		ce.ReclaimedUnit = ce.ReclaimedUnit + te.WillBeReclaimUnit,
		ce.ActivatedUnit = ce.ActivatedUnit - te.WillBeReclaimUnit
	WHERE te.IsDeleted = 0 AND tcp.IsDeleted = 0 AND ts.IsDeleted = 0
		AND FIND_IN_SET(ts.Id, transactionIds) > 0
		AND te.WillBeReclaimUnit > 0
		AND ts.`Type` = transactionType;
    
	UPDATE Transactions ts
	SET ts.AcceptanceStaff = acceptanceStaff,
		ts.StatusId = 4, 
		ts.EffectiveDate = (
			CASE WHEN ts.IsTechnicalConfirmation = 1 THEN ts.EffectiveDate
			ELSE NOW()
			END
		),
		ts.UpdatedDate = NOW()
	WHERE ts.IsDeleted = 0 -- Chờ duyệt 1
	AND ts.`Type` = transactionType 
	AND FIND_IN_SET(ts.Id, transactionIds);
	
	SET SQL_SAFE_UPDATES = 1;    
	
END IF;

SET isSuccess = TRUE;

END