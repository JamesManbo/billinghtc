
CREATE PROCEDURE `GetOutContractSimpleAllByInContractId`(
    IN inContractId INT,
	currencyUnitId INT
)
BEGIN
SELECT DISTINCT oc.Id,
 oc.ContractCode,
 oc.CurrencyUnitId,
 oc.CurrencyUnitCode,
 oc.ContractStatusId,
 
 cus.Id,
 cus.ContractorFullName,
 cus.ContractorPhone,
 
 ocsp.Id,
 ocsp.CId,
 ocsp.CurrencyUnitId,
 ocsp.CurrencyUnitCode,
 ocsp.ServiceId,
 ocsp.ServiceName,
 ocsp.ServicePackageId,
 ocsp.PackageName,
 ocsp.DomesticBandwidth,
 ocsp.InternationalBandwidth,
 ocsp.HasStartAndEndPoint,
 ocsp.PackagePrice,
 ocsp.EquipmentAmount,
 ocsp.InstallationFee,
 ocsp.OtherFee,
 ocsp.TaxAmount,
 ocsp.SubTotalBeforeTax,
 ocsp.SubTotal,
 ocsp.GrandTotalBeforeTax,
 ocsp.GrandTotal, 
 ocsp.TimeLine_PaymentPeriod as PaymentPeriod,
 ocsp.TimeLine_Signed as `Signed`,
 ocsp.TimeLine_Effective as Effective,
 ocsp.TimeLine_LatestBilling as LatestBilling,
 ocsp.TimeLine_NextBilling as NextBilling,

 sp.`Id` AS `Id`,
 sp.`CurrencyUnitId` AS `CurrencyUnitId`,
 sp.`CurrencyUnitCode` AS `CurrencyUnitCode`,
 sp.`PointType` AS `PointType`,
 sp.`InstallationFee` AS `InstallationFee`,
 sp.`OtherFee` AS `OtherFee`,
 sp.`MonthlyCost` AS `MonthlyCost`,
 sp.`EquipmentAmount` AS `EquipmentAmount`,
 '' AS `InstallationAddressSpliter`,
 sp.`InstallationAddress_Building` AS `Building`,
 sp.`InstallationAddress_Floor` AS `Floor`,
 sp.`InstallationAddress_RoomNumber` AS `RoomNumber`,
 sp.`InstallationAddress_Street` AS `Street`,
 sp.`InstallationAddress_District` AS `District`,
 sp.`InstallationAddress_DistrictId` AS `DistrictId`,
 sp.`InstallationAddress_City` AS `City`,
 sp.`InstallationAddress_CityId` AS `CityId`,

 ep.`Id` AS `Id`,
 ep.`CurrencyUnitId` AS `CurrencyUnitId`,
 ep.`CurrencyUnitCode` AS `CurrencyUnitCode`,
 ep.`PointType` AS `PointType`,
 ep.`InstallationFee` AS `InstallationFee`,
 ep.`OtherFee` AS `OtherFee`,
 ep.`MonthlyCost` AS `MonthlyCost`,
 ep.`EquipmentAmount` AS `EquipmentAmount`,
 '' AS `InstallationAddressSpliter`,
 ep.`InstallationAddress_Building` AS `Building`,
 ep.`InstallationAddress_Floor` AS `Floor`,
 ep.`InstallationAddress_RoomNumber` AS `RoomNumber`,
 ep.`InstallationAddress_Street` AS `Street`,
 ep.`InstallationAddress_District` AS `District`,
 ep.`InstallationAddress_DistrictId` AS `DistrictId`,
 ep.`InstallationAddress_City` AS `City`,
 ep.`InstallationAddress_CityId` AS `CityId`,

 tol.Id AS `Id`,
 tol.CurrencyUnitId AS `CurrencyUnitId`,
 tol.CurrencyUnitCode AS `CurrencyUnitCode`,
 tol.OutContractId AS `OutContractId`,
 tol.InContractId AS `InContractId`,
 tol.PromotionTotalAmount AS `PromotionTotalAmount`,
 tol.ServicePackageAmount AS `ServicePackageAmount`,
 tol.TotalTaxAmount AS `TotalTaxAmount`,
 tol.InstallationFee AS `InstallationFee`,
 tol.OtherFee AS `OtherFee`,
 tol.EquipmentAmount AS `EquipmentAmount`,
 tol.SubTotalBeforeTax AS `SubTotalBeforeTax`,
 tol.SubTotal AS `SubTotal`,
 tol.GrandTotalBeforeTax AS `GrandTotalBeforeTax`,
 tol.GrandTotal AS `GrandTotal`,
 ict.Id,
 ict.CurrencyUnitId

 FROM OutContracts oc
 INNER JOIN Contractors cus ON cus.Id = oc.ContractorId
 INNER JOIN ContractTotalByCurrencies tol ON tol.OutContractId = oc.Id AND (currencyUnitId =0 OR tol.CurrencyUnitId = currencyUnitId)
 INNER JOIN OutContractServicePackages ocsp ON ocsp.OutContractId = oc.Id
 INNER JOIN OutputChannelPoints ep ON ep.Id = ocsp.EndPointChannelId
 INNER JOIN ContractSharingRevenueLines csr ON csr.OutContractId = oc.Id
 LEFT JOIN InContracts ict ON ict.Id = csr.InContractId
 LEFT JOIN OutputChannelPoints sp ON sp.Id = ocsp.StartPointChannelId

 WHERE oc.IsDeleted = 0
	AND ocsp.IsDeleted = 0
	AND csr.InContractId = inContractId
	AND ocsp.CurrencyUnitId = ict.CurrencyUnitId
	AND (currencyUnitId = 0 OR ict.CurrencyUnitId = currencyUnitId)	
	AND ocsp.StatusId NOT IN (2,3);
END