CREATE PROCEDURE `GetTotalFTTHProjectRevenue`(	
		IN `skips` INT,
		IN `take` INT,
		OUT `totalRecords` INT,
		IN `marketAreaId` INT,
		IN `contractCode` VARCHAR(50),
		IN `customerCode` VARCHAR(50),
        IN `contractorFullName` VARCHAR(50),
		IN `effectiveStartDate` DATE,
		IN `effectiveEndDate` DATE,
		IN `serviceIds` MEDIUMTEXT,
		IN `projectId` INT,
        
        OUT `sumMonthlyContractFee` DECIMAL(65, 30),
        OUT `sumInternetMonthlyFee` DECIMAL(65, 30),
        OUT `sumTVMonthlyfee` DECIMAL(65, 30),
        OUT `sumHTCChargesReceivedMonthly` DECIMAL(65, 30),
        OUT `sumHTCChargesReceivedAfterPromotion` DECIMAL(65, 30),
        OUT `sumChargesCollected1st` DECIMAL(65, 30),
        OUT `sumLossOfRevenueDueToLiquidation` DECIMAL(65, 30),
        OUT `sumPreYearEconomy` DECIMAL(65, 30),
        OUT `sumEconomyReductionLastYear` DECIMAL(65, 30),
        OUT `sumNewGrowthRevenue` DECIMAL(65, 30),
        OUT `sumTotalRevenue` DECIMAL(65, 30),
        OUT `sumEachMonthRevenue` MEDIUMTEXT
        )
