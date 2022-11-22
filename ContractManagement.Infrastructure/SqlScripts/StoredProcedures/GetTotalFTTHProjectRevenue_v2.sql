CREATE PROCEDURE `GetTotalFTTHProjectRevenue`(
		IN `timeLineSignedStartDate` DATE,
		IN `timeLineSignedEndDate` DATE,
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
	t1.MarketAreaName,
	t1.CityName,
	YEAR ( t1.TimeLine_Signed ) AS ContractYear,
	t1.ProjectName,
	t2.NAME AS StatusName,
	t3.ContractorPhone AS PhoneNumber,
	t3.ContractorAddress AS Address,
	t3.ContractorCode AS CustomerCode,
	t1.ContractCode,
	t1.CustomerCareStaffUserId,
	t4.ServiceName,
	t4.PackageName,
	t4.TimeLine_PrepayPeriod AS Prepay,
	t5.PromotionValue AS MonthsGiven,
	TIMESTAMPDIFF( MONTH, t4.TimeLine_StartBilling, CURRENT_DATE ) AS MonthUse,
	t4.PackagePrice AS MonthlyContractFee,
	0 AS InternetMonthlyFee,
	0 AS TVMonthlyfee,
	t6.OutSharedPackagePercent AS ShareRateForInternet,
	0 AS ShareRateForTV,
	t4.PackagePrice * ( 100 - t6.OutSharedPackagePercent )/ 100 AS HTCChargesReceivedMonthly,
	t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) AS HTCChargesReceivedAfterPromotion,
	0 AS ChargesCollected1st,
	t1.TimeLine_Signed AS ContractSigningDate,
	t4.TimeLine_Effective AS DateOfAcceptance,
	DATE_ADD( t1.TimeLine_Signed, INTERVAL 12 MONTH ) AS ExpirationDate,
	t1.TimeLine_Expiration AS LiquidationDate,
CASE
		
		WHEN t7.Type = 8 
		AND DATE( t1.TimeLine_Expiration ) - DATE( t7.EffectiveDate ) > 0 THEN
			1 ELSE 0 
		END IsAfterTerm,
CASE
		
		WHEN t7.Type = 8 
		AND DATE( t1.TimeLine_Expiration ) - DATE( t7.EffectiveDate ) > 0 THEN
			TIMESTAMPDIFF( MONTH, t7.EffectiveDate, t1.TimeLine_Expiration ) * t4.PackagePrice ELSE 0 
		END LossOfRevenueDueToLiquidation,
	t7.ReasonCancelAcceptance AS DetailReasonForLiquidation,
	0 AS LiquidationType,
	t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) * 12 AS PreYearEconomy,
CASE
		
		WHEN t1.ContractStatusId = 5 
		AND YEAR ( t1.TimeLine_Signed ) = YEAR ( CURRENT_DATE ) - 1 THEN
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) ELSE 0 
			END AS EconomyReductionLastYear,
		0 AS NewGrowthRevenue,
		0 AS TotalRevenue,
t4.TimeLine_StartBilling,
t6.OutSharedPackagePercent,
t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) AS HTCChargesReceivedAfterPromotion,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 1 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS JanRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 2 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 2 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS FebRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 3 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 3 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS MarRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 4 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 4 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS AprRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 5 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 5 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS MayTotal,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 6 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 6 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS MayRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 7 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 7 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS JulRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 8 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 8 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS AugRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 9 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 9 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS SepRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 10 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 10 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS OctRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 11 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 11 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS NovRevenue,
	CASE
			
			WHEN MONTH ( t4.TimeLine_StartBilling ) = 12 THEN
			DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
			WHEN MONTH ( t4.TimeLine_StartBilling ) < 12 THEN
			(
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
		END AS DecRevenue,
		'' AS Note 
	FROM
		outcontracts AS t1
		LEFT JOIN contractstatus AS t2 ON t2.Id = t1.ContractStatusId
		LEFT JOIN contractors AS t3 ON t3.Id = t1.ContractorId
		LEFT JOIN outcontractservicepackages AS t4 ON t4.OutContractId = t1.Id
		LEFT JOIN promotionforcontracts AS t5 ON t5.OutContractServicePackageId = t4.Id
		LEFT JOIN contractsharingrevenuelines AS t6 ON t6.OutContractId = t1.Id
		LEFT JOIN transactions AS t7 ON t7.OutContractId = t1.Id 
	WHERE
	IFNULL( t5.PromotionType, 2 ) = 2 
	AND IFNULL( t5.IsApplied, 1 ) = 1
	;
END