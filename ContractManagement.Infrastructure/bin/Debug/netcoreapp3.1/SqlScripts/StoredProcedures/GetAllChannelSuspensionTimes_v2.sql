CREATE PROCEDURE `GetAllChannelSuspensionTimes`(
	IN cId NVARCHAR(256)
)
BEGIN

	DROP TEMPORARY TABLE IF EXISTS CIDS;
	CREATE TEMPORARY TABLE CIDS( TXT TEXT );
	INSERT INTO CIDS VALUES(cId);

	DROP TEMPORARY TABLE IF EXISTS CIDS_TEMP;
	CREATE TEMPORARY TABLE CIDS_TEMP( VAL NVARCHAR(256) );
	SET @insertToTempSql = CONCAT("INSERT INTO CIDS_TEMP (VAL) VALUES ('", REPLACE(( SELECT GROUP_CONCAT(DISTINCT TXT) AS DATA FROM CIDS), ",", "'),('"),"');");
	PREPARE STMT1 FROM @insertToTempSql;
	EXECUTE STMT1;

	DROP TEMPORARY TABLE IF EXISTS CIDS;

	SELECT 
		t1.Id
		, t1.IsActive
		, t1.DisplayOrder
		, t1.OrganizationPath
		, t1.Description
		, t1.OutContractServicePackageId
		, t1.SuspensionStartDate
		, t1.SuspensionEndDate
		, t1.DaysSuspended
		, t1.DiscountAmount
		, t1.RemainingAmount
		, t1.DiscountAmountBeforeTax
		, t1.RemainingAmountBeforeTax
		, t1.TaxAmount
	FROM ServicePackageSuspensionTimes AS t1
	INNER JOIN OutContractServicePackages t2 ON t1.OutContractServicePackageId = t2.Id
	WHERE t1.IsDeleted = FALSE
		AND t1.IsActive = TRUE
		AND t2.IsDeleted = FALSE
		AND (IFNULL(cId, '') LIKE '' OR EXISTS (SELECT 1 FROM CIDS_TEMP temp WHERE temp.VAL = t2.CId));
END