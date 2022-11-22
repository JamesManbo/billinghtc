CREATE PROCEDURE `GetPermissionOfUser`(
	IN userId INT
)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS effectivePermissions;
    DROP TEMPORARY TABLE IF EXISTS deniedPermissions;
    
    CREATE TEMPORARY TABLE effectivePermissions (
		Id INT,
		PermissionCode NVARCHAR(256),
		PermissionName NVARCHAR(256),
		PermissionPage NVARCHAR(256),
		RoleName NVARCHAR(256)
    );
    
    CREATE TEMPORARY TABLE deniedPermissions (
		Id INT,
		PermissionCode NVARCHAR(256),
		PermissionName NVARCHAR(256),
		PermissionPage NVARCHAR(256),
		RoleName NVARCHAR(256)
    );
	
	-- Truy vấn những quyền được cho phép theo những nhóm quyền được gán cho người dùng hiện tại
    INSERT INTO effectivePermissions
    SELECT DISTINCT sp.Id, sp.PermissionCode, sp.PermissionName, sp.PermissionPage, sr.RoleName
	FROM Users su
	INNER JOIN UserRoles ur ON ur.UserId = su.Id
	INNER JOIN Roles sr ON sr.Id = ur.RoleId
	INNER JOIN RolePermissions rp ON rp.RoleId = sr.Id
	INNER JOIN `Permissions` sp ON sp.Id = rp.PermissionId
	WHERE su.Id = userId
		AND sr.IsDeleted = FALSE 
        AND sr.IsActive = TRUE 
        AND rp.`Grant` = TRUE
		AND ur.IsDeleted = FALSE 
        AND rp.IsDeleted = FALSE;

	-- Truy vấn những quyền bị chặn
    INSERT INTO deniedPermissions
    SELECT DISTINCT sp.Id, sp.PermissionCode, sp.PermissionName, sp.PermissionPage, sr.RoleName
	FROM Users su
	INNER JOIN UserRoles ur ON ur.UserId = su.Id
	INNER JOIN Roles sr ON sr.Id = ur.RoleId
	INNER JOIN RolePermissions rp ON rp.RoleId = sr.Id
	INNER JOIN `Permissions` sp ON sp.Id = rp.PermissionId
	WHERE su.Id = userId
		AND sr.IsDeleted = FALSE
        AND sr.IsActive = TRUE
        AND rp.`Deny` = TRUE
		AND ur.IsDeleted = FALSE
        AND rp.IsDeleted = FALSE;
	
	-- Loại bỏ những quyền bị chặn khỏi danh sách quyền được truy cập của người dùng
	SELECT ep.Id, ep.PermissionCode, ep.PermissionName, ep.PermissionPage
	FROM effectivePermissions ep
	WHERE ep.Id NOT IN (SELECT Id FROM deniedPermissions);    
END