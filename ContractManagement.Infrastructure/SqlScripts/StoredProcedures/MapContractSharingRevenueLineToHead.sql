CREATE PROCEDURE `MapContractSharingRevenueLineToHead`(
IN heatIds VARCHAR(255),
	updateBy varchar(255)
)
BEGIN
	CALL SplitReturnTemp(heatIds);
    SET SQL_SAFE_UPDATES = 0;
    UPDATE ContractSharingRevenueLines t1
    INNER JOIN ContractSharingRevenues t2 ON t1.CsrUid = t2.Uid
	SET t1.CsrId = t2.Id, t1.UpdatedBy = updateBy    
	WHERE t2.Id IN (select distinct(val) from temp);
    SET SQL_SAFE_UPDATES = 1;
END