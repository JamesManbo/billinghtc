using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using Dapper;
using GenericRepository;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IAcceptanceQueries : IQueryRepository
    {
        IPagedList<TransactionDTO> GetList(TransactionRequestFilterModel requestFilterModel);
        TransactionDTO FindById(int id);

    }
    public class AcceptanceQueries : QueryRepository<Transaction, int>, IAcceptanceQueries
    {
        public AcceptanceQueries(ContractDbContext context) : base(context)
        {
            
        }

        public TransactionDTO FindById(int id)
        {
            var cache = new Dictionary<int, TransactionDTO>();
            var dapperExecution = BuildByTemplate<TransactionDTO>();

            //1
            dapperExecution.SqlBuilder.Select("t2.`OutContractServicePackageId`");
            dapperExecution.SqlBuilder.Select("t2.`Id`");
            dapperExecution.SqlBuilder.Select("t2.`TransactionId`");
            dapperExecution.SqlBuilder.Select("t2.`OutContractId`");
            dapperExecution.SqlBuilder.Select("t2.`ServiceId`");
            dapperExecution.SqlBuilder.Select("t2.`ServicePackageId`");
            dapperExecution.SqlBuilder.Select("t2.`ServiceName`");
            dapperExecution.SqlBuilder.Select("t2.`PackageName`");
            dapperExecution.SqlBuilder.Select("t2.`IsFreeStaticIp`");
            dapperExecution.SqlBuilder.Select("t2.`BandwidthLabel`");
            dapperExecution.SqlBuilder.Select("t2.`InternationalBandwidth`");
            dapperExecution.SqlBuilder.Select("t2.`DomesticBandwidth`");
            dapperExecution.SqlBuilder.Select("t2.`InternationalBandwidthUom`");
            dapperExecution.SqlBuilder.Select("t2.`DomesticBandwidthUom`");
            //dapperExecution.SqlBuilder.Select("t2.`Building`");
            //dapperExecution.SqlBuilder.Select("t2.`Floor`");
            dapperExecution.SqlBuilder.Select("t2.`CustomerCode`");
            dapperExecution.SqlBuilder.Select("t2.`CId`");
            dapperExecution.SqlBuilder.Select("t2.`RadiusAccount`");
            dapperExecution.SqlBuilder.Select("t2.`RadiusPassword`");
            //dapperExecution.SqlBuilder.Select("t2.`OutletChannelId`");
            //dapperExecution.SqlBuilder.Select("t2.`HasToCollectMoney`");
            dapperExecution.SqlBuilder.Select("t2.`HasStartAndEndPoint`");
            dapperExecution.SqlBuilder.Select("t2.`TaxAmount`");
            dapperExecution.SqlBuilder.Select("t2.`InstallationFee`");
            dapperExecution.SqlBuilder.Select("t2.`OtherFee`");
            dapperExecution.SqlBuilder.Select("t2.`PackagePrice`");
            dapperExecution.SqlBuilder.Select("t2.`SubTotal`");
            dapperExecution.SqlBuilder.Select("t2.`GrandTotal`");
            //dapperExecution.SqlBuilder.Select("t2.`ExaminedEquipmentAmount`");
            dapperExecution.SqlBuilder.Select("t2.`EquipmentAmount`");
            //dapperExecution.SqlBuilder.Select("t2.`GrandTotalIncludeEquipment`");
            //dapperExecution.SqlBuilder.Select("t2.`GrandTotalIncludeExaminedEquipment`");
            dapperExecution.SqlBuilder.Select("t2.`StatusId`");
            //dapperExecution.SqlBuilder.Select("t2.`SpInstallationFee`");
            //dapperExecution.SqlBuilder.Select("t2.`SpPackagePrice`");
            //dapperExecution.SqlBuilder.Select("t2.`SpSubTotal`");
            //dapperExecution.SqlBuilder.Select("t2.`SpGrandTotal`");
            //dapperExecution.SqlBuilder.Select("t2.`EpInstallationFee`");
            //dapperExecution.SqlBuilder.Select("t2.`EpPackagePrice`");
            //dapperExecution.SqlBuilder.Select("t2.`EpSubTotal`");
            //dapperExecution.SqlBuilder.Select("t2.`EpGrandTotal`");
            dapperExecution.SqlBuilder.Select("t2.`IsAcceptanced`");
            dapperExecution.SqlBuilder.Select("t2.`IsOld`");

            //2
            dapperExecution.SqlBuilder.Select("'' AS InstallationSpliter");
            //dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_Street` AS `Street`");
            //dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_City` AS `City`");
            //dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_District` AS `District`");
            //dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_EndPoint` AS `EndPoint`");
            //dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_StartPoint` AS `StartPoint`");

            //3
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_PrepayPeriod` AS `PrepayPeriod`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_Signed` AS `Signed`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_Effective` AS `Effective`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_LatestBilling` AS `LatestBilling`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_NextBilling` AS `NextBilling`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_SuspensionEndDate` AS `SuspensionEndDate`");
            dapperExecution.SqlBuilder.Select("t2.`TimeLine_SuspensionStartDate` AS `SuspensionStartDate`");

            //4
            dapperExecution.SqlBuilder.Select("t3.`Id`");
            dapperExecution.SqlBuilder.Select("t3.`TransactionServicePackageId`");
            dapperExecution.SqlBuilder.Select("t3.`ContractEquipmentId`");
            dapperExecution.SqlBuilder.Select("t3.`EquipmentId`");
            dapperExecution.SqlBuilder.Select("t3.`EquipmentName`");
            dapperExecution.SqlBuilder.Select("t3.`Manufacturer`");
            dapperExecution.SqlBuilder.Select("t3.`Specifications`");
            dapperExecution.SqlBuilder.Select("t3.`UnitPrice`");
            dapperExecution.SqlBuilder.Select("t3.`IsFree`");
            dapperExecution.SqlBuilder.Select("t3.`ExaminedUnit`");
            dapperExecution.SqlBuilder.Select("t3.`RealUnit`");
            dapperExecution.SqlBuilder.Select("t3.`ReclaimedUnit`");
            dapperExecution.SqlBuilder.Select("t3.`SerialCode`");
            dapperExecution.SqlBuilder.Select("t3.`StatusId`");
            dapperExecution.SqlBuilder.Select("t3.`HasToReclaim`");
            dapperExecution.SqlBuilder.Select("t3.`IsOld`");
            dapperExecution.SqlBuilder.Select("t3.`ServiceName`");
            dapperExecution.SqlBuilder.Select("t3.`PackageName`");
            dapperExecution.SqlBuilder.Select("t3.`SubTotal`");
            dapperExecution.SqlBuilder.Select("t3.`GrandTotal`");
            dapperExecution.SqlBuilder.Select("t3.`ExaminedSubTotal`");
            dapperExecution.SqlBuilder.Select("t3.`ExaminedGrandTotal`");

            //5
            dapperExecution.SqlBuilder.Select("af.`Id`");
            dapperExecution.SqlBuilder.Select("af.`TransactionId`");
            dapperExecution.SqlBuilder.Select("af.`InContractId`");
            dapperExecution.SqlBuilder.Select("af.`OutContractId`");
            dapperExecution.SqlBuilder.Select("af.`ResourceStorage`");
            dapperExecution.SqlBuilder.Select("af.`Name`");
            dapperExecution.SqlBuilder.Select("af.`FileName`");
            dapperExecution.SqlBuilder.Select("af.`FilePath`");
            dapperExecution.SqlBuilder.Select("af.`Size`");
            dapperExecution.SqlBuilder.Select("af.`FileType`");
            dapperExecution.SqlBuilder.Select("af.`Extension`");
            dapperExecution.SqlBuilder.Select("af.`RedirectLink`");

            dapperExecution.SqlBuilder.Select("oc.`ContractCode`");

            dapperExecution.SqlBuilder.Select("ct.`Id`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorCode`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorPhone`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorEmail`");
            dapperExecution.SqlBuilder.Select("ct.`AccountingCustomerCode`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorAddress`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorIdNo`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorTaxIdNo`");
            dapperExecution.SqlBuilder.Select("ct.`IsEnterprise`");
            dapperExecution.SqlBuilder.Select("ct.`IsBuyer`");


            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionEquipments AS t3 ON (t3.TransactionServicePackageId = t2.Id OR t3.TransactionId = t1.Id) AND t3.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON af.TransactionId = t1.Id AND af.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContracts oc ON t1.OutContractId = oc.Id AND oc.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "Contractors ct ON oc.ContractorId = ct.Id AND ct.IsDeleted = FALSE");


            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(AcceptanceDTO),
                    typeof(TransactionServicePackageDTO),
                    typeof(InstallationAddress),
                    typeof(BillingTimeLine),
                    typeof(TransactionEquipmentDTO),
                    typeof(AttachmentFileDTO),
                    typeof(string),
                    typeof(ContractorDTO),
                    
                }, results =>
                {
                    var acceptanceEntry = results[0] as AcceptanceDTO;
                    if (acceptanceEntry == null) return null;

                    if (!cache.TryGetValue(acceptanceEntry.Id, out var result))
                    {
                        result = acceptanceEntry;

                        cache.Add(acceptanceEntry.Id, acceptanceEntry);
                    }
                    var transactionServicePackage = results[1] as TransactionServicePackageDTO;

                    if (transactionServicePackage!=null) {
                        var billingTimeLine = results[3] as BillingTimeLine;
                        if (billingTimeLine != null)
                            transactionServicePackage.TimeLine = billingTimeLine;

                        var installationAddress = results[2] as InstallationAddress;
                        //TODO Outlet channel point logic changes
                        //if (installationAddress != null)
                        //transactionServicePackage.InstallationAddress = installationAddress;
                    }

                    var transactionEquipment = results[4] as TransactionEquipmentDTO;
                    if (transactionEquipment != null && result.TransactionEquipments.All(o => o.Id != transactionEquipment.Id))
                        result.TransactionEquipments.Add(transactionEquipment);


                    if (transactionServicePackage != null && result.TransactionServicePackages.All(s => s.Id != transactionServicePackage.Id))
                    {
                        result.TransactionServicePackages.Add(transactionServicePackage);
                    }

                    var transactionAttachmentFile = results[5] as AttachmentFileDTO;
                    if (transactionAttachmentFile != null && result.AttachmentFiles.All(s => s.Id != transactionAttachmentFile.Id))
                        result.AttachmentFiles.Add(transactionAttachmentFile);
                    
                    var contractor = results[7] as ContractorDTO;
                    if (contractor != null)
                        result.Contractor = contractor;

                    return result;
                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                "OutContractServicePackageId,InstallationSpliter,PaymentPeriod,Id,Id,ContractCode,Id"));
            return cache.Values.FirstOrDefault();
        }

        public IPagedList<TransactionDTO> GetList(TransactionRequestFilterModel requestFilterModel)
        {
            var cache = new Dictionary<int, TransactionDTO>();
            var dapperExecution = BuildByTemplate<TransactionDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.InnerJoin("OutContracts AS t3 ON t3.Id = t1.OutContractId AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.InnerJoin("Contractors AS t4 ON t4.Id = t3.ContractorId AND t4.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians AS t5 ON t5.ProjectId = t3.ProjectId AND t5.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.Select("t3.`ContractCode`");

            dapperExecution.SqlBuilder.Select("t4.`Id`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorCode`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorPhone`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorEmail`");
            dapperExecution.SqlBuilder.Select("t4.`AccountingCustomerCode`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorAddress`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorIdNo`");
            dapperExecution.SqlBuilder.Select("t4.`ContractorTaxIdNo`");
            dapperExecution.SqlBuilder.Select("t4.`IsEnterprise`");
            dapperExecution.SqlBuilder.Select("t4.`IsBuyer`");

            dapperExecution.SqlBuilder.Where("t1.IsTechnicalConfirmation = true");

            if (requestFilterModel
                .Any("outContractId"))
            {
                dapperExecution.SqlBuilder.Where("t1.OutContractId = @outContractId", new { outContractId = requestFilterModel.Get("outContractId") });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.StatusIds))
            {
                var statusIds = requestFilterModel.StatusIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t1.StatusId IN @statusIds)", new { statusIds });
            }
            if (requestFilterModel.FromDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.TransactionDate >= @fromDate", new { fromDate = requestFilterModel.FromDate.Value });
            }
            if (requestFilterModel.ToDate.HasValue)
            {
                var toD = requestFilterModel.ToDate.Value;
                dapperExecution.SqlBuilder.Where("t1.TransactionDate < @toDate", new { toDate = toD.AddDays(1) });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                var projectIds = requestFilterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t3.ProjectId IN @projectIds Or t3.ProjectId IS NULL)", new { projectIds });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.AcceptanceTypes))
            {
                var lstType = requestFilterModel.AcceptanceTypes.Split(",");
                dapperExecution.SqlBuilder.Where("t1.Type IN @types", new { types = lstType });
            }
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t4.ContractorFullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }
            if (!string.IsNullOrWhiteSpace(requestFilterModel.SupporterId))
            {
                dapperExecution.SqlBuilder.Where("t5.UserTechnicianId = @identityGuid", new { identityGuid = requestFilterModel.SupporterId });
            }
            //dapperExecution.SqlBuilder.Where("(t1.HandleUserId IS NULL Or t1.HandleUserId = @handleUserId)", new { handleUserId = requestFilterModel.SupporterId });
            var rs =  dapperExecution
                .ExecutePaginateQuery<TransactionDTO, string, ContractorDTO>(
                    (acceptance, outContractCode, contractor) =>
                    {
                        acceptance.Contractor = contractor;
                        if (!cache.TryGetValue(acceptance.Id, out var acceptanceEntry))
                        {
                            acceptanceEntry = acceptance;

                            cache.Add(acceptanceEntry.Id, acceptanceEntry);
                        }
                        
                        return acceptanceEntry;
                    }, "ContractCode, Id");
            return rs;
        }
    }
}
