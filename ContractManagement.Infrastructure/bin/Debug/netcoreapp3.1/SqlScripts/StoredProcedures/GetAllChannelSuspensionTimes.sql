
CREATE PROCEDURE `GetAllChannelSuspensionTimes`()
BEGIN
	SELECT 
		Id
		, IsActive
		, DisplayOrder
		, OrganizationPath
		, Description
		, OutContractServicePackageId
		, SuspensionStartDate
		, SuspensionEndDate
		, DaysSuspended
		, DiscountAmount
		, RemainingAmount
		, DiscountAmountBeforeTax
		, RemainingAmountBeforeTax
		, TaxAmount
	FROM ServicePackageSuspensionTimes 
	WHERE IsDeleted = FALSE
		AND IsActive = TRUE;
END