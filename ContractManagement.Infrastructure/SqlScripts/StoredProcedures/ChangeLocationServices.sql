CREATE PROCEDURE `ChangeLocationServices`(
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
IF (transactionType = 5) THEN -- Dịch chuyển địa điểm

    SET SQL_SAFE_UPDATES = 0;
	UPDATE outcontractservicepackages ocsp
	INNER JOIN transactionservicepackages tsp ON tsp.OutContractServicePackageId = ocsp.id AND tsp.isOld = 0 AND tsp.IsDeleted = 0
	INNER JOIN transactions ts ON ts.Id = tsp.TransactionId AND ts.StatusId = 1 -- Chờ duyệt 1
    AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND ts.Id IN (select distinct(val) from temp) 
	SET ocsp.InstallationAddress_Street = tsp.InstallationAddress_Street, -- Chuyển địa chỉ lặp đặt
	ocsp.InstallationAddress_StartPoint = tsp.InstallationAddress_StartPoint, -- Chuyển địa chỉ lặp đặt điểm đầu
	ocsp.InstallationAddress_EndPoint = tsp.InstallationAddress_EndPoint, -- Chuyển địa chỉ lặp đặt điểm cuối
    ocsp.UpdatedBy = acceptanceStaff
    WHERE ocsp.StatusId <> 2 AND ocsp.IsDeleted = 0; -- Hủy dịch vụ 2
    
    UPDATE transactions ts
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