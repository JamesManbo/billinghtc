CREATE PROCEDURE `sp_GetFeeTotalReportData`(
	IN marketAreaId int,
	IN outCustomerCode MEDIUMTEXT,
	IN inContractCode MEDIUMTEXT,
	IN outContractCode MEDIUMTEXT,
	IN customerName MEDIUMTEXT,
	IN serviceId INT,
	IN `month` INT,
	IN `quarter` INT,
	IN `year`INT,
	IN signedStartDate date,
	IN signedEndDate date ,
	IN `take` INT,
	IN `skip` INT,
	OUT `total` INT
)
BEGIN
		#Routine body goes here...	
SELECT
		t1.InContractCode,
		t1.OutContractCode,
		t1.CurrencyUnitCode,
        ctor.ContractorFullName as ContractorName,
        csp.ServiceName,
        oct.TimeLine_Expiration as TimeLineExpiration,
		#COUNT(t1.Id) AS count,
		Sum(CASE WHEN t2.SharingType = 1 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareChannelRentalAmount,
		Sum(CASE WHEN t2.SharingType = 2 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareCommissionAmount,
		Sum(CASE WHEN t2.SharingType = 3 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareRevenueAmount,
		Sum(CASE WHEN t2.SharingType = 4 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareMaintenanceAmount,
		Sum(CASE WHEN t2.SharingType = 5 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareInfrastructureAmount,
		Sum(CASE WHEN t2.SharingType = 6 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareConstructionAmount,
		Sum(CASE WHEN t2.SharingType = 7 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareEquipmentAmount,
		Sum(CASE WHEN t2.SharingType = 8 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareSupplyAmount,
		Sum(CASE WHEN t2.SharingType = 9 THEN IFNULL(t2.SharingAmount,0) ELSE 0 END) AS ShareCostOfAcceptanceAmount,
		SUM(IFNULL(t2.SharingAmount,0)) AS TotalShareAmount,
		CASE WHEN `month` > 0 THEN ROUND(ExchangeMoney(csp.PackagePrice, oct.CurrencyUnitCode),0)
			ELSE ROUND(ExchangeMoney(csp.PackagePrice, oct.CurrencyUnitCode)*3,0) END AS ContractRevenue

	FROM ContractSharingRevenueLines AS t1
		INNER JOIN SharingRevenueLineDetails AS t2 ON t1.Id = t2.SharingLineId
		INNER JOIN InContracts AS ict ON ict.Id = t1.InContractId
		INNER JOIN Contractors AS ictor ON ictor.Id = ict.ContractorId
		INNER JOIN OutContractServicePackages AS csp ON csp.Id = t1.OutServiceChannelId
		INNER JOIN OutContracts AS oct ON oct.Id = t1.OutContractId
		INNER JOIN Contractors AS ctor ON ctor.Id = oct.ContractorId 
	WHERE 1 = 1 
 		AND (marketAreaId = 0 OR ict.MarketAreaId = marketAreaId)
 		AND (outCustomerCode = '' OR ctor.ContractorCode  LIKE CONCAT('%', outCustomerCode, '%') )
 		AND (inContractCode = '' OR t1.InContractCode  LIKE CONCAT('%', inContractCode, '%') )	
 		AND (outContractCode = '' OR t1.OutContractCode  LIKE CONCAT('%', outContractCode, '%') )	
 		AND (customerName = '' OR ctor.ContractorFullName  LIKE CONCAT('%', customerName, '%') )	
 		AND (serviceId = 0 OR csp.ServiceId = serviceId)
 		AND (`month` = 0 OR IFNULL(t2.`Month`,0) = `month`)
 		AND (`quarter` = 0 OR IFNULL(t2.`Month`,0) BETWEEN `quarter` - 3 AND `quarter`)
 		AND (`year` = 0 OR IFNULL(t2.`Year`,0) = `year`)
 		AND (signedStartDate IS NULL OR DATE(oct.TimeLine_Signed) >=  signedStartDate)
 		AND (signedEndDate IS NULL OR DATE(oct.TimeLine_Signed) <=  signedEndDate)
		
	GROUP BY
		t1.CurrencyUnitCode,
		t1.SharingType,
		t1.InContractId,
		t1.OutContractId
		LIMIT take OFFSET skip;
		
		SELECT COUNT(*) from ( 	
			SELECT 	t1.CurrencyUnitCode,
					t1.SharingType,
					t1.InContractId,
					t1.OutContractId
				FROM ContractSharingRevenueLines AS t1
					INNER JOIN SharingRevenueLineDetails AS t2 ON t1.Id = t2.SharingLineId
					INNER JOIN InContracts AS ict ON ict.Id = t1.InContractId
					INNER JOIN Contractors AS ictor ON ictor.Id = ict.ContractorId
					INNER JOIN OutContractServicePackages AS csp ON csp.Id = t1.OutServiceChannelId
					INNER JOIN OutContracts AS oct ON oct.Id = t1.OutContractId
					INNER JOIN Contractors AS ctor ON ctor.Id = oct.ContractorId 
				WHERE 1 = 1 
				AND (marketAreaId = 0 OR ict.MarketAreaId = marketAreaId)
				AND (outCustomerCode = '' OR ctor.ContractorCode  LIKE CONCAT('%', outCustomerCode, '%') )
				AND (inContractCode = '' OR t1.InContractCode  LIKE CONCAT('%', inContractCode, '%') )	
                AND (outContractCode = '' OR t1.OutContractCode  LIKE CONCAT('%', outContractCode, '%') )	
                AND (customerName = '' OR ctor.ContractorFullName  LIKE CONCAT('%', customerName, '%') )	
				AND (serviceId = 0 OR csp.ServiceId = serviceId)
				AND (`month` = 0 OR IFNULL(t2.`Month`,0) = `month`)
				AND (`quarter` = 0 OR IFNULL(t2.`Month`,0) BETWEEN `quarter` - 2 AND `quarter`)
				AND (`year` = 0 OR IFNULL(t2.`Year`,0) = `year`)
				AND (signedStartDate IS NULL OR DATE(oct.TimeLine_Signed) >=  signedStartDate)
				AND (signedEndDate IS NULL OR DATE(oct.TimeLine_Signed) <=  signedEndDate)
					
				GROUP BY
					t1.CurrencyUnitCode,
					t1.SharingType,
					t1.InContractId,
					t1.OutContractId
			) AS tb1
			INTO total ;
		
END