CREATE PROCEDURE `sp_GetReportServiceDebtData`( 	
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
 	IN `contractType` INT,
	IN `orderBy` VARCHAR(100),
	
	IN `skip` INT,
	IN `take` INT,
	OUT `total` INT
)
BEGIN
		SELECT DISTINCT t1.InContractId,
			t1.TargetId,
			t1.ProjectId,
			t1.InvoiceCode,
			t1.InvoiceDate,
			t1.InvoiceReceivedDate,
			rvDebt.debt * t1.ExchangeRate as OpeningDebtAmount,
			t1.PaidTotal,
			t1.GrandTotal,
			(IFNULL(rvDebt.debt,0) + t1.GrandTotal) * t1.ExchangeRate AS GrandTotalIncludeDebt,
			t1.PaymentDate,
			t1.PaymentPeriod,
			t1.TypeId,
			t1.StatusId,
			t4.`Name` AS StatusName,
			CASE when t1.TypeId = 1 THEN 'Thuê kênh truyền'
			 when t1.TypeId = 2 THEN 'Phân chia hoa hồng'
			  when t1.TypeId = 3 THEN 'Phân chia doanh thu'
				 when t1.TypeId = 4 THEN 'Bảo trì & bảo dưỡng'
			end AS ContractType,
			t2.TargetFullName AS ContractorFullName,
			t3.IndustryNames,
			t3.CategoryName,
			IFNULL(t1.ProjectName,'') AS ProjectName,
			t1.ContractCode,
			t1.MarketAreaName,
			t1.Content,
			IFNULL(t8.ServiceName,'') AS ServiceName,
			IFNULL(t1.ClearingTotal,0) AS ClearingTotal,
			t5.ClearingDate,
            t7.SignedUserName AS SignedUserName,
            t7.OrganizationUnitName AS OrganizationUnitName
		FROM PaymentVouchers AS t1

		INNER JOIN VoucherTargets AS t2 ON t1.TargetId = t2.Id
		LEFT JOIN VoucherTargetProperties AS t3 ON t2.Id = t3.TargetId
		INNER JOIN PaymentVoucherStatuses AS t4 ON t1.StatusId = t4.Id
		LEFT JOIN Clearings AS t5 ON t5.Id = t1.ClearingId
		INNER JOIN ITC_FBM_Contracts.InContracts AS t7 ON t7.Id = t1.InContractId
		INNER JOIN PaymentVoucherDetails AS t8 ON t8.PaymentVoucherId = t1.Id
		LEFT JOIN (
				SELECT
					InContractId,
					@debt := SUM( IFNULL( RemainingTotal, 0 ) ) AS debt 
				FROM
					PaymentVouchers AS rv 
				WHERE
					 ( rv.StatusId NOT IN ( 4, 5 ) OR ( rv.StatusId = 6 AND Payment_Method = 1 ) ) #đã thu và đã hủy + thu hộ nhưng là tiền mặt\
					
				GROUP BY
					InContractId 
				) AS rvDebt ON t1.InContractId = rvDebt.InContractId 
		WHERE 1=1
				AND t1.StatusId <> 5
				AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
                AND (contractorFullName = '' OR t2.TargetFullName LIKE CONCAT('%',contractorFullName,'%') )
                AND (contractType = 0 OR t1.TypeId = contractType )
				AND (projectId = 0 OR t1.ProjectId = projectId )
				AND (serviceId = 0 OR t8.ServiceId = serviceId )
				
				AND (isEnterprise = 2 OR t2.IsEnterprise = isEnterprise)
				AND (agentId = '' OR t7.AgentId LIKE CONCAT('%',agentId,'%') )
				AND (
						(	 `status`  = 0 ) 
					OR ( `status`  = 1 AND IFNULL(t1.InvoiceCode,'') = '')
					OR ( `status`  = 2 AND t1.StatusId NOT IN (8,9))
					OR ( `status`  = 3 AND t1.StatusId = 9)
					OR ( `status`  = 4 AND t1.StatusId = 8)
				) 
				LIMIT take OFFSET skip 

		;
	
		
		SELECT COUNT(*)
		FROM PaymentVouchers AS t1

		INNER JOIN VoucherTargets AS t2 ON t1.TargetId = t2.Id
		LEFT JOIN VoucherTargetProperties AS t3 ON t2.Id = t3.TargetId
		INNER JOIN PaymentVoucherStatuses AS t4 ON t1.StatusId = t4.Id
		LEFT JOIN Clearings AS t5 ON t5.Id = t1.ClearingId
		INNER JOIN ITC_FBM_Contracts.InContracts AS t7 ON t7.Id = t1.InContractId
		INNER JOIN PaymentVoucherDetails AS t8 ON t8.PaymentVoucherId  = t1.Id
		LEFT JOIN (
				SELECT
					InContractId,
					@debt := SUM( IFNULL( RemainingTotal, 0 ) ) AS debt 
				FROM
					PaymentVouchers AS rv 
				WHERE
					 ( rv.StatusId NOT IN ( 4, 5 ) OR ( rv.StatusId = 6 AND Payment_Method = 1 ) ) #đã thu và đã hủy + thu hộ nhưng là tiền mặt\
					
				GROUP BY
					InContractId 
				) AS rvDebt ON t1.InContractId = rvDebt.InContractId 
		WHERE 1=1
				AND t1.StatusId <> 5
				AND DATE(t7.TimeLine_Signed) BETWEEN startDate AND endDate
				AND (marketAreaId = 0 OR t1.MarketAreaId = marketAreaId )				
 				AND (contractCode = '' OR t1.ContractCode LIKE CONCAT('%',contractCode,'%') )
 				AND (customerCode = '' OR t2.TargetCode LIKE CONCAT('%',customerCode,'%') )
                AND (contractorFullName = '' OR t2.TargetFullName LIKE CONCAT('%',contractorFullName,'%') )
                AND (contractType = 0 OR t1.TypeId = contractType )
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
END