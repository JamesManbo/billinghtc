CREATE PROCEDURE `SynchronizeVoucherTargetProperties`(`customerGuid` varchar(68), `voucherTargetId` int)
BEGIN
	
	IF EXISTS (SELECT 1 FROM VoucherTargetProperties WHERE TargetId = voucherTargetId)
	THEN
		DELETE FROM VoucherTargetProperties WHERE TargetId = voucherTargetId;
	END IF;
	
	INSERT INTO VoucherTargetProperties (
			TargetId
			,StructureId
			,CategoryId
			,GroupIds
			,ClassId
			,TypeId
			,IndustryIds
			,ApplicationUserIdentityGuid
			,StructureName
			,CategoryName
			,GroupNames
			,ClassName
			,TypeName
			,IndustryNames
			,IsActive
			,IsDeleted
			,DisplayOrder
			,CreatedDate
			,CreatedBy
		)
		SELECT
			voucherTargetId,
			cp.ContractorStructureId,
			cp.ContractorCategoryId,
			cp.ContractorGroupIds,
			cp.ContractorClassId,
			cp.ContractorTypeId,
			cp.ContractorIndustryIds,
			cp.ApplicationUserIdentityGuid,
			cp.ContractorStructureName,
			cp.ContractorCategoryName,
			cp.ContractorGroupNames,
			cp.ContractorClassName,
			cp.ContractorTypeName,
			cp.ContractorIndustryNames,
			TRUE,
			FALSE,
			0,
			CURDATE(),
			"SYSTEM"
		FROM ITC_FBM_Contracts.ContractorProperties AS cp WHERE cp.ApplicationUserIdentityGuid = customerGuid;

END