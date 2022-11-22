CREATE PROCEDURE `ReclaimEquipments`(
IN transactionIds VARCHAR(255),
	transactionType INT,
    acceptanceStaff VARCHAR(255),
    INOUT IsSuccess BOOL
)
BEGIN
DECLARE `_rollback` BOOL DEFAULT 0;
DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET `_rollback` = 1;

CALL SplitReturnTemp(transactionIds);    

START TRANSACTION;
IF (transactionType = 7) THEN -- Thu hồi thiết bị

    SET SQL_SAFE_UPDATES = 0;
	UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.ContractEquipmentId = ce.id AND te.isOld = 0 AND te.IsDeleted = 0
	INNER JOIN Transactions ts ON ts.Id = te.TransactionId AND ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp) 
	SET ce.StatusId = 4, -- StatusId = 4:Đang chờ thu hồi 
    ce.ReclaimedUnit = te.ReclaimedUnit,
    ce.UpdatedBy = ts.CreatedBy
    WHERE ce.IsDeleted = 0 AND ce.HasToReClaim = 1;
    
    UPDATE Transactions ts
    SET ts.AcceptanceStaff = acceptanceStaff, ts.StatusId = 2 -- Đã duyệt 2
    WHERE ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp);
    SET SQL_SAFE_UPDATES = 1;    
    
END IF;

IF `_rollback` THEN
		SET IsSuccess = NOT `_rollback`;
        ROLLBACK;
    ELSE
		SET IsSuccess = NOT `_rollback`;
        COMMIT;
    END IF;
END