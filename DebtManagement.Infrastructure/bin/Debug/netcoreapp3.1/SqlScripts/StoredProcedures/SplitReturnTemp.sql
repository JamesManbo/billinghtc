CREATE PROCEDURE `SplitReturnTemp`(
IN x mediumtext
)
BEGIN
drop temporary table if exists t;
create temporary table t( txt text );
insert into t values(x);

drop temporary table if exists temp;
create temporary table temp( val text );
set @sql = concat("insert into temp (val) values ('", replace(( select group_concat(distinct txt) as data from t), ",", "'),('"),"');");
prepare stmt1 from @sql;
execute stmt1;

drop temporary table if exists t;
END