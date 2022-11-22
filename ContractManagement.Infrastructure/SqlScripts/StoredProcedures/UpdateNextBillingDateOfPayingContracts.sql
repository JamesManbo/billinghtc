CREATE PROCEDURE `UpdateNextBillingDateOfPayingContracts`()
BEGIN

	IF EXISTS (SELECT 1 FROM TemporaryPayingContracts)
	THEN
		BEGIN
			UPDATE OutContractServicePackages osp
			INNER JOIN TemporaryPayingContracts tpc ON osp.Id = tpc.ServicePackageId
			SET 
				osp.TimeLine_NextBilling = DATE_ADD(osp.TimeLine_NextBilling, INTERVAL osp.TimeLine_PaymentPeriod MONTH);
		END;
	END IF;

END