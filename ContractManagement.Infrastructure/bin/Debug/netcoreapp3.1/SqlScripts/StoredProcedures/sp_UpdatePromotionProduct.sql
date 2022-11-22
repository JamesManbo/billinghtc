CREATE   PROCEDURE `sp_UpdatePromotionProduct`(in OldId int,in NewId int)
BEGIN
	SET SQL_SAFE_UPDATES = 0;
	update PromotionDetails set IsActive = false where Id = OldId;
	update PromotionProducts set PromotionDetailId = NewId where PromotionDetailId = OldId;
END