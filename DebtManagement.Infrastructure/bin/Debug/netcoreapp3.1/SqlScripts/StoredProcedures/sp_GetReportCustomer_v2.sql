CREATE PROCEDURE `sp_GetReportCustomer`(

	IN `orderBys` MEDIUMTEXT,
	IN `dir` MEDIUMTEXT,
	IN `skips` INT,
	IN `take` INT,
	IN `filters` MEDIUMTEXT 
	)
BEGIN
	DROP TEMPORARY TABLE
	IF
		EXISTS outcontract,
		servicepackage,
		equipment,
		receiptvoucher;
	CREATE TEMPORARY TABLE outcontract SELECT
	ct.Id,
	ct.Id AS ContractId,
	ct.ContractCode,
	ct.MarketAreaName,
	ct.ProjectName,
	ct.ContractStatusId,
	ct.Payment_Address AS paymentAdd,
	ctstatus.NAME AS IsActive,
	CONVERT ( ct.TimeLine_Signed, DATE ) AS TimeLineSigned,
	CONVERT ( ct.TimeLine_Expiration, DATE ) AS TimeLineExpiration,
	us.ClassId,
	class.ClassName AS CustomerClass,
	us.UserName,
	us.FullName AS customerName,
	us.MobilePhoneNo AS PhoneNo,
	us.TaxIdNo,
	us.CustomerCode,
	us.Email,
	us.Address 
	FROM
		ITC_FBM_Contracts.OutContracts AS ct
		INNER JOIN ITC_FBM_Contracts.Contractors AS ctor ON ctor.Id = ct.ContractorId
		LEFT JOIN ITC_FBM_Contracts.ContractStatus AS ctstatus ON ct.ContractStatusId = ctstatus.Id
		LEFT JOIN ITC_FBM_CRM.ApplicationUsers AS us ON us.IdentityGuid = ctor.IdentityGuid
		LEFT JOIN ITC_FBM_CRM.ApplicationUserClasses AS class ON us.ClassId = class.Id 
	ORDER BY
		ct.Id ASC	
	LIMIT take OFFSET skips;
	
	CREATE TEMPORARY TABLE servicepackage SELECT
	csp.Id AS cspId,
	csp.OutContractId,

	csp.PackagePrice,
	csp.ServiceId,
	csp.ServiceName,
	csp.ServicePackageId,
	csp.PackageName AS servicePackageName,
	CONCAT( csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom ) AS InternationalBandwidth,
	CONCAT( csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS DomesticBandwidth,
	csp.BandwidthLabel,
	csp.InstallationFee,
	'Home' AS contractAdd,
	csp.TimeLine_PrepayPeriod AS TimeLinePrepayPeriod,
	CONVERT ( csp.TimeLine_Effective, DATE ) AS TimeLineEffective,
	CONVERT ( DATE_ADD(csp.TimeLine_NextBilling, INTERVAL csp.TimeLine_PaymentPeriod MONTH), DATE ) AS TimeLineLatestBilling,
	CONVERT ( csp.TimeLine_NextBilling, DATE ) AS TimeLineNextBilling,
	CASE
			
			WHEN pfc.PromotionType = 3 THEN
			pfc.PromotionValue ELSE 0 
		END AS promotionDateQuantity 
	FROM
		ITC_FBM_Contracts.OutContractServicePackages AS csp
		LEFT JOIN ITC_FBM_Contracts.PromotionForContracts AS pfc ON pfc.OutContractServicePackageId = csp.Id
		LEFT JOIN ITC_FBM_Contracts.ContractEquipments AS eq ON eq.OutContractPackageId = csp.Id
		LEFT JOIN ITC_FBM_Contracts.EquipmentStatuses AS eqs ON eq.StatusId = eqs.Id 
		AND pfc.PromotionType = 3;
	CREATE TEMPORARY TABLE equipment SELECT
	oct.Id AS OutContractId,
	csp.Id AS OutContractServicePackageId,
	ce.EquipmentId,
	ce.EquipmentName,
	GROUP_CONCAT( IFNULL( ce.SerialCode, '' ) SEPARATOR '; ' ) AS EquipmentSerial,
	SUM( ce.RealUnit ) AS EquipmentQuantity 
	FROM
		ITC_FBM_Contracts.ContractEquipments AS ce
		INNER JOIN ITC_FBM_Contracts.OutContractServicePackages AS csp ON csp.Id = ce.OutContractPackageId
		INNER JOIN ITC_FBM_Contracts.OutContracts AS oct ON oct.id = csp.OutContractId 
	GROUP BY
		oct.Id ,
		csp.Id ,
		ce.EquipmentId,
		ce.EquipmentName;
	CREATE TEMPORARY TABLE receiptvoucher SELECT
	rv.Id AS ReceiptVoucherId,
	rv.OutContractId,
	rvDebt.debt AS RemainingTotal,
	IssuedDate,
	Content,
	CAST( GrandTotal AS SIGNED ) AS GrandTotal,
	CAST( PaidTotal AS SIGNED ) AS PaidTotal,
	PaymentDate 
	FROM
		ReceiptVouchers AS rv
		LEFT JOIN (
		SELECT
			OutContractId,
			@debt := SUM( IFNULL( RemainingTotal, 0 ) ) AS debt 
		FROM
			ReceiptVouchers AS rv 
		WHERE
			 ( rv.StatusId NOT IN ( 4, 5 ) OR ( rv.StatusId = 6 AND Payment_Method = 1 ) ) #đã thu và đã hủy + thu hộ nhưng là tiền mặt\
			
		GROUP BY
			OutContractId 
		) AS rvDebt ON rv.OutContractId = rvDebt.OutContractId 
;
	
	SET @sqlQuery = CONCAT( '	
		SELECT DISTINCT
		outcontract.*,
		CASE WHEN 	IFNULL(TIMESTAMPDIFF( MONTH, TimeLineEffective,TimeLineNextBilling ),-1) < 0 THEN 0 
		ELSE TIMESTAMPDIFF( MONTH, TimeLineEffective, TimeLineNextBilling ) + 1 + promotionDateQuantity 
		END AS TotalMonthUse,
		servicepackage.* ,
		equipment.*,
		receiptvoucher.*
		FROM
		outcontract
		LEFT JOIN servicepackage ON outcontract.ContractId = servicepackage.OutContractId			
		LEFT JOIN equipment ON equipment.OutContractId = outcontract.ContractId 	AND equipment.OutContractServicePackageId = 	servicepackage.cspId
		LEFT JOIN	receiptvoucher ON receiptvoucher.OutContractId = outcontract.ContractId
		WHERE 1=1 ', filters, ' 
	ORDER BY ', orderBys, ' ,outcontract.ContractId ASC;' );
	PREPARE stmt 
	FROM
		@sqlQuery;
	EXECUTE stmt;
	DEALLOCATE PREPARE stmt;

END