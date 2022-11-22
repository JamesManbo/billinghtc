CREATE PROCEDURE `MapContractSharingRevenueLineToHead`()
BEGIN
    SET SQL_SAFE_UPDATES = 0;
    UPDATE ContractSharingRevenueLines t1
    INNER JOIN OutContractServicePackages t2 ON t1.InServiceChannelUid = t2.Uid
	SET t1.InServiceChannelId = t2.Id
	WHERE t1.InServiceChannelId IS NULL;
    SET SQL_SAFE_UPDATES = 1;
END