CREATE PROCEDURE `DeleteContractSharingRevenuesByIds`(
IN ids varchar(255),
updateBy varchar(255))
BEGIN
	CALL SplitReturnTemp(ids);
    SET SQL_SAFE_UPDATES = 0;
	UPDATE ContractSharingRevenues SET IsDeleted = 1, UpdatedBy = updateBy, UpdatedDate = NOW() WHERE Id IN (select distinct(val) from temp);
	UPDATE ContractSharingRevenueLines SET IsDeleted = 1, UpdatedBy = updateBy, UpdatedDate = NOW() WHERE CsrId IN (select distinct(val) from temp);
    SET SQL_SAFE_UPDATES = 1;
END