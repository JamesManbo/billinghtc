CREATE PROCEDURE `MapContractSharingRevenueLineToHead`()
BEGIN
    SET SQL_SAFE_UPDATES = 0;

    UPDATE ContractSharingRevenueLines t1
    INNER JOIN OutContractServicePackages t2 ON t1.InServiceChannelUid = t2.Uid
		SET t1.InServiceChannelId = t2.Id
		WHERE t1.InServiceChannelId IS NULL 
    AND t2.Uid IS NOT NULL 
    AND t2.Uid <> '';

    UPDATE ContractSharingRevenueLines t1
    INNER JOIN OutContractServicePackages t2 ON t1.OutServiceChannelUid = t2.Uid
		SET t1.OutServiceChannelId = t2.Id
		WHERE t1.OutServiceChannelId IS NULL 
    AND t2.Uid IS NOT NULL 
    AND t2.Uid <> '';

    UPDATE ContractSharingRevenueLines t1
    INNER JOIN OutContractServicePackages t2 ON t1.OutServiceChannelId = t2.Id
		SET t1.SharingFrom = t2.TimeLine_Effective, 
				t1.SharingTo = IF(t1.SharingDuration > 0, DATE_ADD(t2.TimeLine_Effective, INTERVAL t1.SharingDuration MONTH), NULL)
		WHERE t1.SharingFrom IS NULL
		AND t2.TimeLine_Effective IS NOT NULL;

    UPDATE SharingRevenueLineDetails t1
    INNER JOIN ContractSharingRevenueLines t2 ON t1.SharingLineUid = t2.Uid
    SET t1.SharingLineId = t2.Id
    WHERE t1.SharingLineId IS NULL
    AND t2.Uid IS NOT NULL 
    AND t2.Uid <> '';

    SET SQL_SAFE_UPDATES = 1;
END