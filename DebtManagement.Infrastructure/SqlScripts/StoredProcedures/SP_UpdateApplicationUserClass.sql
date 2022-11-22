CREATE  PROCEDURE `UpdateApplicationUserClass`(
IN userIdentityGuid NVARCHAR(50),
IN outContractId INT
)
BEGIN
	DECLARE classId INT ;
    DECLARE total DECIMAL;
    DECLARE maxConditionStartPoint DECIMAL;
    DECLARE maxClassId INT ;
	-- lấy ra tổng giá trị hợp đồng đầu ra mà khách hàng đã ký hoặc nghiệm thu
	SELECT SUM(rv.PaidTotal) INTO total 
    FROM ITC_FBM_Debts.ReceiptVouchers rv 
    WHERE rv.IsDeleted = 0
    AND rv.TargetId = userIdentityGuid;
    
    -- lấy ra giá trị hạng lớn nhất
    SELECT MAX(a.ConditionStartPoint) INTO maxConditionStartPoint FROM ITC_FBM_CRM.ApplicationUserClasses a 
    WHERE a.IsDeleted = FALSE;
    
     SELECT a.Id INTO maxClassId FROM ITC_FBM_CRM.ApplicationUserClasses a 
    WHERE a.IsDeleted = FALSE AND a.ConditionStartPoint = maxConditionStartPoint;
    
    -- lấy ra hạng khách hạng theo tổng giá trị hợp đồng
	SELECT a.Id INTO classId FROM ITC_FBM_CRM.ApplicationUserClasses a 
    WHERE a.IsDeleted = FALSE 
    AND total  between a.ConditionStartPoint AND a.ConditionEndPoint;
    -- cập nhật hạng khách hàng
    SET SQL_SAFE_UPDATES = 0;
    IF total >= maxConditionStartPoint THEN
		UPDATE ITC_FBM_CRM.ApplicationUsers au SET au.ClassId = maxClassId WHERE au.IsDeleted = FALSE AND au.IdentityGuid = userIdentityGuid;
    ELSE
		UPDATE ITC_FBM_CRM.ApplicationUsers au SET au.ClassId = classId WHERE au.IsDeleted = FALSE AND au.IdentityGuid = userIdentityGuid;
    END IF;
    SET SQL_SAFE_UPDATES = 1;
END