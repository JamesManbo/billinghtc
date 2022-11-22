CREATE  PROCEDURE `sp_GetDataTotalRevenue`()
BEGIN
	SELECT N'Châu Kiệt' AS ContractorName,
		'HD-20210621-MB-VID' as ContractCode,
		'2021-06-02' as	 TimeLineSigned,
		'2022-06-02' AS TimeLineExpiration,
		50000000 AS GrandTotal
	FROM OutContracts 
	LIMIT 1;
END