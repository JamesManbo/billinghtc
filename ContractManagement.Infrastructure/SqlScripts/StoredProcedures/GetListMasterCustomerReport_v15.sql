CREATE PROCEDURE `GetListMasterCustomerReport`(
		IN `timeLineSignedStartDate` Date,
		IN `timeLineSignedEndDate` Date,
		IN marketAreaId INT,
		IN serviceId INT,
		IN projectId INT,
		IN contractCode VARCHAR(50),
		IN customerCode VARCHAR(50),
		IN `statusId` INT,
		IN `customerCategoryId` INT,
		IN orderBy VARCHAR(100),	
		IN `skips` INT,
		IN `take` INT,

		OUT `total` INT)
BEGIN
DROP TEMPORARY TABLE
		IF EXISTS outcontract;
CREATE TEMPORARY TABLE outcontract 
		SELECT
			ct.Id,
			ct.CurrencyUnitCode,
 			ct.Id AS ContractId,
 			ct.ContractCode,
 			ct.MarketAreaName,
 			ct.TimeLine_Signed AS TimeLineSigned,
 			ct.TimeLine_Expiration AS TimeLineExpiration,
			ct.NumberBillingLimitDays,			
 			ct.OrganizationUnitName,
			ct.SignedUserName,
			ct.ProjectName,
			t2.`Name` AS ContractStatusName,
 			
 			ous.FullName AS  CustomerCare,
 			#User infomation
 			cp.ContractorCategoryName AS CustomerCategory,
 			cp.ContractorGroupNames AS GroupName,
 			cp.ContractorStructureName AS CustomerStruct,
 			cp.ContractorTypeName AS CustomerType,
 			cp.ContractorClassName AS CustomerClass,
 			ctor.ContractorFullName AS CustomerName,
 			ctor.ContractorCode AS CustomerCode ,
 			ctor2.ContractorFullName
 			,
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
 			csp.TimeLine_PrepayPeriod AS TimeLinePrepayPeriod,
 			csp.TimeLine_PaymentPeriod AS TimeLinePaymentPeriod,
 			CONVERT ( csp.TimeLine_Effective, DATE ) AS TimeLineEffective ,
			
			SUM( t1.ReductionFreeTotal + t1.Discount_Amount + t1.PromotionTotalAmount + t1.DiscountAmountSuspendTotal ) AS LostRevenue,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 1,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth1,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 2,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth2,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 3,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth3,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 4,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth4,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 5,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth5,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 6,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth6,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 7,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth7,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 8,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth8,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 9,  ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth9,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 10, ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth10,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 11, ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth11,
 			SUM( IF ( MONTH ( t1.IssuedDate ) = 12, ExchangeMoney( t1.PaidTotal,ct.CurrencyUnitCode), 0 ) ) AS ValueMonth12,
 			SUM( ExchangeMoney(t1.PaidTotal,ct.CurrencyUnitCode)) AS valueYearNow 
 		FROM
 			OutContracts AS ct
 			LEFT  JOIN Contractors AS ctor ON ctor.Id = ct.ContractorId
 			LEFT JOIN ContractorProperties as cp ON cp.ContractorId = ctor.Id
 			LEFT JOIN Contractors AS ctor2 ON ct.AgentId = ctor.IdentityGuid
			LEFT  JOIN ContractStatus AS t2 ON t2.Id = ct.ContractStatusId 			
 			LEFT JOIN ITC_FBM_Organizations.Users AS ous ON ous.IdentityGuid = ct.CustomerCareStaffUserId
			LEFT  JOIN OutContractServicePackages AS csp ON ct.Id = csp.OutContractId
 			LEFT JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
 			LEFT JOIN OutputChannelPoints AS ePoint ON csp.EndPointChannelId = ePoint.Id
 			LEFT JOIN ITC_FBM_Debts.ReceiptVouchers  AS t1 ON ct.Id =  t1.OutContractId
  		WHERE 1=1 
				AND t1.StatusId <> 5			
 				AND DATE(ct.TimeLine_Signed) BETWEEN timeLineSignedStartDate AND timeLineSignedEndDate			
   			AND (marketAreaId = 0 OR ct.MarketAreaId = marketAreaId)				
 				AND (projectId  = 0 OR ct.ProjectId = projectId )
 				AND (contractCode = '' OR ct.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 				AND (customerCode = '' OR ctor.ContractorCode LIKE CONCAT('%', customerCode, '%') ) 
   			AND (customerCategoryId = 0 OR cp.ContractorCategoryId = customerCategoryId)
   			AND (statusId = 0 OR ct.ContractStatusId = statusId)
   			AND (serviceId = 0 OR csp.ServiceId = serviceId)   		

		GROUP BY ct.Id;
		
		SELECT * FROM outcontract
		LIMIT take OFFSET skips;
		SELECT COUNT(ct.Id)
		FROM outcontract AS ct	INTO total;

END