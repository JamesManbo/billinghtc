CREATE  PROCEDURE `GetRevenueAndTaxAmountInYear`()
BEGIN
	DROP TEMPORARY TABLE
	IF
		EXISTS months;
	CREATE TEMPORARY TABLE months SELECT
	1 AS Thang UNION ALL
	SELECT
		2 AS Thang UNION ALL
	SELECT
		3 AS Thang UNION ALL
	SELECT
		4 AS Thang UNION ALL
	SELECT
		5 AS Thang UNION ALL
	SELECT
		6 AS Thang UNION ALL
	SELECT
		7 AS Thang UNION ALL
	SELECT
		8 AS Thang UNION ALL
	SELECT
		9 AS Thang UNION ALL
	SELECT
		10 AS Thang UNION ALL
	SELECT
		11 AS Thang UNION ALL
	SELECT
		12 AS Thang;
	SELECT
		* 
	FROM
		(
		SELECT
			months.Thang,
			SUM(
			IFNULL( rv.PaidTotal, 0 )) AS PaidTotal,
			SUM(
			IFNULL( rv.TaxAmount, 0 )) AS TaxAmount 
		FROM
			months
			LEFT JOIN ReceiptVouchers AS rv ON months.Thang = MONTH ( rv.PaymentDate ) 
		GROUP BY
			months.Thang 
		ORDER BY
			months.Thang 
		) AS tb1 
		UNION ALL
		SELECT
			'Total' AS `Thang`,
			SUM( PaidTotal ) AS PaidTotal,
			SUM( TaxAmount ) AS TaxAmount 
		FROM
			ReceiptVouchers AS rv 
		WHERE
			IsActive = 1 
			AND IsDeleted = 0 
			AND PaymentDate IS NOT NULL 
			AND YEAR ( PaymentDate ) = YEAR ( NOW()) ;
END