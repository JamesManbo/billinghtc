CREATE PROCEDURE `GetOutContractSimpleAllByIds`(
IN ids VARCHAR(4000)
)
BEGIN
    CALL SplitReturnTemp (ids);

	SELECT oc.Id,
        `cus`.`ContractorFullName`,
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
        `oc`.`SignedUserId`,
        `oc`.`SalesmanId`,
        `oc`.`Description`,       
        `oc`.`FiberNodeInfo`,
        `oc`.`ContractNote`,
        `oc`.`AgentContractCode`,
        `oc`.`OrganizationUnitId`,
        `oc`.`AgentId`,
        `oc`.`OrganizationUnitName`,

        `cus`.`Id`,
        `cus`.`ContractorFullName`,
        `cus`.`ContractorPhone`,

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
         tol.GrandTotal AS `GrandTotal`
    FROM `OutContracts` oc
    INNER JOIN `Contractors` cus ON cus.Id = oc.ContractorId
    INNER JOIN OutContractServicePackages ocsp ON ocsp.OutContractId = oc.Id
    INNER JOIN ContractTotalByCurrencies tol ON tol.OutContractId = oc.Id
    WHERE 
        oc.Id IN (SELECT val FROM temp)
        AND oc.IsDeleted = FALSE;
END