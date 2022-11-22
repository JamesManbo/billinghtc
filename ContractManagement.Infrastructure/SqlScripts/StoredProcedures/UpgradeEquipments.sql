CREATE PROCEDURE `UpgradeEquipments`(
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
IF (transactionType = 9) THEN -- Nâng cấp thiết bị thiết bị

	drop temporary table if exists sptstemp;
	create temporary table sptstemp( OutContractPackageId INT, TransactionId INT, CreatedBy LONGTEXT);
	insert into sptstemp(OutContractPackageId, TransactionId, CreatedBy)
    SELECT ocsp.Id, ts.Id, ts.CreatedBy
    FROM OutcContractServicePackages ocsp
	INNER JOIN OutContractTransactions octs ON octs.OutContractId = ocsp.OutContractId AND octs.IsDeleted = 0
	INNER JOIN Transactions ts ON ts.Id = octs.TransactionId AND ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp)
    WHERE ocsp.IsDeleted = 0;

    SET SQL_SAFE_UPDATES = 0;
    
    UPDATE ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.OldEquipmentId = ce.EquipmentId AND te.isOld = 0 AND te.IsDeleted = 0
	INNER JOIN sptstemp temp ON temp.TransactionId = te.TransactionId AND temp.OutContractPackageId = ce.OutContractPackageId
	SET ce.StatusId = 4, -- Thiết bị Đang chờ thu hồi
    ce.UpdatedBy = temp.CreatedBy
    WHERE ce.IsDeleted = 0;
    
    INSERT INTO ContractEquipments (
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
    CreatedBy
	)
	SELECT
    te.IsFree,
    te.ExaminedUnit,
    te.StatusId,
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
    temp.OutContractPackageId,
	temp.CreatedBy
    FROM ContractEquipments ce
	INNER JOIN TransactionEquipments te ON te.OldEquipmentId = ce.EquipmentId AND te.isOld = 0 AND te.IsDeleted = 0
	INNER JOIN sptstemp temp ON temp.TransactionId = te.TransactionId AND temp.OutContractPackageId = ce.OutContractPackageId	
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