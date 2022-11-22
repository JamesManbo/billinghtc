CREATE PROCEDURE `TerminateContracts`(
IN transactionIds VARCHAR(255),
	transactionType INT,
    acceptanceStaff VARCHAR(255),
    effectiveDate DateTime,
	isOutContract BOOL,
    INOUT isSuccess BOOL
)
BEGIN
DECLARE `_rollback` BOOL DEFAULT 0;
DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET `_rollback` = 1;

START TRANSACTION;

IF (transactionType = 8) THEN -- Hủy hợp đồng

    SET SQL_SAFE_UPDATES = 0;
		IF (isOutContract = TRUE)
		THEN
			UPDATE OutContracts oc
			INNER JOIN Transactions ts ON ts.OutContractId = oc.Id
				AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds)
			SET 
			oc.ContractStatusId = 5,
			oc.UpdatedBy = acceptanceStaff,
			oc.TimeLine_Liquidation = NOW();

			UPDATE OutContractServicePackages ocsp
			INNER JOIN OutContracts oc ON oc.Id = ocsp.OutContractId
			INNER JOIN Transactions ts ON ts.OutContractId = oc.Id
			SET	
				ocsp.StatusId = 2,
				ocsp.TimeLine_TerminateDate = NOW()
			WHERE ocsp.IsDeleted = 0
			AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds);
		ELSE
			UPDATE InContracts ic
			INNER JOIN Transactions ts ON ts.InContractId = ic.Id
				AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds)
			SET 
			ic.ContractStatusId = 5,
			ic.UpdatedBy = acceptanceStaff,
			ic.TimeLine_Liquidation = NOW();

			UPDATE OutContractServicePackages ocsp
			INNER JOIN InContracts ic ON ic.Id = ocsp.InContractId
			INNER JOIN Transactions ts ON ts.InContractId = ic.Id
			SET	
				ocsp.StatusId = 2,
				ocsp.TimeLine_TerminateDate = NOW()
			WHERE ocsp.IsDeleted = 0
			AND ts.`Type` = transactionType AND ts.IsDeleted = 0 AND FIND_IN_SET(ts.Id, transactionIds);	
		END IF;
		
		-- Cập nhật trạng thái thiết bị cần thu hồi
		UPDATE ContractEquipments ce
		INNER JOIN TransactionEquipments te ON ce.Id = te.ContractEquipmentId
		INNER JOIN TransactionChannelPoints tcp ON tcp.Id = te.OutputChannelPointId
		INNER JOIN TransactionServicePackages tsp ON (tsp.StartPointChannelId = tcp.Id OR tsp.EndPointChannelId = tcp.Id)
		INNER JOIN Transactions ts ON ts.Id = tsp.TransactionId
		SET 		
			ce.ReclaimedUnit = ce.ReclaimedUnit + te.WillBeReclaimUnit,
			ce.ActivatedUnit = ce.ActivatedUnit - te.WillBeReclaimUnit,
			te.ReclaimedUnit = te.WillBeReclaimUnit
		WHERE te.IsDeleted = 0 AND tcp.IsDeleted = 0 AND ts.IsDeleted = 0
		AND FIND_IN_SET(ts.Id, transactionIds) > 0 AND ts.`Type` = transactionType;
    
    UPDATE Transactions ts
    SET 
        ts.AcceptanceStaff = acceptanceStaff, 
        ts.StatusId = 4,
        ts.EffectiveDate = effectiveDate
    WHERE ts.`Type` = transactionType
    AND ts.IsDeleted = 0
    AND FIND_IN_SET(ts.Id, transactionIds);

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