CREATE PROCEDURE `sp_GetReportServiceOutDebtData`( 	
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
	#Routine body goes here...
 		DROP TEMPORARY TABLE
 		IF EXISTS reportTb;
 		CREATE TEMPORARY TABLE reportTb
			SELECT 
			
			t1.Id,
			t1.CreatedDate,
				t3.FullName AS ContractorFullName,
				t4.`Name` AS CategoryName,
				
				t1.ContractCode,
				t1.MarketAreaName,
				t1.ProjectName,
				t7.NumberBillingLimitDays,
				t6.`Name` AS IndustryName,	
				
				t1.InvoiceCode,
				t1.InvoiceDate,
				IFNULL(t1.OpeningDebtAmount,0) * t1.ExchangeRate as OpeningDebtAmount,
				
				IFNULL(t1.GrandTotalIncludeDebt,0) * t1.ExchangeRate AS GrandTotalIncludeDebt,
				t1.GrandTotal,	
				IFNULL(t1.PaidTotal,0) * t1.ExchangeRate AS PaidTotal,
				t1.PaymentDate,
				t1.Content,
				t1.CurrencyUnitId,
				t1.CurrencyUnitCode

			FROM ReceiptVouchers as t1 
			INNER JOIN VoucherTargets AS t2 ON t2.Id = t1.TargetId
			INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t3 ON t3.IdentityGuid = t2.ApplicationUserIdentityGuid
			LEFT JOIN ITC_FBM_CRM.CustomerCategories AS t4 ON t4.Id = t3.CustomerCategoryId
			LEFT JOIN ITC_FBM_CRM.ApplicationUserIndustries AS t5 ON t5.UserId = t3.Id
			LEFT JOIN ITC_FBM_CRM.Industries AS t6 ON t6.Id = t5.IndustryId
			INNER JOIN ITC_FBM_Contracts.OutContracts AS t7 ON t7.Id = t1.OutContractId
			INNER JOIN ReceiptVoucherDetails AS t8 ON t8.ReceiptVoucherId = t1.Id
			
			WHERE 1=1
				AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
				AND (projectId = 0 OR t1.ProjectId = projectId )
				AND (serviceId = 0 OR t8.ServiceId = serviceId )
				
				AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
				AND (agentId = '' OR t7.AgentId LIKE CONCAT('%',agentId,'%') )
				AND (
						(	 `status`  = 0 ) 
					OR ( `status`  = 1 AND t1.InvoiceDate is NULL)
					OR ( `status`  = 2 AND t1.StatusId NOT IN (8,9))
					OR ( `status`  = 3 AND t1.StatusId = 9)
					OR ( `status`  = 4 AND t1.StatusId = 8)
				) 
				LIMIT take OFFSET skip 
				;
			
			set @sqlQuery=CONCAT('
				SELECT * 
				FROM reportTb 
				',orderBy,' 
				LIMIT ',take,' OFFSET ',skip,'  ;' );
				
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		
		SELECT COUNT(*)
		FROM ReceiptVouchers as t1 
		INNER JOIN VoucherTargets AS t2 ON t2.Id = t1.TargetId
		INNER JOIN ITC_FBM_CRM.ApplicationUsers AS t3 ON t3.IdentityGuid = t2.ApplicationUserIdentityGuid
		LEFT JOIN ITC_FBM_CRM.CustomerCategories AS t4 ON t4.Id = t3.CustomerCategoryId
		LEFT JOIN ITC_FBM_CRM.ApplicationUserIndustries AS t5 ON t5.UserId = t3.Id
		LEFT JOIN ITC_FBM_CRM.Industries AS t6 ON t6.Id = t5.IndustryId
		INNER JOIN ITC_FBM_Contracts.OutContracts AS t7 ON t7.Id = t1.OutContractId	
		INNER JOIN ReceiptVoucherDetails AS t8 ON t8.ReceiptVoucherId = t1.Id
		WHERE 1=1
			AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
				AND (projectId = 0 OR t1.ProjectId = projectId )
				AND (serviceId = 0 OR t8.ServiceId = serviceId )
				
				AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
				AND (agentId = '' OR t7.AgentId LIKE CONCAT('%',agentId,'%') )
				AND (
						(	 `status`  = 0 ) 
					OR ( `status`  = 1 AND t1.InvoiceDate is NULL)
					OR ( `status`  = 2 AND t1.StatusId NOT IN (8,9))
					OR ( `status`  = 3 AND t1.StatusId = 9)
					OR ( `status`  = 4 AND t1.StatusId = 8)
				) 
		INTO total;
		DEALLOCATE PREPARE stmt;
END