BEGIN
	DROP TEMPORARY TABLE
	IF EXISTS temp;
	CREATE TEMPORARY TABLE temp 
		SELECT DISTINCT
			t1.Id,
			t1.MarketAreaName,
			t1.CityName,
			YEAR ( t1.TimeLine_Signed ) AS ContractYear,
			t1.ProjectName,
			t2.NAME AS StatusName,
			cp.ContractorTypeName AS ContractorTypeName,
			t3.ContractorFullName AS ContractorFullName,			 
			t3.ContractorPhone AS PhoneNumber,
			t3.ContractorAddress AS Address,
			t3.ContractorCode AS CustomerCode,
			t1.ContractCode,
			ous.FullName AS CustomerCareStaffUserName,
			t4.ServiceName,
            t4.PackageName AS ServicePackageName,
			t4.TimeLine_PrepayPeriod AS Prepay,
			t5.PromotionValue AS MonthsGiven,
			TIMESTAMPDIFF( MONTH, t4.TimeLine_StartBilling, CURRENT_DATE ) AS MonthUse,
			t4.PackagePrice AS MonthlyContractFee,
			CASE 
				WHEN t4.ServiceId <> 19 THEN t4.PackagePrice	 
				ELSE 0
			END AS InternetMonthlyFee,
			CASE 
				WHEN t4.ServiceId = 19 THEN t4.PackagePrice	 
				ELSE 0
			END AS TVMonthlyfee,
			CASE 
				WHEN t4.ServiceId <> 19 THEN t6.OutSharedPackagePercent	 
				ELSE 0
			END AS ShareRateForInternet,
			CASE 
				WHEN t4.ServiceId = 19 THEN t6.OutSharedPackagePercent	 
				ELSE 0
			END AS ShareRateForTV,
			t4.PackagePrice * ( 100 - t6.OutSharedPackagePercent )/ 100 AS HTCChargesReceivedMonthly,
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) AS HTCChargesReceivedAfterPromotion,
			t4.TimeLine_PrepayPeriod * t4.PackagePrice AS ChargesCollected1st,
			t1.TimeLine_Signed AS ContractSigningDate,
			t4.TimeLine_Effective AS DateOfAcceptance,
			DATE_ADD( t1.TimeLine_Signed, INTERVAL 12 MONTH ) AS ExpirationDate,
			t1.TimeLine_Expiration AS LiquidationDate,
			CASE
				WHEN t7.Type = 8 
				AND DATE( t1.TimeLine_Expiration ) - DATE( t7.EffectiveDate ) > 0 THEN
					'Trước hạn' ELSE 'Sau hạn' 
				END IsAfterTerm,
			CASE	
				WHEN t7.Type = 8 AND DATE( t1.TimeLine_Expiration ) - DATE( t7.EffectiveDate ) > 0 
           THEN TIMESTAMPDIFF( MONTH, t7.EffectiveDate, t1.TimeLine_Expiration ) * t4.PackagePrice 
        ELSE 0 
			END LossOfRevenueDueToLiquidation,
			t7.ReasonCancelAcceptance AS DetailReasonForLiquidation,
			t7.Reason AS LiquidationType,
			t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) * 12 AS PreYearEconomy,
			CASE	
				WHEN t1.ContractStatusId = 5 AND YEAR ( t1.TimeLine_Signed ) = YEAR ( CURRENT_DATE ) - 1 
                THEN
					t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 ) 
				ELSE 0 
				END AS EconomyReductionLastYear,
			0 AS NewGrowthRevenue,
			0 AS TotalRevenue,
			t4.TimeLine_StartBilling,
			t6.OutSharedPackagePercent,
			CASE
				WHEN MONTH ( t4.TimeLine_StartBilling ) = 1 
                THEN
				DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
				t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
                ELSE 0 
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
				END AS MayRevenue,
			CASE
					
					WHEN MONTH ( t4.TimeLine_StartBilling ) = 6 THEN
					DATEDIFF( LAST_DAY( t4.TimeLine_StartBilling ), t4.TimeLine_StartBilling ) * (
					t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) 
					WHEN MONTH ( t4.TimeLine_StartBilling ) < 6 THEN
					(
					t4.PackagePrice * ( 100 - IFNULL(t6.OutSharedPackagePercent,0) )/ 100 - ( IFNULL(t5.PromotionValue,0) * t4.PackagePrice / 12 )) ELSE 0 
				END AS JunRevenue,
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
			t1.ContractNote AS Note 
			FROM OutContracts AS t1
			LEFT JOIN ContractStatus AS t2 ON t2.Id = t1.ContractStatusId
			LEFT JOIN Contractors AS t3 ON t3.Id = t1.ContractorId
			LEFT JOIN ContractorProperties AS cp ON cp.ContractorId = t3.Id
			LEFT JOIN OutContractServicePackages AS t4 ON t4.OutContractId = t1.Id
			LEFT JOIN PromotionForContracts AS t5 ON t5.OutContractServicePackageId = t4.Id
			LEFT JOIN ContractSharingRevenueLines AS t6 ON t6.OutContractId = t1.Id
			LEFT JOIN 
					(	SELECT t1.Id,t1.OutContractId,t1.Reason,t1.ReasonCancelAcceptance,t1.Type,t1.EffectiveDate,
							t2.OutContractServicePackageId
							FROM Transactions AS t1
							LEFT JOIN TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id
							WHERE t1.Type = 8
					)AS t7  ON t7.OutContractId = t1.Id AND t7.OutContractServicePackageId = t4.Id
			LEFT JOIN ITC_FBM_Organizations.Users AS ous ON ous.IdentityGuid = t1.CustomerCareStaffUserId
			WHERE IFNULL( t5.PromotionType, 2 ) = 2 
				AND IFNULL( t5.IsApplied, 1 ) = 1
				AND t1.ProjectId IS NOT NULL
			 	AND	t4.ServiceId IN (serviceIds)
				AND (effectiveStartDate IS NULL OR DATE(t4.TimeLine_Effective) >=  effectiveStartDate)
				AND (effectiveEndDate IS NULL OR DATE(t4.TimeLine_Effective) <=  effectiveEndDate)
				-- AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId)				
				AND (projectId  = 0 OR t1.ProjectId = projectId )
				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%', contractCode, '%') )
				AND (customerCode = '' OR t3.ContractorCode LIKE CONCAT('%', customerCode, '%') )	
		;
		
		--
        SELECT * FROM temp
		LIMIT take OFFSET skips;
		SELECT 
        COUNT(t1.Id),
        SUM(IFNULL(MonthlyContractFee, 0)),
        SUM(IFNULL(InternetMonthlyFee, 0)),
        SUM(IFNULL(TVMonthlyfee, 0)),
        SUM(IFNULL(HTCChargesReceivedMonthly, 0)),
        SUM(IFNULL(HTCChargesReceivedAfterPromotion, 0)),
        SUM(IFNULL(ChargesCollected1st, 0)),
        SUM(IFNULL(LossOfRevenueDueToLiquidation, 0)),
        SUM(IFNULL(PreYearEconomy, 0)),
        SUM(IFNULL(EconomyReductionLastYear, 0)),
        SUM(IFNULL(NewGrowthRevenue, 0)),
        SUM(IFNULL(TotalRevenue, 0)),
		CONCAT(
        SUM(JanRevenue), "/", SUM(FebRevenue),
        "/", SUM(MarRevenue), "/", SUM(AprRevenue),
        "/", SUM(MayRevenue), "/", SUM(JunRevenue), 
        "/", SUM(JulRevenue), "/", SUM(AugRevenue), 
        "/", SUM(SepRevenue), "/", SUM(OctRevenue), 
        "/", SUM(NovRevenue), "/", SUM(DecRevenue))
        INTO totalRecords, sumMonthlyContractFee, sumInternetMonthlyFee, sumTVMonthlyfee, sumHTCChargesReceivedMonthly,
        sumHTCChargesReceivedAfterPromotion, sumChargesCollected1st, sumLossOfRevenueDueToLiquidation, sumPreYearEconomy,
        sumEconomyReductionLastYear, sumNewGrowthRevenue, sumTotalRevenue, sumEachMonthRevenue
		FROM temp AS t1;
	END