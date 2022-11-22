CREATE PROCEDURE `GetEquipmentInProjectOfOutContract`(
IN skips INT, 
IN take INT, 
IN startDate VARCHAR(50),
IN endDate VARCHAR(50),
IN projectIds VARCHAR(50),
IN equipmentStatus VARCHAR(50),
IN equipmentIds VARCHAR(50)
)
BEGIN
	CREATE TEMPORARY TABLE temp
	SELECT o.ProjectId, 
	o.ProjectName, 
	SUM(FIND_IN_SET(ce.StatusId, 1)) Examined,
	SUM(FIND_IN_SET(ce.StatusId, 2)) Confirmed,
    SUM(FIND_IN_SET(ce.StatusId, 3)) Deployed,
	SUM(FIND_IN_SET(ce.StatusId, 4)) HasToBeReclaim,
    SUM(FIND_IN_SET(ce.StatusId, 5)) Reclaimed,
	SUM(FIND_IN_SET(ce.StatusId, 6)) Cancelled,
    SUM(FIND_IN_SET(ce.StatusId, 7)) Terminate,
    COUNT(ce.Id) TotalEquipment FROM OutContractServicePackages octsp /*tổng số thiết bị/dự án*/
	INNER JOIN OutContracts o ON octsp.OutContractId = o.Id
	INNER JOIN ContractEquipments ce ON ce.OutContractPackageId = octsp.Id
	WHERE octsp.IsDeleted = false
	AND o.IsDeleted = false
	AND ce.IsDeleted = false
	AND IF(startDate IS NOT NULL , o.CreatedDate  >= CONVERT(startDate , DATETIME(6)), o.CreatedDate  = o.CreatedDate )
	AND IF(endDate IS NOT NULL  , o.CreatedDate  <= CONVERT(endDate , DATETIME(6)), o.CreatedDate  = o.CreatedDate ) 
    AND IF(projectIds <> '' , FIND_IN_SET(o.ProjectId, projectIds), o.ProjectId  = o.ProjectId )
    AND IF(equipmentStatus <> '' , FIND_IN_SET(ce.StatusId, equipmentStatus) , ce.StatusId  = ce.StatusId )
    AND IF(equipmentIds <> '' , FIND_IN_SET(ce.EquipmentId, equipmentIds) , ce.EquipmentId  = ce.EquipmentId )
	GROUP BY o.ProjectId,o.ProjectName;
    
    SELECT *, FOUND_ROWS() AS Total  FROM temp LIMIT take OFFSET skips ; /*Total:tổng số lượng dự án*/
END