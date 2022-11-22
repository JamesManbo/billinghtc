CREATE  PROCEDURE `sp_DeleteServicePackage`(in id MEDIUMTEXT,in serviceid int)
BEGIN

	set @input = id;
	SET @deter = ',';
	SET @subString = '';
	SET @inde = 1;
	DROP TABLE IF EXISTS tb;
	CREATE TEMPORARY TABLE tb(item VARCHAR(500));
	WHILE (@inde > 0) DO	
		SET @inde = LOCATE(@deter,@input);
		SET @subString = SUBSTRING(@input,1,@inde-1);
		set @input = SUBSTRING(@input,@inde + 1,LENGTH(@input));
		IF(LENGTH(@subString) > 0) THEN
		INSERT into tb VALUES(@subString);
	END IF;
	
END WHILE;

	INSERT INTO tb VALUES(@input);
--	select * from tb;
 UPDATE ServicePackagePrice as spp SET spp.IsActive = FALSE WHERE spp.ServicePackageId = serviceid AND spp.Id NOT IN (SELECT * FROM tb);

END