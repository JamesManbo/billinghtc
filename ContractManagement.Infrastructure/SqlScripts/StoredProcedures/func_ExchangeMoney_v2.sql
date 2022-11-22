CREATE FUNCTION `ExchangeMoney`(
	money DECIMAL(18,5),
	pCurrencyCode VARCHAR(10)
) RETURNS decimal(18,5)
BEGIN
	#Routine body goes here...
	DECLARE rate DECIMAL;
		SET rate := (SELECT TransferValue FROM CurExchangeRates WHERE CurExchangeRates.CurrencyCode = pCurrencyCode);
		
	RETURN money * CASE pCurrencyCode
		WHEN 'VND' THEN
			1
		ELSE
			rate
	END ;

END