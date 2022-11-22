CREATE PROCEDURE `sp_GetTotalSharingRevenusByReceiptVoucher`(
	IN skip INT,
	IN inContractId INT,
	IN startDate DATE,
	IN endDate DATE	,
	OUT total INT
)
BEGIN
	#Routine body goes here...
	
SELECT
	t1.CurrencyUnitCode,
	t4.ReceiptVoucherId,
	t5.VoucherCode,
	t5.MarketAreaName,
	t5.ProjectName,
	t5.IssuedDate,
	t5.ContractCode,
	t5.CashTotal,
	t5.GrandTotal,
	t4.Id,
	t1.InSharedInstallFeePercent AS SPointSharedInstallFeePercent,
	sPoint.InstallationFee AS SPointInstallationFee,
	CASE WHEN t5.IsFirstVoucherOfContract THEN IFNULL( sPoint.InstallationFee, 0 ) * IFNULL( t1.InSharedInstallFeePercent, 0 )/ 100 
		ELSE 0 
	END AS SPointSharingInstallationAmount,
	
	t1.OutSharedInstallFeePercent AS EPointSharedInstallFeePercent,
	ePoint.InstallationFee AS EPointInstallationFee,
	CASE WHEN t5.IsFirstVoucherOfContract THEN IFNULL( ePoint.InstallationFee, 0 ) * IFNULL( t1.OutSharedInstallFeePercent, 0 )/ 100 
		ELSE 0 
	END AS EPointSharingInstallationAmount,
	
	t1.InSharedPackagePercent AS SPointSharedPackagePercent,
	t1.OutSharedPackagePercent AS EPointSharedPackagePercent,
	IFNULL( sPoint.MonthlyCost, 0 ) AS SPointMonthlyCost,
	IFNULL( ePoint.MonthlyCost, 0 ) AS EPointMonthlyCost,
	IFNULL( sPoint.MonthlyCost, 0 ) * IFNULL( t1.InSharedPackagePercent, 0 ) AS SPointSharingPackageAmount,
	CASE WHEN t3.HasStartAndEndPoint = 1 THEN IFNULL( ePoint.MonthlyCost, 0 ) * IFNULL( t1.OutSharedPackagePercent, 0 )/ 100 
		ELSE t5.GrandTotal * IFNULL( t1.OutSharedPackagePercent, 0 )/ 100 
	END AS EPointSharingPackageAmount,
	t1.InContractId,
	t1.OutContractId,
	t3.PackagePrice,
	t3.HasStartAndEndPoint,
	t3.StartPointChannelId,
	t3.EndPointChannelId,
	t3.ServiceName 
FROM
	ITC_FBM_Contracts.ContractSharingRevenueLines AS t1
	INNER JOIN ITC_FBM_Contracts.OutContractServicePackages AS t3 ON t1.OutServiceChannelId = t3.Id
	INNER JOIN ReceiptVoucherDetails AS t4 ON t4.OutContractServicePackageId = t1.OutServiceChannelId
	INNER JOIN ReceiptVouchers AS t5 ON t5.Id = t4.ReceiptVoucherId
	LEFT JOIN ITC_FBM_Contracts.OutputChannelPoints AS sPoint ON sPoint.Id = t3.StartPointChannelId
	LEFT JOIN ITC_FBM_Contracts.OutputChannelPoints AS ePoint ON ePoint.Id = t3.EndPointChannelId 
WHERE
	1 = 1 
	AND t1.InContractId = inContractId
	AND t1.SharingType IN ( 3, 2 ) 	
	#AND t5.IssuedDate BETWEEN startDate AND endDate
	AND t1.InSharedPackagePercent + t1.InSharedInstallFeePercent + t1.OutSharedInstallFeePercent + t1.OutSharedPackagePercent > 0 
	LIMIT 5
	;
	SELECT COUNT(t5.Id)
	FROM
	ITC_FBM_Contracts.ContractSharingRevenueLines AS t1
	INNER JOIN ITC_FBM_Contracts.OutContractServicePackages AS t3 ON t1.OutServiceChannelId = t3.Id
	INNER JOIN ReceiptVoucherDetails AS t4 ON t4.OutContractServicePackageId = t1.OutServiceChannelId
	INNER JOIN ReceiptVouchers AS t5 ON t5.Id = t4.ReceiptVoucherId
	LEFT JOIN ITC_FBM_Contracts.OutputChannelPoints AS sPoint ON sPoint.Id = t3.StartPointChannelId
	LEFT JOIN ITC_FBM_Contracts.OutputChannelPoints AS ePoint ON ePoint.Id = t3.EndPointChannelId 
WHERE
	1 = 1 
	AND t1.InContractId = inContractId
	AND t1.SharingType IN ( 3, 2 ) 	
	AND t5.IssuedDate BETWEEN startDate AND endDate
	AND t1.InSharedPackagePercent + t1.InSharedInstallFeePercent + t1.OutSharedInstallFeePercent + t1.OutSharedPackagePercent > 0 
	INTO total
	;

END