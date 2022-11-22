CREATE PROCEDURE `GetListMasterCustomerReport`(
		IN `timeLineSignedStartDate` VARCHAR(20),
		IN `timeLineSignedEndDate` VARCHAR(20),
		IN `projectId` INT,
		IN `serviceId` INT,
		IN `statusId` INT,
		IN `isEnterprise` TINYINT,
		IN orderBy VARCHAR(100),	
		IN `skips` INT,
		IN `take` INT,

		OUT `total` INT)
BEGIN
		
		#Routine body goes here...

		DROP TEMPORARY TABLE
		IF
			EXISTS outcontract,
			servicepackage,
			receiptvoucher,
			revenue,
			sharingRevenue;
			
 		CREATE TEMPORARY TABLE outcontract 
 		SELECT
			ct.Id,
			ct.CurrencyUnitCode,
 			ct.Id AS ContractId,
 			ct.ContractCode,
 			ct.MarketAreaName,
 			ct.TimeLine_Signed AS TimeLineSigned,
 			ct.TimeLine_Expiration AS TimeLineExpiration,
 			ct.OrganizationUnitName,
 			
 			ous.FullName AS  customerCare,
 			#User infomation
 			cusCat.`Name` AS CustomerCategory,
 			grp.GroupName,
 			cusStrt.`Name` AS CustomerStruct,
 			cusType.`Name` AS CustomerType,
 			cusClass.ClassName AS CustomerClass,
 			us.FullName AS CustomerName,
 			us.CustomerCode ,
 			ctor2.ContractorFullName
 			
 		FROM
 			OutContracts AS ct
 			INNER JOIN Contractors AS ctor ON ctor.Id = ct.ContractorId
 			LEFT JOIN ITC_FBM_CRM.ApplicationUsers AS us ON us.IdentityGuid = ctor.IdentityGuid
 			LEFT JOIN ITC_FBM_CRM.ApplicationUserClasses AS cusClass ON us.ClassId = cusClass.Id
 			LEFT JOIN ITC_FBM_CRM.ApplicationUserUserGroups AS cusGrp ON us.Id = cusGrp.Id
 			LEFT JOIN ITC_FBM_CRM.ApplicationUserGroups AS grp ON grp.Id = cusGrp.GroupId
 			LEFT JOIN ITC_FBM_CRM.CustomerCategories AS cusCat ON us.CustomerCategoryId = cusCat.Id
 			LEFT JOIN ITC_FBM_CRM.CustomerStructure AS cusStrt ON us.CustomerStructureId = cusStrt.Id
 			LEFT JOIN ITC_FBM_CRM.CustomerTypes AS cusType ON us.CustomerTypeId = cusType.Id
 			LEFT JOIN Contractors AS ctor2 ON ct.AgentId = ctor.IdentityGuid
 			
 			LEFT JOIN ITC_FBM_Organizations.Users AS ous ON ous.IdentityGuid = ct.CustomerCareStaffUserId
 			
  		WHERE 1=1 
				AND DATE(ct.TimeLine_Signed) BETWEEN DATE(timeLineSignedStartDate) AND DATE(timeLineSignedEndDate)
  			AND (projectId = 0 OR ct.ProjectId = projectId)
  			AND (isEnterprise = 2 OR ctor.IsEnterprise = isEnterprise)
  			AND (statusId = 0 OR ct.ContractStatusId = statusId)
  
 		LIMIT take OFFSET skips
 		;
 			
 		CREATE TEMPORARY TABLE servicepackage 	
 		SELECT
 			csp.OutContractId,
 			csp.Id AS ServicePackageId,
 			csp.HasStartAndEndPoint,
 			sPoint.InstallationAddress_Street AS InstallationAddressStartPoint,
 			ePoint.InstallationAddress_Street AS InstallationAddressEndPoint,
 			csp.ServiceName,
 			csp.PackageName AS servicePackageName,
 			CONCAT( csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom, ' / ', csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS Bandwidth,
 			ExchangeMoney(csp.InstallationFee,ct.CurrencyUnitCode)  AS InstallationFee,
 			ExchangeMoney(csp.PackagePrice   ,ct.CurrencyUnitCode) AS PackagePrice,
 			csp.TimeLine_PaymentPeriod AS TimeLinePaymentPeriod,
 			CONVERT ( csp.TimeLine_Effective, DATE ) AS TimeLineEffective 
 		FROM
 			OutContractServicePackages AS csp
 			LEFT JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
 			LEFT JOIN OutputChannelPoints AS ePoint ON csp.EndPointChannelId = ePoint.Id
			INNER JOIN outcontract AS ct ON ct.Id = csp.OutContractId
  		WHERE csp.StatusId = 0
 			AND (serviceId = 0 OR csp.ServiceId = serviceId)
 		;
	
 		CREATE TEMPORARY TABLE revenue
 			SELECT
 			t1.OutContractId,
 			#CONVERT ( SUM( ExchangeMoney(t1.PaidTotal,oct.CurrencyUnitCode) ), SIGNED ) 
			0 AS LastYearAmount,
 			SUM( t1.ReductionFreeTotal + t1.Discount_Amount + t1.PromotionTotalAmount + t1.DiscountAmountSuspendTotal ) AS LostRevenue,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 1,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth1,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 2,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth2,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 3,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth3,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 4,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth4,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 5,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth5,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 6,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth6,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 7,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth7,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 8,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth8,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 9,  ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth9,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 10, ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth10,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 11, ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth11,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 12, ExchangeMoney( t1.PaidTotal,oct.CurrencyUnitCode), 0 ) ) AS ValueMonth12 ,
 			SUM( ExchangeMoney(t1.PaidTotal,oct.CurrencyUnitCode)) AS valueYearNow 
 		FROM
 			ITC_FBM_Debts.ReceiptVouchers  AS t1 
			INNER JOIN outcontract AS oct ON oct.Id =  t1.OutContractId
 		WHERE 1=1
 			AND YEAR ( IssuedDate ) = YEAR(CURDATE()) 
 		GROUP BY
 			OutContractId 
 		ORDER BY
 			OutContractId;
			
	CREATE TEMPORARY TABLE sharingRevenue
	SELECT  DISTINCT
		csr.OutContractId,
		csr.InContractCode,
		csr.InContractId,
		
		CASE WHEN csrl.SharingType = 2 THEN 'Hoa hồng'
				WHEN  csrl.SharingType = 3 THEN 'Doanh thu'
			ELSE
				'Không'
		END AS HasSharing ,		
		csrl.SharingType,
		csrl.SharedPackagePercent AS HTCPercent,
		CONVERT(csrl.SharedPackagePercent,SIGNED) AS SharedPackagePercent,
		100-csrl.SharedPackagePercent as PartnerPercent,
		ctor.ContractorFullName AS InContractorFullName
	FROM ContractSharingRevenues AS csr
	INNER JOIN outcontract AS oct ON oct.Id = csr.OutContractId
	INNER JOIN ContractSharingRevenueLines AS csrl ON csr.Id = csrl.CsrId
	INNER JOIN InContracts AS ict ON ict.Id = csr.InContractId
	INNER JOIN Contractors AS ctor ON ctor.Id = ict.ContractorId
	WHERE csr.IsActive = TRUE AND csr.IsDeleted = FALSE

	ORDER BY OutContractId
	;
	
	
SET @sqlQuery = CONCAT( '				
		SELECT
			* 
		FROM
			outcontract AS oct
			LEFT JOIN servicepackage AS sv ON sv.OutContractId = oct.ContractId
			LEFT JOIN revenue AS rv ON rv.OutContractId = oct.ContractId 
			LEFT JOIN sharingRevenue as sr ON sr.OutContractId = oct.ContractId
			  ',orderBy,'	
			LIMIT ',take,' OFFSET ',skips,';' );
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		SELECT
			COUNT(*) 
		FROM
			outcontract AS oct
			LEFT JOIN servicepackage AS sv ON sv.OutContractId = oct.ContractId
			LEFT JOIN revenue AS rv ON rv.OutContractId = oct.ContractId 
			LEFT JOIN sharingRevenue as sr ON sr.OutContractId = oct.ContractId into total;
    DEALLOCATE PREPARE stmt;


	END