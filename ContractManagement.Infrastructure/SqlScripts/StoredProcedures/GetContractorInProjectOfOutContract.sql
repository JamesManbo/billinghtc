CREATE PROCEDURE `GetContractorInProjectOfOutContract`(
IN skips INT, 
IN take INT, 
IN startDate VARCHAR(50),
IN endDate VARCHAR(50),
IN projectIds VARCHAR(50),
IN packageIds VARCHAR(50)
)
BEGIN
		CREATE TEMPORARY TABLE temp
		SELECT
		t1.ProjectId,
		t1.ProjectName,
		SUM( IF(t1.ContractStatusId = 5,1,0)) AS TotalCustomerQuit ,
		COUNT( DISTINCT t1.Id) AS TotalCustomer 
	FROM
		OutContracts AS t1
		INNER JOIN outcontractservicepackages AS csp ON t1.Id = csp.OutContractId 
	WHERE
		t1.IsDeleted = FALSE  
		AND t1.TimeLine_Signed > startDate 
		AND t1.TimeLine_Signed < endDate
		AND IF(projectIds <> '' , FIND_IN_SET(t1.ProjectId, projectIds), 1  = 1 )
    AND IF(packageIds <> '' , FIND_IN_SET(csp.ServicePackageId, packageIds), 1 = 1 )
          
	GROUP BY
		t1.ProjectId,
		t1.ProjectName;
		SELECT *, FOUND_ROWS() AS Total  FROM temp LIMIT take OFFSET skips ;
END