CREATE PROCEDURE `UpdateNextBillingDateOfPayingContracts`()
BEGIN

	IF EXISTS (SELECT 1 FROM TemporaryPayingContracts)
	THEN
		BEGIN
			UPDATE OutContractServicePackages osp
			INNER JOIN TemporaryPayingContracts tpc ON osp.Id = tpc.ServicePackageId
			SET 
				osp.TimeLine_LatestBilling = DATE(osp.TimeLine_NextBilling),
				osp.TimeLine_NextBilling = 
				IF(osp.TimeLine_PaymentForm = 0 OR DAYOFMONTH(osp.TimeLine_NextBilling) = 1,
					DATE_ADD(osp.TimeLine_NextBilling, INTERVAL osp.TimeLine_PaymentPeriod MONTH),					
					DATE_ADD(LAST_DAY(osp.TimeLine_NextBilling), INTERVAL 1 DAY)
				);
		END;
	END IF;

END