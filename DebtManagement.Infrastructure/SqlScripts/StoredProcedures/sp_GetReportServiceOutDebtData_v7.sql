CREATE PROCEDURE `sp_GetReportServiceOutDebtData`( 	
	IN `marketAreaId` INT, 	
	IN `contractCode` VARCHAR(100),
	IN `customerCode` VARCHAR(100),
    IN `contractorFullName` VARCHAR(100),
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
				t2.TargetFullName AS ContractorFullName,
				t3.ContractorCategoryName AS CategoryName,
				t3.ContractorIndustryNames AS IndustryNames,
				t10.GroupName AS ServiceName,
				
				t1.ContractCode,
				t1.MarketAreaName,
				t1.ProjectName,
				t7.NumberBillingLimitDays,
					
				
				t1.InvoiceCode,
				t1.InvoiceDate,
				t1.TargetDebtRemaningTotal * t1.ExchangeRate as OpeningDebtAmount,
				t1.GrandTotal,
				(IFNULL(t1.TargetDebtRemaningTotal,0) + t1.GrandTotal) * t1.ExchangeRate AS GrandTotalIncludeDebt,
				(t1.TargetDebtRemaningTotal + t1.GrandTotal - t1.PaidTotal) * t1.ExchangeRate AS TongLuyKeThang,
				
																				  
				   
				IFNULL(t1.PaidTotal,0) * t1.ExchangeRate AS PaidTotal,
				t1.PaymentDate,
				t1.Content,
				t1.CurrencyUnitId,
				t1.CurrencyUnitCode,
				IFNULL(t1.ClearingTotal,0) AS ClearingTotal,
				t5.ClearingDate,
				t4.`Name` AS StatusName,
				t7.TimeLine_PaymentPeriod AS PaymentPeriod,
                t7.SignedUserName AS SignedUserName,
				t7.OrganizationUnitName AS OrganizationUnitName,
                t7.CustomerCareStaffUserId AS CustomerCareStaffUserName

			FROM ReceiptVouchers as t1 
			INNER JOIN VoucherTargets AS t2 ON t2.Id = t1.TargetId
			LEFT JOIN ITC_FBM_Contracts.ContractorProperties AS t3 ON t3.ApplicationUserIdentityGuid = t2.ApplicationUserIdentityGuid
			INNER JOIN ReceiptVoucherStatuses AS t4 ON t1.StatusId = t4.Id
			LEFT JOIN Clearings AS t5 ON t5.Id = t1.ClearingId
			INNER JOIN ITC_FBM_Contracts.OutContracts AS t7 ON t7.Id = t1.OutContractId
			INNER JOIN ReceiptVoucherDetails AS t8 ON t8.ReceiptVoucherId = t1.Id
			INNER JOIN ITC_FBM_Contracts.Services AS t9 ON t9.Id = t8.ServiceId
			INNER JOIN ITC_FBM_Contracts.ServiceGroups AS t10 ON t10.Id = t9.GroupId
			
			WHERE 1=1
				AND t1.StatusId <> 5
				AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
                AND (contractorFullName = '' OR t2.TargetFullName LIKE CONCAT('%',contractorFullName,'%') )
				AND (projectId = 0 OR t1.ProjectId = projectId )
				AND (serviceId = 0 OR t8.ServiceId = serviceId )
				
				AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
				AND (agentId = '' OR t7.AgentId LIKE CONCAT('%',agentId,'%') )
				AND (
					-- paymentvoucherstatuses
						(	 `status`  = 0 ) 
					OR ( `status`  = 1 AND t1.InvoiceDate is NULL)
					OR ( `status`  = 2 AND t1.StatusId NOT IN (5,6,7))
					OR ( `status`  = 3 AND t1.StatusId = 5)
					OR ( `status`  = 4 AND t1.StatusId = 5)
				) 
				LIMIT take OFFSET skip 
				;
			
			set @sqlQuery=CONCAT('
				SELECT * 
				FROM reportTb 
				',orderBy,' ;' );
				
		PREPARE stmt FROM @sqlQuery;
    EXECUTE stmt;
		
		SELECT COUNT(*)
		FROM ReceiptVouchers as t1 
		INNER JOIN VoucherTargets AS t2 ON t2.Id = t1.TargetId
		LEFT JOIN ITC_FBM_Contracts.ContractorProperties AS t3 ON t3.ApplicationUserIdentityGuid = t2.ApplicationUserIdentityGuid
		INNER JOIN ReceiptVoucherStatuses AS t4 ON t1.StatusId = t4.Id
		LEFT JOIN Clearings AS t5 ON t5.Id = t1.ClearingId
		INNER JOIN ITC_FBM_Contracts.OutContracts AS t7 ON t7.Id = t1.OutContractId	
		INNER JOIN ReceiptVoucherDetails AS t8 ON t8.ReceiptVoucherId = t1.Id
		INNER JOIN ITC_FBM_Contracts.Services AS t9 ON t9.Id = t8.ServiceId
		INNER JOIN ITC_FBM_Contracts.ServiceGroups AS t10 ON t10.Id = t9.GroupId
		WHERE 1=1
			AND t1.StatusId <> 5
			AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
                AND (contractorFullName = '' OR t2.TargetFullName LIKE CONCAT('%',contractorFullName,'%') )
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