CREATE PROCEDURE `sp_GetReportServiceDebtData`( 	
	IN `marketAreaId` INT, 	
	IN `contractCode` VARCHAR(100),
	IN `customerCode` VARCHAR(100),
	IN `startDate` DATE, 
	IN `endDate` DATE, 
	IN `serviceId` INT, 
	IN `projectId` INT, 
	
	IN `agentId` VARCHAR(50),
	IN `isEnterprise` TINYINT, 
	IN `status` INT, 	
	IN `orderBy` VARCHAR(100),
	
	IN `skip` INT,
	IN `take` INT,
	OUT `total` INT
)
BEGIN
				DROP TEMPORARY TABLE
		IF EXISTS ContractValue,Contractor;
		
		CREATE TEMPORARY TABLE Contractor SELECT DISTINCT
			ict.Id,
			ict.ContractCode,
			ict.MarketAreaName,
			type.`Name`,
			ctor.ContractorFullName 
			FROM
				ITC_FBM_Contracts.InContracts AS ict
				INNER JOIN ITC_FBM_Contracts.Contractors AS ctor ON ict.ContractorId = ctor.Id
				INNER JOIN ITC_FBM_Contracts.InContractTypes AS type ON type.Id = ict.ContractTypeId
				INNER JOIN ITC_FBM_Contracts.InContractServices AS t2 ON t2.InContractId = ict.Id
				WHERE 1 = 1
				AND DATE(ict.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR ict.MarketAreaId = marketAreaId )
 				AND (contractCode = '' OR ict.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR ctor.ContractorCode LIKE CONCAT('%',customerCode,'%') )				
				AND (projectId = 0 OR ict.ProjectId = projectId )
				AND (serviceId = 0 OR t2.ServiceId = serviceId )
					
				AND (isEnterprise = 2 OR ctor.IsEnterprise = isEnterprise)
				AND (agentId = '' OR ict.AgentId LIKE CONCAT('%',agentId,'%') )
			;
				
		CREATE TEMPORARY TABLE ContractValue 
			SELECT
				IFNULL( sc.InContractId, 0 ) AS InContractId,
				IFNULL(csrl.InServiceChannelId,0) AS ServiceId ,
				sc.GrandTotal - SUM( IFNULL( csrl.InSharedFixedAmount, 0 ) ) AS ContractValue ,
				#sc.CurrencyUnitId,
				sc.CurrencyUnitCode
			FROM ITC_FBM_Contracts.OutContractServicePackages AS sc
			INNER JOIN ITC_FBM_Contracts.Contractors AS t1 ON t1.Id = sc.InContractId			
			LEFT JOIN ITC_FBM_Contracts.ContractSharingRevenueLines AS csrl ON csrl.InContractId = t1.Id 
			WHERE
					IFNULL( csrl.SharingType, 1 ) = 1 
			GROUP BY
				sc.IncontractId 
		UNION ALL
			SELECT
				csrl.InContractId,
				IFNULL(csrl.InServiceChannelId,0) AS ServiceId ,
				SUM( csrl.InSharedPackagePercent * csp.PackagePrice * csp.TimeLine_PaymentPeriod / 100 ) AS ContractValue ,
				#csrl.CurrencyUnitId,
				csrl.CurrencyUnitCode
			FROM				
				ITC_FBM_Contracts.ContractSharingRevenueLines AS csrl 
				INNER JOIN ITC_FBM_Contracts.OutContractServicePackages AS csp ON csp.Id = csrl.InServiceChannelId
			WHERE
				csrl.SharingType IN ( 2, 3 ) 
			GROUP BY
				csrl.InContractId 
		UNION ALL
			SELECT
				csrl.InContractId,
				IFNULL(csrl.InServiceChannelId,0) AS ServiceId ,
				SUM( csrl.InSharedFixedAmount ) AS ContractValue ,
				#csrl.CurrencyUnitId,
				csrl.CurrencyUnitCode
			FROM
				ITC_FBM_Contracts.ContractSharingRevenueLines AS csrl 
				INNER JOIN ITC_FBM_Contracts.OutContractServicePackages AS csp ON csp.Id = csrl.Out 
			WHERE
				csrl.SharingType = 4 
			GROUP BY
				csrl.InContractId;				
		
					
SET @sqlQuery = CONCAT( '				
			SELECT
				ict.InContractId AS Id,
				ict.InContractId,
				#ict.CurrencyUnitId,
				ict.CurrencyUnitCode,
				ict.ContractValue,
				ctor.MarketAreaName,
				ctor.ContractCode,
				ctor.ContractorFullName,
				pv.PaymentPeriod,
				pv.InvoiceCode,
				pv.InvoiceDate,
				pv.InvoiceReceivedDate,
				pv.OpeningDebtAmount,
				pv.GrandTotal AS GrandTotal,
				pv.GrandTotalIncludeDebt,
				pv.PaidTotal,
				pv.PaymentDate,
				pv.GrandTotalIncludeDebt - pv.PaidTotal AS TongLuyKeThang,
				pv.Content 
			FROM
				ContractValue AS ict
				INNER JOIN Contractor AS ctor ON ctor.Id = ict.InContractId
				LEFT JOIN PaymentVouchers AS pv ON ict.InContractId = pv.InContractId 	
			WHERE (',`status`,' = 0 OR pv.StatusId = ',`status`,')			
			GROUP BY ict.InContractId 
			 ',orderBy,'	
			LIMIT ',take,' OFFSET ',skip,';' 
		);
		
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		
		SELECT COUNT(*)
		FROM ContractValue AS ict
			INNER JOIN Contractor AS ctor ON ctor.Id = ict.InContractId
			LEFT JOIN PaymentVouchers AS pv ON ict.InContractId = pv.InContractId 	
		WHERE (`status` = 0 OR pv.StatusId = `status`)			
		
		INTO total;
		 DEALLOCATE PREPARE stmt;
END