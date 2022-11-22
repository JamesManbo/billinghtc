CREATE PROCEDURE `UpdateNextBillingDateOfPayingContracts`()
BEGIN

	IF EXISTS (SELECT 1 FROM TemporaryPayingContracts)
	THEN
		BEGIN
			UPDATE OutContractServicePackages osp
			INNER JOIN TemporaryPayingContracts tpc ON osp.Id = tpc.ServicePackageId
			LEFT JOIN PromotionForContracts pfc 
				ON pfc.OutContractServicePackageId = osp.Id 
				AND pfc.PromotionType = 2
				AND pfc.IsApplied = TRUE
			LEFT JOIN Promotions p ON pfc.PromotionId = p.Id
			SET 
				osp.TimeLine_LatestBilling = DATE(osp.TimeLine_NextBilling),
				osp.TimeLine_NextBilling = 
				IF(osp.TimeLine_PaymentForm = 0 OR DAYOFMONTH(osp.TimeLine_NextBilling) = 1,
					DATE_ADD(osp.TimeLine_NextBilling,
					INTERVAL
						(CASE
							WHEN p.Id IS NOT NULL AND DATE(p.StartDate) <= DATE(CURDATE()) AND DATE(p.EndDate) >= DATE(CURDATE()) AND pfc.MinPaymentPeriod < osp.TimeLine_PaymentPeriod
								THEN osp.TimeLine_PaymentPeriod + pfc.Quantity
							ELSE osp.TimeLine_PaymentPeriod
							END
						)
					MONTH),					
					DATE_ADD(LAST_DAY(osp.TimeLine_NextBilling), INTERVAL 1 DAY)
				)
			WHERE osp.TimeLine_NextBilling IS NOT NULL;
		END;
	END IF;

END