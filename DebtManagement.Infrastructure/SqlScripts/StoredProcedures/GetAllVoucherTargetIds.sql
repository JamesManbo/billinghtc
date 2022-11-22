CREATE PROCEDURE `GetAllVoucherTargetIds` ()
BEGIN
	SELECT IdentityGuid, Id
	FROM VoucherTargets
    WHERE IsDeleted = FALSE;
END
