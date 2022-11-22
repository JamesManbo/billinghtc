CREATE PROCEDURE `GetInContractTotalQuantityEveryMonth`()
BEGIN
	#Routine body goes here...
SELECT 
			SUM( IF ( MONTH ( CreatedDate ) = 1, 1, 0 ) )  AS ValueMonth1,
			SUM( IF ( MONTH ( CreatedDate ) = 2, 1, 0 ) )  AS ValueMonth2,
			SUM( IF ( MONTH ( CreatedDate ) = 3, 1, 0 ) )  AS ValueMonth3,
			SUM( IF ( MONTH ( CreatedDate ) = 4, 1, 0 ) )  AS ValueMonth4,
			SUM( IF ( MONTH ( CreatedDate ) = 5, 1, 0 ) )  AS ValueMonth5,
			SUM( IF ( MONTH ( CreatedDate ) = 6, 1, 0 ) )  AS ValueMonth6,
			SUM( IF ( MONTH ( CreatedDate ) = 7, 1, 0 ) )  AS ValueMonth7,
			SUM( IF ( MONTH ( CreatedDate ) = 8, 1, 0 ) )  AS ValueMonth8,
			SUM( IF ( MONTH ( CreatedDate ) = 9, 1, 0 ) )  AS ValueMonth9,
			SUM( IF ( MONTH ( CreatedDate ) = 10, 1, 0 ) ) AS ValueMonth10,
			SUM( IF ( MONTH ( CreatedDate ) = 11, 1, 0 ) ) AS ValueMonth11,
			SUM( IF ( MONTH ( CreatedDate ) = 12, 1, 0 ) ) AS ValueMonth12,
			COUNT(Id) AS Total 
FROM InContracts AS oct
WHERE IsActive = 1 AND IsDeleted = 0
AND YEAR(CreatedDate) = YEAR(NOW())
;

END