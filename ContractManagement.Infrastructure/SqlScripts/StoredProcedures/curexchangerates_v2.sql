﻿CREATE VIEW `CurExchangeRates` AS select `t1`.`CurrencyCode` AS `CurrencyCode`,`t1`.`TransferValue` AS `TransferValue` from `ExchangeRates` `t1` where (cast(`t1`.`CreatedDate` as date) = cast(curdate() as date));