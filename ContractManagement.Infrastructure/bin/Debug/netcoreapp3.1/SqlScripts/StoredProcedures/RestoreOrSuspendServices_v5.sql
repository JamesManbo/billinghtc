CREATE PROCEDURE `RestoreOrSuspendServices`(
    IN transactionIds VARCHAR(255),
    transactionType INT,
    acceptanceStaff VARCHAR(255),
    INOUT isSuccess BOOL
)
BEGIN
	-- Thực hiện khôi phục
	IF (transactionType = 11)
	THEN
	
	SET SQL_SAFE_UPDATES = 0;
	
	-- Cập nhật lại thông tin kênh(trạng thái, ngày khôi phục hoạt động, số ngày tạm ngưng)
	UPDATE OutContractServicePackages ocsp
	INNER JOIN TransactionServicePackages tsp ON tsp.OutContractServicePackageId = ocsp.id
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
	SET ocsp.StatusId = 0, -- Khôi phục 0 
			ocsp.TimeLine_SuspensionEndDate = ts.EffectiveDate,
			ocsp.TimeLine_DaysSuspended = DATEDIFF(ts.EffectiveDate, ocsp.TimeLine_SuspensionStartDate)
	WHERE ocsp.StatusId NOT IN (2, 3, 4, 5) 
	AND ocsp.IsDeleted = 0
	AND tsp.IsDeleted = 0
	AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds) > 0;
	
	UPDATE TransactionServicePackages tsp
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
	SET
		tsp.TimeLine_SuspensionEndDate = ts.EffectiveDate
	WHERE ts.`Type` = transactionType
	AND ts.IsDeleted = 0 
	AND FIND_IN_SET(ts.Id, transactionIds) > 0;
	
	-- Cập nhật lại trạng thái của thiết bị
	UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.ContractEquipmentId = ce.Id
	INNER JOIN TransactionChannelPoints tcp ON tcp.Id = te.OutputChannelPointId
	INNER JOIN TransactionServicePackages tsp ON (tsp.StartPointChannelId = tcp.Id OR tsp.EndPointChannelId = tcp.Id)
	INNER JOIN Transactions t ON t.Id = tsp.TransactionId
	SET
		ce.SupporterHoldedUnit = IFNULL(ce.SupporterHoldedUnit, 0) - te.SupporterHoldedUnit,
		ce.ActivatedUnit = ce.ActivatedUnit + te.SupporterHoldedUnit
	WHERE te.IsDeleted = 0 
		AND tcp.IsDeleted = 0 
		AND t.IsDeleted = 0 
		AND te.SupporterHoldedUnit > 0
		AND FIND_IN_SET(t.Id, transactionIds) > 0 AND t.`Type` = transactionType;
    
	-- Cập nhật lại lại thông tin phụ lục(người duyệt nghiệm thu phụ lục, trạng thái, ngày duyệt)
	UPDATE Transactions ts
	SET ts.AcceptanceStaff = acceptanceStaff,
			ts.StatusId = 4, -- Đã duyệt 2
			ts.EffectiveDate = ts.EffectiveDate
	WHERE ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds) > 0;
	SET SQL_SAFE_UPDATES = 1;

    -- Thực hiện tạm ngưng
	ELSEIF (transactionType = 3) 
	THEN

	SET SQL_SAFE_UPDATES = 0;
	UPDATE OutContractServicePackages ocsp
	INNER JOIN TransactionServicePackages tsp ON tsp.OutContractServicePackageId = ocsp.id AND tsp.IsDeleted = 0
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
	SET ocsp.StatusId = 1, -- Tạm ngưng 1
			ocsp.UpdatedDate = NOW(),
			ocsp.TimeLine_SuspensionStartDate = ts.EffectiveDate
  WHERE ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds) > 0;	
	
	-- Cập nhật số lượng tạm giữ của thiết bị
	UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.ContractEquipmentId = ce.Id
	INNER JOIN TransactionChannelPoints tcp ON tcp.Id = te.OutputChannelPointId
	INNER JOIN TransactionServicePackages tsp ON (tsp.StartPointChannelId = tcp.Id OR tsp.EndPointChannelId = tcp.Id)
	INNER JOIN Transactions t ON t.Id = tsp.TransactionId
	SET
		ce.SupporterHoldedUnit = IFNULL(ce.SupporterHoldedUnit, 0) + te.WillBeHoldUnit,
		ce.ActivatedUnit = ce.ActivatedUnit - te.WillBeHoldUnit
	WHERE te.IsDeleted = 0 
		AND tcp.IsDeleted = 0
		AND t.IsDeleted = 0 
		AND te.WillBeHoldUnit > 0
		AND FIND_IN_SET(t.Id, transactionIds) > 0 AND t.`Type` = transactionType;

     
	INSERT INTO ServicePackageSuspensionTimes (
        OutContractServicePackageId,
        SuspensionStartDate, 
        DaysSuspended, 
        DiscountAmount,
        RemainingAmount,
        CreatedDate,
        IsActive,
        IsDeleted,
        DisplayOrder    
	)
	SELECT
        ocsp.Id,
        ocsp.TimeLine_SuspensionStartDate,
        0,
        0,
        0,
        NOW(),
        1,
        0,
        0
	FROM OutContractServicePackages ocsp
	INNER JOIN TransactionServicePackages tsp ON tsp.OutContractServicePackageId = ocsp.id
	INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId	
	WHERE ocsp.IsDeleted = 0
	AND tsp.IsDeleted = 0
	AND ts.`Type` = transactionType
	AND ts.IsDeleted = 0
	AND FIND_IN_SET(ts.Id, transactionIds) > 0;
	
	
	UPDATE Transactions ts
	SET ts.AcceptanceStaff = acceptanceStaff,
			ts.StatusId = 4, -- Đã duyệt 2
			ts.EffectiveDate = ts.EffectiveDate
	WHERE ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds) > 0;    
	SET SQL_SAFE_UPDATES = 1;
    
END IF;

SET isSuccess = TRUE;

END