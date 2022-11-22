CREATE PROCEDURE `AddServiceChannelToInContract`(
	IN serviceChannelIds VARCHAR(255),
    inContractId INT,
	updateBy varchar(255)
)
BEGIN
	SET SQL_SAFE_UPDATES = 0;
    UPDATE InputChannels sc
	SET sc.InContractId = NULL, sc.UpdatedBy = updateBy
	WHERE sc.InContractId = inContractId;
    
	CALL SplitReturnTemp (serviceChannelIds);
    UPDATE InputChannels sc
	SET sc.InContractId = inContractId, sc.UpdatedBy = updateBy
	WHERE sc.Id IN (select distinct(val) from temp);
    SET SQL_SAFE_UPDATES = 1;
END