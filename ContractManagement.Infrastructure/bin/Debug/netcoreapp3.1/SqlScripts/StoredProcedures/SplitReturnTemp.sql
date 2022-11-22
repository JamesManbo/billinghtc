
CREATE PROCEDURE `SplitReturnTemp`(
IN x varchar(1000)
)
BEGIN
drop temporary table if exists t;
create temporary table t( txt text );
insert into t values(x);

drop temporary table if exists temp;
create temporary table temp( val int );
set @sql = concat("insert into temp (val) values ('", replace(( select group_concat(distinct txt) as data from t), ",", "'),('"),"');");
prepare stmt1 from @sql;
execute stmt1;

drop table if exists t;
END