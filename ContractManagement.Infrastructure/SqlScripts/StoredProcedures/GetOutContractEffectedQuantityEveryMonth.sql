CREATE PROCEDURE `GetOutContractEffectedQuantityEveryMonth`()
BEGIN
DROP TEMPORARY TABLE IF EXISTS months,quantity,cte;
CREATE TEMPORARY TABLE months 
	SELECT 1 AS Thang UNION ALL
	SELECT 2 AS Thang UNION ALL
	SELECT 3 AS Thang UNION ALL
	SELECT 4 AS Thang UNION ALL
	SELECT 5 AS Thang UNION ALL
	SELECT 6 AS Thang UNION ALL
	SELECT 7 AS Thang UNION ALL
	SELECT 8 AS Thang UNION ALL
	SELECT 9 AS Thang UNION ALL
	SELECT 10 AS Thang UNION ALL
	SELECT 11 AS Thang UNION ALL
	SELECT 12 AS Thang;
	
CREATE TEMPORARY TABLE cte
	SELECT MONTH(TimeLine_Effective) AS Thang,COUNT(DISTINCT OutContractId) AS Quantity
	FROM (
		SELECT  OutContractId,TimeLine_Effective 
		FROM OutContractServicePackages
		WHERE TimeLine_Effective IS NOT NULL
			AND YEAR(TimeLine_Effective) = YEAR(NOW())
	) AS csp
	GROUP BY MONTH(TimeLine_Effective) 
;

SELECT * FROM
		(SELECT months.Thang,
			IFNULL(q.Quantity,0) AS Quantity
		FROM 
			months
		LEFT JOIN cte AS q ON months.Thang = q.Thang 
		ORDER BY
			months.Thang 
		) AS tb1 
	UNION ALL
		 		 SELECT 'Total' AS Thang,COUNT(DISTINCT OutContractId) AS Quantity FROM OutContractServicePackages AS csp 
		 WHERE TimeLine_Effective IS NOT NULL
				AND YEAR(TimeLine_Effective) = YEAR(NOW())
;
END