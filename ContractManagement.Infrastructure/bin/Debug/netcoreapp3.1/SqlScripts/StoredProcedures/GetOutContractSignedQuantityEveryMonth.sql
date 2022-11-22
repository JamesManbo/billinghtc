CREATE PROCEDURE `GetOutContractSignedQuantityEveryMonth`()
BEGIN
	#Routine body goes here...
SELECT 
			SUM( IF ( MONTH ( TimeLine_Signed ) = 1, 1, 0 ) ) AS ValueMonth1,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 2, 1, 0 ) ) AS ValueMonth2,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 3, 1, 0 ) ) AS ValueMonth3,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 4, 1, 0 ) ) AS ValueMonth4,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 5, 1, 0 ) ) AS ValueMonth5,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 6, 1, 0 ) ) AS ValueMonth6,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 7, 1, 0 ) ) AS ValueMonth7,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 8, 1, 0 ) ) AS ValueMonth8,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 9, 1, 0 ) ) AS ValueMonth9,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 10, 1, 0 ) ) AS ValueMonth10,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 11, 1, 0 ) ) AS ValueMonth11,
			SUM( IF ( MONTH ( TimeLine_Signed ) = 12, 1, 0 ) ) AS ValueMonth12 ,
			COUNT(Id) as Total
FROM OutContracts
WHERE IsActive = 1 AND IsDeleted = 0
AND YEAR(TimeLine_Signed) = YEAR(NOW())
;

END