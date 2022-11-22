CREATE PROCEDURE `GetListMasterCustomerReport`(

		IN `timeLineSignedStartDate` Date,
		IN `timeLineSignedEndDate` Date,
		IN `projectId` INT,
		IN `serviceId` INT,
		IN `isEnterprise` TINYINT,
		
		IN `orderBys` MEDIUMTEXT,
		IN `dir` MEDIUMTEXT,
		IN `skips` INT,
		IN `take` INT,
		IN `filters` MEDIUMTEXT ,
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
-- 		SET outContractCode = '%' + outContractCode + '%';
		CREATE TEMPORARY TABLE outcontract 
		SELECT
		ct.Id,
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
			
		WHERE ct.TimeLine_Signed BETWEEN IFNULL(timeLineSignedStartDate,'1900-01-01') AND IFNULL(timeLineSignedEndDate,'2900-01-01')
			AND (projectId = 0 OR ct.ProjectId = projectId)
			AND (isEnterprise = 2 OR ctor.IsEnterprise = isEnterprise)

		LIMIT take OFFSET skips
		;
			
		CREATE TEMPORARY TABLE servicepackage 	
		SELECT
			csp.OutContractId,
			csp.Id AS ServicePackageId,
			csp.HasStartAndEndPoint,
			sPoint.InstallationAddress_Street AS InstallationStartPointAddressStreet,
			ePoint.InstallationAddress_Street AS InstallationAddressStreet,
			csp.ServiceName,
			csp.PackageName AS servicePackageName,
			CONCAT( csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom, ' / ', csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS Bandwidth,
			CONVERT ( csp.InstallationFee, SIGNED ) AS InstallationFee,
			CONVERT ( csp.InstallationFee, SIGNED ) AS InstallationFeeUSD,
			CONVERT ( csp.PackagePrice, SIGNED ) AS PackagePrice,
			CONVERT ( csp.PackagePrice, SIGNED ) AS PackagePriceUSD,
			csp.TimeLine_PaymentPeriod AS TimeLinePaymentPeriod,
			CONVERT ( csp.TimeLine_Effective, DATE ) AS TimeLineEffective 
		FROM
			OutContractServicePackages AS csp
			INNER JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
			INNER JOIN OutputChannelPoints AS ePoint ON csp.StartPointChannelId = ePoint.Id
		WHERE (serviceId = 0 OR csp.ServiceId = serviceId)
		;
			
		CREATE TEMPORARY TABLE revenue
			SELECT
			OutContractId,
			CONVERT ( SUM( IF ( YEAR ( IssuedDate ) < yearReport, PaidTotal, 0 ) ), SIGNED ) AS LastYearAmount,
			CONVERT ( SUM( IF ( YEAR ( IssuedDate ) = yearReport, ReductionFreeTotal + Discount_Amount + PromotionTotalAmount + DiscountAmountSuspendTotal, 0 ) ), SIGNED ) AS LostRevenue,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 1, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth1,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 2, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth2,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 3, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth3,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 4, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth4,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 5, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth5,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 6, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth6,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 7, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth7,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 8, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth8,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 9, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth9,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 10, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth10,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 11, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth11,
			CONVERT ( SUM( IF ( MONTH ( IssuedDate ) = 12, PaidTotal, 0 ) ), SIGNED ) AS ValueMonth12 ,
			CONVERT ( SUM( PaidTotal), SIGNED ) AS valueYearNow 
		FROM
			ITC_FBM_Debts.ReceiptVouchers 
		WHERE
			YEAR ( IssuedDate ) = yearReport 
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
			ORDER BY ',orderBys,' ,oct.ContractId ASC;
			' );
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		SELECT FOUND_ROWS() into total;
    DEALLOCATE PREPARE stmt;


	END