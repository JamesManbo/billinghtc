CREATE PROCEDURE `GetOutContractSharingRevenueByInContract`(
IN inContractId INT,
IN inContractType INT,
IN currencyUnitId INT 
)
BEGIN
		SELECT `oc`.`Id` AS `OutContractId`,
		`oc`.`CurrencyUnitId`,
		`oc`.`CurrencyUnitCode`,
		`oc`.`IdentityGuid`,
		`oc`.`ContractCode`,
		`oc`.`AgentCode`,
		`oc`.`MarketAreaId`,
		`oc`.`MarketAreaName`,
		`oc`.`ProjectId`,
		`oc`.`ProjectName`,
		`oc`.`ContractTypeId`,
		`oc`.`ContractStatusId`,
		`oc`.`ContractorId`,
		`c`.`ContractorFullName`,
		`oc`.`SignedUserId`,
		`oc`.`SalesmanId`,
		`oc`.`Description`,
		`oc`.`TotalTaxPercent`,
		`oc`.`IsIncidentControl`,
		`oc`.`IsControlUsageCapacity`,
		`oc`.`FiberNodeInfo`,
		`oc`.`ContractNote`,
		`oc`.`AgentContractCode`,
		`oc`.`OrganizationUnitId`,
		`oc`.`CashierUserId`,
		`oc`.`AgentId`,
		`oc`.`OrganizationUnitName`,
		`oc`.`SignedUserName`,
								   
			`osrl`.`SharingType`,
			`ocsp`.`Id`,
			`ocsp`.`CurrencyUnitId`,
			`ocsp`.`CurrencyUnitCode`,
			`ocsp`.`OutContractId`,
			`ocsp`.`ServicePackageId`,
			`ocsp`.`ServiceId`,
			`ocsp`.`ServiceName`,
			`ocsp`.`PackageName`,
			`ocsp`.`IsFreeStaticIp`,
			`ocsp`.`BandwidthLabel`,
			`ocsp`.`InternationalBandwidth`,
			`ocsp`.`DomesticBandwidth`,
			`ocsp`.`InternationalBandwidthUom`,
			`ocsp`.`DomesticBandwidthUom`,
			`ocsp`.`CustomerCode`,
			`ocsp`.`CId`,
			`ocsp`.`HasStartAndEndPoint`,
			`ocsp`.`TaxAmount`,
			`ocsp`.`InstallationFee`,
			`ocsp`.`OtherFee`,
			`ocsp`.`PackagePrice`,
			`ocsp`.`SubTotalBeforeTax`,
			`ocsp`.`SubTotal`,
			`ocsp`.`GrandTotal`,
			`ocsp`.`GrandTotalBeforeTax`,
			`ocsp`.`EquipmentAmount`,
			`ocsp`.`StatusId`,
			`ocsp`.`TimeLine_PaymentPeriod` AS `PaymentPeriod`,
			`ocsp`.`TimeLine_Effective` AS `Effective`,
			`ocsp`.`TimeLine_Signed` AS `Signed`,
			`ocsp`.`TimeLine_LatestBilling` AS `LatestBilling`,
			`ocsp`.`TimeLine_NextBilling` AS `NextBilling`,
			`ocsp`.`TimeLine_SuspensionStartDate` AS `SuspensionStartDate`,
			`ocsp`.`TimeLine_SuspensionEndDate` AS `SuspensionEndDate`,
			`ocsp`.`TimeLine_DaysSuspended` AS `DaysSuspended`,        
			`osrl`.`Id`,

		#`osrl`.`OutContractPackageId`,
		#`osrl`.`ServiceId`,
-- 		`osrl`.`ServiceName`,
-- 		`osrl`.`ServicePackageId`,
-- 		`osrl`.`ServicePackageName`,
		`osrl`.`InServiceChannelId`,
		`osrl`.`OutServiceChannelId`,
		`osrl`.`OutSharedInstallFeePercent`,
		`osrl`.`OutSharedPackagePercent`,
		`osrl`.`OutSharedFixedAmount`
					
									  
	FROM `ContractSharingRevenueLines` AS osrl
	INNER JOIN `OutContracts` AS oc ON oc.Id = osrl.OutContractId AND osrl.CurrencyUnitCode = oc.CurrencyUnitCode	
	INNER JOIN `OutContractServicePackages` AS ocsp ON osrl.OutServiceChannelId = ocsp.Id
  INNER JOIN `Contractors` AS c ON c.Id = oc.ContractorId
	WHERE 1 = 1
	AND osrl.InContractId = inContractId
	AND osrl.SharingType = inContractType
	AND oc.CurrencyUnitId = currencyUnitId
	AND oc.IsDeleted = FALSE
	AND ocsp.IsDeleted = FALSE
	AND osrl.IsDeleted = FALSE
  AND osrl.IsDeleted = FALSE
;
END