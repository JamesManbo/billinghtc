CREATE PROCEDURE `ChangeEquipments`(
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
IF (transactionType = 6) THEN -- Thay đổi thiết bị

    SET SQL_SAFE_UPDATES = 0;
    
    INSERT INTO ContractEquipments (
    IsInSurveyPlan,
    ReclaimedUnit,
    ServiceId,
    ServicePackageId,
    IsFree,
    ExaminedUnit, 
    StatusId, 
    EquipmentId,
    EquipmentName,
    HasToReclaim,
    DeviceCode,
    Manufacturer,
    SerialCode,
    Specifications,
    RealUnit,
    UnitPrice,
    ExaminedSubTotal,
    ExaminedGrandTotal,
    SubTotal,
    GrandTotal,    
    OutContractPackageId,
    CreatedBy,
    CreatedDate,
    IsActive,
    IsDeleted,
    DisplayOrder,
    TransactionEquipmentId
	)
	SELECT
    0,
    0,
    ce.ServiceId,
    ce.ServicePackageId,
    te.IsFree,
    te.ExaminedUnit,
    te.StatusId, -- StatusId = 2: Đã được duyệt, chờ triển khai
    te.EquipmentId,
    te.EquipmentName,
    te.HasToReclaim,
    te.DeviceCode,
    te.Manufacturer,
    te.SerialCode,
    te.Specifications,
    te.RealUnit,     
	te.UnitPrice,
    te.ExaminedUnit * te.UnitPrice,
    te.ExaminedUnit * te.UnitPrice,
    te.RealUnit * te.UnitPrice,
    te.RealUnit * te.UnitPrice,
    ce.OutContractPackageId,
	acceptanceStaff,
    NOW(),
    1,
    0,
    0,
    te.Id
	FROM ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.ContractEquipmentId = ce.id AND te.isOld = 0 AND te.IsDeleted = 0
	INNER JOIN Transactions ts ON ts.Id = te.TransactionId AND ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp)
    WHERE ce.IsDeleted = 0;
    
    UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.ContractEquipmentId = ce.id AND te.isOld = 1 AND te.IsDeleted = 0
	INNER JOIN Transactions ts ON ts.Id = te.TransactionId AND ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp) 
	SET ce.StatusId = 4, -- Thiết bị Đang chờ thu hồi
    ce.UpdatedBy = ts.CreatedBy
    WHERE ce.IsDeleted = 0;
    
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