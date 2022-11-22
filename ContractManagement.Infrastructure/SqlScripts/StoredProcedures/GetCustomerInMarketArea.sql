CREATE PROCEDURE `GetCustomerInMarketArea`( IN startDate VARCHAR(50),IN endDate VARCHAR(50) )
BEGIN
	#Routine body goes here...
SELECT
	SUM( IF ( t2.MarketAreaId = 1, 1, 0 ) ) AS TotalNorth,
	SUM( IF ( t2.MarketAreaId = 2, 1, 0 ) ) AS TotalCenter,
	SUM( IF ( t2.MarketAreaId = 3, 1, 0 ) ) AS TotalSouth,
	COUNT( DISTINCT t2.ContractorId ) AS Total 
FROM
	(
	SELECT
		t1.MarketAreaId,
		t1.ContractorId 
	FROM
		`OutContracts` AS t1
		INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId 
	WHERE
		t1.`IsDeleted` = FALSE 
		AND t2.IsDeleted = FALSE 
		AND ( t2.ServiceId != 1 OR IFNULL( t1.ProjectId, 0 ) = 0 ) 
		AND t1.IsDeleted = FALSE 
		AND t1.TimeLine_Signed >= startDate
		AND t1.TimeLine_Signed < endDate
	GROUP BY
		t1.ContractorId,
		t1.MarketAreaId 
	) AS t2;
END