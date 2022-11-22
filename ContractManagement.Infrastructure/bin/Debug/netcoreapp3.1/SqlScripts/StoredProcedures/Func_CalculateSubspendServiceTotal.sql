CREATE FUNCTION `CalculateSubspendServiceTotal`(
suspensionStartDate DATE,
suspensionEndDate DATE,
packagePrice DECIMAL(65,30)
) RETURNS decimal(65,30)
    DETERMINISTIC
BEGIN
DECLARE discountAmount DECIMAL(65,30) DEFAULT 0;
DECLARE startDay INT DEFAULT DAY(suspensionStartDate);
DECLARE startMonth INT DEFAULT MONTH(suspensionStartDate);
DECLARE startYear INT DEFAULT YEAR(suspensionStartDate);
DECLARE endYear INT DEFAULT YEAR(suspensionEndDate);
DECLARE yearDiff INT DEFAULT endYear - startYear;
DECLARE endDay INT DEFAULT DAY(suspensionEndDate);
DECLARE endMonth INT DEFAULT MONTH(suspensionEndDate) + yearDiff * 12;
DECLARE daysOfMonth,usedDays,x,yearIdx INT;

IF(startMonth = endMonth) THEN
			SET daysOfMonth = DAY(LAST_DAY(CONCAT(startYear,'-',startMonth,'-',0)));
			SET usedDays = endDay - startDay;
			SET discountAmount = (packagePrice / daysOfMonth) * usedDays;
ELSE
SET x = startMonth;
	loop_month:  LOOP
		IF (x > endMonth) THEN 
			LEAVE  loop_month;
		END  IF;
        
		SET yearIdx = FLOOR(x / 12);
		SET daysOfMonth = DAY(LAST_DAY(CONCAT(startYear - yearIdx,'-',x - 12 * yearIdx,'-',0)));
        
		IF  (x = startMonth) THEN
			SET discountAmount = discountAmount + (packagePrice / daysOfMonth) * (daysOfMonth - startDay);
		ELSEIF (x = endMonth) THEN
			SET discountAmount = discountAmount + (packagePrice / daysOfMonth) * endDay;
		ELSEIF (x > startMonth AND x < endMonth) THEN
			SET discountAmount = discountAmount + packagePrice;        
		END  IF;
            
		SET  x = x + 1;
	END LOOP;
END IF;

RETURN discountAmount;
END