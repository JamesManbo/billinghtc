CREATE PROCEDURE `GetNeedToPaymentContractServices`()
BEGIN
	IF NOT EXISTS (SELECT 1 FROM TemporaryPayingContracts)
	THEN
	BEGIN
			INSERT INTO TemporaryPayingContracts(`OutContractId`, `ServicePackageId`)
			SELECT oc.Id, t2.Id
			FROM OutContracts oc
			INNER JOIN OutContractServicePackages t2 ON t2.OutContractId = oc.Id AND t2.IsDeleted = FALSE
			WHERE  oc.IsDeleted = FALSE
				AND oc.ContractStatusId NOT IN (4, 5, 9)
				AND (t2.StatusId IN (0, 5) OR (t2.StatusId = 1 AND t2.TimeLine_PaymentForm = 1))
				AND t2.TimeLine_NextBilling IS NOT NULL
				AND TIMESTAMPDIFF(DAY, t2.TimeLine_NextBilling, UTC_TIMESTAMP()) >= 0
				AND t2.FlexiblePricingTypeId = 1;
		END;
	END IF;
	
	SELECT `oc`.`Id`,
	`oc`.`CurrencyUnitId`,
	`oc`.`CurrencyUnitCode`,
	`oc`.`IdentityGuid`,
	`oc`.`ContractCode`,
	`oc`.`AgentCode`,
	`oc`.`MarketAreaId`,
	`oc`.`MarketAreaName`,
	`oc`.`ContractTypeId`,
	`oc`.`ContractStatusId`,
	`oc`.`ContractorId`,
	`oc`.`SignedUserId`,
	`oc`.`SalesmanId`,
	`oc`.`Description`,    
	`oc`.`TotalTaxPercent`,
	`oc`.`FiberNodeInfo`,
	`oc`.`ContractNote`,
	`oc`.`AgentContractCode`,
	`oc`.`OrganizationUnitId`,
	`oc`.`CashierUserId`,
	`oc`.`CashierUserName`,
	`oc`.`CashierFullName`,
	`oc`.`NumberBillingLimitDays`,
	`oc`.`Payment_Form` AS `Form`,
	`oc`.`Payment_Method` AS `Method`,
	`oc`.`Payment_Address` AS `Address`,    
	`oc`.`TimeLine_Signed` AS `Signed`,
	`oc`.`TimeLine_Effective` AS `Effective`,
	`oc`.`TimeLine_Liquidation` AS `Liquidation`,
	`oc`.`TimeLine_Expiration` AS `Expiration`,
	`oc`.`TimeLine_PaymentPeriod` AS `PaymentPeriod`,
	`oc`.`TimeLine_RenewPeriod` AS `RenewPeriod`,

	`t2`.`Id`,
	`t2`.`IsActive`,
	`t2`.`ProjectId`,
	`t2`.`ServicePackageId`,
	`t2`.`ServiceId`,
	`t2`.`ServiceName`,
	`t2`.`PackageName`,
	`t2`.`IsFreeStaticIp`,
	`t2`.`InternationalBandwidth`,
	`t2`.`DomesticBandwidth`,
	`t2`.`CustomerCode`,
	`t2`.`TaxAmount`,
	`t2`.`TaxPercent`,
	`t2`.`PackagePrice`,
	`t2`.`SubTotalBeforeTax`,
	`t2`.`SubTotal`,
	`t2`.`GrandTotal`,
	`t2`.`GrandTotalBeforeTax`,
	`t2`.`EquipmentAmount`,
	`t2`.`InstallationFee`,
	`t2`.`CId`,
	`t2`.`StartPointChannelId`,
	`t2`.`EndPointChannelId`,
	`t2`.`DomesticBandwidthUom`,
	`t2`.`InternationalBandwidthUom`,
	`t2`.`OtherFee`,
	`t2`.`CurrencyUnitId`,
	`t2`.`CurrencyUnitCode`,
	`t2`.`TimeLine_PaymentPeriod` AS `PaymentPeriod`,
	`t2`.`TimeLine_Effective` AS `Effective`,
	`t2`.`TimeLine_Signed` AS `Signed`,
	`t2`.`TimeLine_LatestBilling` AS `LatestBilling`,
	`t2`.`TimeLine_NextBilling` AS `NextBilling`,
	`t2`.`TimeLine_PrepayPeriod` AS `PrepayPeriod`,
	`t2`.`TimeLine_StartBilling` AS `StartBilling`,
	`t2`.`TimeLine_SuspensionStartDate` AS `SuspensionStartDate`,
	`t2`.`TimeLine_SuspensionEndDate` AS `SuspensionEndDate`,
	`t2`.`TimeLine_TerminateDate` AS `TerminateDate`,
	
	`t3`.`Id` AS Id,
	`t3`.`IdentityGuid` AS IdentityGuid,
	`t3`.`ContractorFullName` AS ContractorFullName,
	`t3`.`ContractorCode` AS ContractorCode,
	`t3`.`ContractorPhone` AS ContractorPhone,
	`t3`.`ContractorEmail` AS ContractorEmail,
	`t3`.`ContractorFax` AS ContractorFax,
	`t3`.`ContractorAddress` AS ContractorAddress,
	`t3`.`ContractorIdNo` AS ContractorIdNo,
	`t3`.`ContractorTaxIdNo` AS ContractorTaxIdNo,
	`t3`.`IsEnterprise` AS IsEnterprise,
	`t3`.`IsBuyer` AS IsBuyer,
	`t3`.`IsPartner` AS IsPartner,
	`t3`.`UserIdentityGuid` AS UserIdentityGuid,
	`t3`.`ApplicationUserIdentityGuid` AS ApplicationUserIdentityGuid,
	`t3`.`AccountingCustomerCode` AS AccountingCustomerCode,
	`t3`.`Representative` AS Representative,
	`t3`.`Position` AS Position,
	`t3`.`AuthorizationLetter` AS AuthorizationLetter,
	`t3`.`ContractorShortName` AS ContractorShortName,
	`t3`.`ContractorCity` AS ContractorCity,
	`t3`.`ContractorCityId` AS ContractorCityId,
	`t3`.`ContractorDistrict` AS ContractorDistrict,
	`t3`.`ContractorDistrictId` AS ContractorDistrictId,
	
	IFNULL(`t6`.`OutContractServicePackageId`, 0) AS `OutContractServicePackageId`,
	`t6`.`TaxCategoryId` AS `TaxCategoryId`,
	`t6`.`TaxCategoryName` AS `TaxCategoryName`,
	`t6`.`TaxCategoryCode` AS `TaxCategoryCode`,
	`t6`.`TaxValue` AS `TaxValue`,
	cep.Id AS `Id`,
	cep.CurrencyUnitId AS `CurrencyUnitId`,
	cep.CurrencyUnitCode AS `CurrencyUnitCode`,
	cep.PointType AS `PointType`,
	cep.InstallationFee AS `InstallationFee`,
	cep.OtherFee AS `OtherFee`,
	cep.MonthlyCost AS `MonthlyCost`,
	cep.EquipmentAmount AS `EquipmentAmount`,
	'' AS `EndPointInstallationSpliter`,
	cep.`InstallationAddress_Building` AS `Building`,
	cep.`InstallationAddress_Floor` AS `Floor`,
	cep.`InstallationAddress_RoomNumber` AS `RoomNumber`,
	cep.`InstallationAddress_Street` AS `Street`,
	cep.`InstallationAddress_District` AS `District`,
	cep.`InstallationAddress_DistrictId` AS `DistrictId`,
	cep.`InstallationAddress_City` AS `City`,
	cep.`InstallationAddress_CityId` AS `CityId`,

	IFNULL(csp.Id, 0) AS `Id`,
	csp.CurrencyUnitId AS `CurrencyUnitId`,
	csp.CurrencyUnitCode AS `CurrencyUnitCode`,
	csp.PointType AS `PointType`,
	csp.InstallationFee AS `InstallationFee`,
	csp.OtherFee AS `OtherFee`,
	csp.MonthlyCost AS `MonthlyCost`,
	csp.EquipmentAmount AS `EquipmentAmount`,
	'' AS `StartPointInstallationSpliter`,
	csp.`InstallationAddress_Building` AS `Building`,
	csp.`InstallationAddress_Floor` AS `Floor`,
	csp.`InstallationAddress_RoomNumber` AS `RoomNumber`,
	csp.`InstallationAddress_Street` AS `Street`,
	csp.`InstallationAddress_District` AS `District`,
	csp.`InstallationAddress_DistrictId` AS `DistrictId`,
	csp.`InstallationAddress_City` AS `City`,
	csp.`InstallationAddress_CityId` AS `CityId`
	
	FROM TemporaryPayingContracts voc
	INNER JOIN OutContracts oc ON voc.OutContractId = oc.Id
	INNER JOIN OutContractServicePackages t2 ON t2.Id = voc.ServicePackageId
	INNER JOIN Contractors t3 ON t3.Id = t2.PaymentTargetId
	INNER JOIN OutputChannelPoints cep ON cep.Id = t2.EndPointChannelId
	LEFT JOIN OutputChannelPoints csp ON csp.Id = t2.StartPointChannelId
	LEFT JOIN OutContractServicePackageTaxes t6 ON t2.Id = t6.OutContractServicePackageId;
END