CREATE PROCEDURE `GetTotalRevenueEnterpiseReport`(
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

		OUT `total` INT
		)
BEGIN
	#Routine body goes here...
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
		t4.`Name` AS ContractStatusName,#User infomation
		cp.ContractorCategoryName AS CustomerCategory,
		cp.ContractorGroupNames AS GroupName,
		cp.ContractorStructureName AS CustomerStruct,
		cp.ContractorTypeName AS CustomerType,
		cp.ContractorClassName AS CustomerClass,
		ctor.ContractorFullName AS CustomerName,
		ctor.ContractorCode AS CustomerCode,
		ctor2.ContractorFullName,
		csp.OutContractId,
		csp.Id AS ServicePackageId,
		csp.HasStartAndEndPoint,
		sPoint.InstallationAddress_Street AS InstallationAddressStartPoint,
		ePoint.InstallationAddress_Street AS InstallationAddressEndPoint,
		csp.ServiceName,
		csp.PackageName AS servicePackageName,
		CONCAT( csp.InternationalBandwidth, ' ', csp.InternationalBandwidthUom, ' / ', csp.DomesticBandwidth, ' ', csp.DomesticBandwidthUom ) AS Bandwidth,
		ExchangeMoney ( csp.InstallationFee, ct.CurrencyUnitCode ) AS InstallationFee,
		ExchangeMoney ( csp.PackagePrice, ct.CurrencyUnitCode ) AS PackagePrice,
		csp.TimeLine_PrepayPeriod AS TimeLinePrepayPeriod,
		csp.TimeLine_PaymentPeriod AS TimeLinePaymentPeriod,
		CONVERT ( csp.TimeLine_Effective, DATE ) AS TimeLineEffective,
		t2.`Code`,
		t2.TransactionDate,
		t3.`Name` AS TransactionStatus,
	CASE
			
			WHEN t2.Type IN ( 3, 4, 8 ) THEN
			t2.EffectiveDate ELSE '' 
		END AS TransactionEffectedDate,
		t2.ReasonCancelAcceptance,
	CASE
			
			WHEN t2.Type = 8 
			AND DATE( ct.TimeLine_Expiration ) - DATE( t2.EffectiveDate ) > 0 THEN
				1 ELSE 0 
			END IsEndBeforeExpriedDate,
		TIMESTAMPDIFF( MONTH, ct.TimeLine_Expiration, t2.EffectiveDate ) * csp.PackagePrice AS AmountLost,
		ct.TimeLine_Expiration AS TimeLineExpiration,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 1 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice ELSE 0 
		END AS JanuaryTotal,
		0 AS JanuaryNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 2 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 2 THEN
			csp.PackagePrice ELSE 0 
		END AS FebruaryTotal,
		0 AS FebruaryNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 3 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 3 THEN
			csp.PackagePrice ELSE 0 
		END AS MarTotal,
		0 AS MarNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 4 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 4 THEN
			csp.PackagePrice ELSE 0 
		END AS AprTotal,
		0 AS AprNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 5 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 5 THEN
			csp.PackagePrice ELSE 0 
		END AS MayTotal,
		0 AS MayNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 6 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 6 THEN
			csp.PackagePrice ELSE 0 
		END AS JunTotal,
		0 AS JunNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 7 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 7 THEN
			csp.PackagePrice ELSE 0 
		END AS JulTotal,
		0 AS JulNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 8 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 8 THEN
			csp.PackagePrice ELSE 0 
		END AS AugTotal,
		0 AS AugNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 9 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 9 THEN
			csp.PackagePrice ELSE 0 
		END AS SepTotal,
		0 AS SepNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 10 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 10 THEN
			csp.PackagePrice ELSE 0 
		END AS OctTotal,
		0 AS OctNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 11 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 11 THEN
			csp.PackagePrice ELSE 0 
		END AS NovTotal,
		0 AS NovNewTotal,
	CASE
			
			WHEN MONTH ( csp.TimeLine_StartBilling ) = 12 THEN
			DATEDIFF( LAST_DAY( csp.TimeLine_StartBilling ), csp.TimeLine_StartBilling ) * csp.PackagePrice 
			WHEN MONTH ( csp.TimeLine_StartBilling ) < 12 THEN
			csp.PackagePrice ELSE 0 
		END AS DecTotal,
		0 AS DecNewTotal 
	FROM
		OutContracts AS ct
		LEFT JOIN Contractors AS ctor ON ctor.Id = ct.ContractorId
		LEFT JOIN ContractorProperties AS cp ON cp.ContractorId = ctor.Id
		LEFT JOIN Contractors AS ctor2 ON ct.AgentId = ctor.IdentityGuid
		LEFT JOIN ContractStatus AS t4 ON t4.Id = ct.ContractStatusId
		LEFT JOIN OutContractServicePackages AS csp ON ct.Id = csp.OutContractId
		LEFT JOIN OutputChannelPoints AS sPoint ON csp.StartPointChannelId = sPoint.Id
		LEFT JOIN OutputChannelPoints AS ePoint ON csp.EndPointChannelId = ePoint.Id
		LEFT JOIN Transactions AS t2 ON t2.OutContractId = ct.id
		LEFT JOIN TransactionStatuses AS t3 ON t3.Id = t2.StatusId
	WHERE
		1 = 1
		AND DATE(ct.TimeLine_Signed) BETWEEN timeLineSignedStartDate AND timeLineSignedEndDate			
   		AND (marketAreaId = 0 OR ct.MarketAreaId = marketAreaId)				
 		AND (projectId  = 0 OR ct.ProjectId = projectId )
 		AND (contractCode = '' OR ct.ContractCode LIKE CONCAT('%', contractCode, '%') ) 
 		AND (customerCode = '' OR ctor.ContractorCode LIKE CONCAT('%', customerCode, '%') ) 
		;
END