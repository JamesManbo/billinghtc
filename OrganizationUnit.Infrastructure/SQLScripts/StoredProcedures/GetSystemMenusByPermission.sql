-- =============================================
-- Author:		SonND
-- Create date: 2019/12/04
-- Description:	Lấy về danh sách các trang quản trị dựa vào quyền của người dùng
-- GetSystemMenusByPermission 1005
-- =============================================
CREATE PROCEDURE `GetSystemMenusByPermission`(
	userId INT
)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS effectivePermissions;
    DROP TEMPORARY TABLE IF EXISTS deniedPermissions;
    DROP TEMPORARY TABLE IF EXISTS permissionOfUser;
    DROP TEMPORARY TABLE IF EXISTS hasPermissionMenus;
    
	CREATE TEMPORARY TABLE permissionOfUser(
		Id INT,
		PermissionSetId INT,
		PermissionCode VARCHAR(256),
		PermissionName VARCHAR(256),
		PermissionPage VARCHAR(256)
	);
    
	CREATE TEMPORARY TABLE effectivePermissions(
		Id INT,
		PermissionSetId INT,
		PermissionCode VARCHAR(256),
		PermissionName VARCHAR(256),
		PermissionPage VARCHAR(256)
	);
    
	CREATE TEMPORARY TABLE deniedPermissions(
		Id INT,
		PermissionSetId INT,
		PermissionCode VARCHAR(256),
		PermissionName VARCHAR(256),
		PermissionPage VARCHAR(256)
	);
    
    CREATE TEMPORARY TABLE hasPermissionMenus(
		Id INT,
		ParentId INT,
		MenuIcon VARCHAR(256),
		MenuPath VARCHAR(256),
		MenuTitle VARCHAR(256),
		MenuCode VARCHAR(256),
		DisplayOrder INT
	);

	-- Truy vấn những quyền được cho phép theo những nhóm quyền được gán cho người dùng hiện tại
    INSERT INTO effectivePermissions
    SELECT DISTINCT sp.Id, sp.PermissionSetId, sp.PermissionName, sp.PermissionCode, sp.PermissionPage
	FROM SystemUsers su
	INNER JOIN SystemRoleUserAssignments ur ON ur.SystemUserId = su.Id
	INNER JOIN SystemRoles sr ON sr.Id = ur.SystemRoleId
	INNER JOIN SystemRoleSystemPermissions rp ON rp.SystemRoleId = sr.Id
	INNER JOIN SystemPermissions sp ON sp.Id = rp.SystemPermissionId
	WHERE su.Id = userId
		AND sr.IsDeleted = FALSE 
        AND sr.IsActive = TRUE 
        AND rp.`Grant` = TRUE
		AND ur.IsDeleted = FALSE 
        AND rp.IsDeleted = FALSE;

	-- Truy vấn những quyền bị chặn
    INSERT INTO deniedPermissions
    SELECT DISTINCT sp.Id, sp.PermissionSetId, sp.PermissionName, sp.PermissionCode, sp.PermissionPage
	FROM SystemUsers su
	INNER JOIN SystemRoleUserAssignments ur ON ur.SystemUserId = su.Id
	INNER JOIN SystemRoles sr ON sr.Id = ur.SystemRoleId
	INNER JOIN SystemRoleSystemPermissions rp ON rp.SystemRoleId = sr.Id
	INNER JOIN SystemPermissions sp ON sp.Id = rp.SystemPermissionId
	WHERE su.Id = userId
		AND sr.IsDeleted = FALSE
        AND sr.IsActive = TRUE
        AND rp.`Deny` = TRUE
		AND ur.IsDeleted = FALSE
        AND rp.IsDeleted = FALSE;
	
	-- Loại bỏ những quyền bị chặn khỏi danh sách quyền được truy cập của người dùng		
	INSERT INTO permissionOfUser
	SELECT ep.Id, ep.PermissionSetId, ep.PermissionName, ep.PermissionCode, ep.PermissionPage
	FROM effectivePermissions ep
	WHERE ep.Id NOT IN (SELECT Id FROM deniedPermissions);

	INSERT INTO hasPermissionMenus
	SELECT DISTINCT
		sm.Id,
		sm.ParentId,
		sm.MenuIcon,
		sm.MenuPath,
		sm.MenuTitle,
		sm.MenuCode,
		sm.DisplayOrder
	FROM SystemMenus sm
	INNER JOIN permissionOfUser pm ON pm.PermissionSetId = sm.Id
	WHERE sm.IsDeleted = FALSE;

	WITH RECURSIVE CTE (Id, ParentId, MenuIcon, MenuPath, MenuTitle, MenuCode, DisplayOrder)
	AS (
		SELECT
			hsm.Id,
			hsm.ParentId,
			hsm.MenuIcon,
			hsm.MenuPath,
			hsm.MenuTitle,
			hsm.MenuCode,
			hsm.DisplayOrder
		FROM hasPermissionMenus hsm
		UNION ALL
		SELECT
			sm.Id,
			sm.ParentId,
			sm.MenuIcon,
			sm.MenuPath,
			sm.MenuTitle,
			sm.MenuCode,
			sm.DisplayOrder
		FROM CTE c
		INNER JOIN SystemMenus sm ON c.ParentId = sm.Id
		WHERE sm.IsDeleted = FALSE
	)
	SELECT DISTINCT * FROM CTE;
END