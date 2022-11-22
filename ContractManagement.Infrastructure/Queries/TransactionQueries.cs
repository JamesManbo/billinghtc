using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Organizations;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.Transactions;
using Dapper;
using GenericRepository;
using GenericRepository.Core;
using GenericRepository.DapperSqlBuilder;
using GenericRepository.Extensions;
using Global.Configs.SystemArgument;
using Global.Models.Auth;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ITransactionQueries : IQueryRepository
    {
        IEnumerable<TransactionDTO> FindByIds(int[] ids);
        IEnumerable<TransactionSupporterDTO> GetTransactionSupporterReport(string userId, string strProjectIds, DateTime startDate, DateTime endDate);
        IEnumerable<TransactionPendingTaskDTO> GetCurrentPendingTaskByProject(int marketId, DateTime startDate, DateTime endDate);
        IPagedList<TransactionDTO> GetList(TransactionRequestFilterModel requestFilterModel, IEnumerable<OrganizationUnitDTO> allowedOus, bool ignorePermission = false);
        TransactionDTO Find(int id, IEnumerable<OrganizationUnitDTO> allowedOus);
        TransactionDTO FindFromSupporterService(int id);
        TransactionDTO FindByCode(string code, IEnumerable<OrganizationUnitDTO> allowedOus);
        TransactionDTO FindCanAcceptancedById(int id);
        int GetOrderNumberByContractId(int contractId, bool isAppendix, bool isOutContract = true);
        string GenerateTransactionCode(string contractCode, bool isAppendix = false);
        int GetOrderNumberByContractCode(string contractCode, bool isAppendix);
        IEnumerable<int> GetProjectIdsBySupporter(string userSupporterId);
        bool IsTransactionCodeExistsed(string transactionCode);
        IEnumerable<(string, string)> GetRadiusAccountOfTransactions(int[] transactionIds);
    }

    public class TransactionSqlBuilder : SqlBuilder
    {
        public TransactionSqlBuilder()
        {
        }
        public TransactionSqlBuilder(string tableName) : base(tableName) { }
        public void SelectServicePackage(string alias)
        {
            Select($"{alias}.`Id` AS `Id`");
            Select($"{alias}.`OutContractServicePackageId` AS `OutContractServicePackageId`");
            Select($"{alias}.`TransactionId` AS `TransactionId`");
            Select($"{alias}.`OutContractId` AS `OutContractId`");
            Select($"{alias}.`StatusId` AS `StatusId`");
            Select($"{alias}.`IsOld` AS `IsOld`");
            Select($"{alias}.`IsAcceptanced` AS `IsAcceptanced`");
            Select($"{alias}.`StartPointChannelId` AS `StartPointChannelId`");
            Select($"{alias}.`EndPointChannelId` AS `EndPointChannelId`");
            Select($"{alias}.`CurrencyUnitId` AS `CurrencyUnitId`");
            Select($"{alias}.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            Select($"{alias}.`ServiceId` AS `ServiceId`");
            Select($"{alias}.`ServiceName` AS `ServiceName`");
            Select($"{alias}.`ServicePackageId` AS `ServicePackageId`");
            Select($"{alias}.`PackageName` AS `PackageName`");
            Select($"{alias}.`PackagePrice` AS `PackagePrice`");
            Select($"{alias}.`IsFreeStaticIp` AS `IsFreeStaticIp`");
            Select($"{alias}.`BandwidthLabel` AS `BandwidthLabel`");
            Select($"{alias}.`InternationalBandwidth` AS `InternationalBandwidth`");
            Select($"{alias}.`DomesticBandwidth` AS `DomesticBandwidth`");
            Select($"{alias}.`InternationalBandwidthUom` AS `InternationalBandwidthUom`");
            Select($"{alias}.`DomesticBandwidthUom` AS `DomesticBandwidthUom`");
            Select($"{alias}.`IsInFirstBilling`");
            Select($"{alias}.`CId` AS `CId`");
            Select($"{alias}.`RadiusAccount` AS `RadiusAccount`");
            Select($"{alias}.`RadiusPassword` AS `RadiusPassword`");
            Select($"{alias}.`IsRadiusAccountCreated` AS `IsRadiusAccountCreated`");
            Select($"{alias}.`PromotionAmount` AS `PromotionAmount`");
            Select($"{alias}.`InstallationFee`");
            Select($"{alias}.`EquipmentAmount`");
            Select($"{alias}.`OtherFee`");
            Select($"{alias}.`TaxAmount` AS `TaxAmount`");
            Select($"{alias}.`SubTotalBeforeTax` AS `SubTotalBeforeTax`");
            Select($"{alias}.`SubTotal` AS `SubTotal`");
            Select($"{alias}.`GrandTotalBeforeTax` AS `GrandTotalBeforeTax`");
            Select($"{alias}.`GrandTotal` AS `GrandTotal`");
            Select($"{alias}.`HasStartAndEndPoint` AS `HasStartAndEndPoint`");
            Select($"{alias}.`TransactionServicePackageId` AS `TransactionServicePackageId`");
            Select($"{alias}.`IsTechnicalConfirmation` AS `IsTechnicalConfirmation`");
            Select($"{alias}.`IsSupplierConfirmation` AS `IsSupplierConfirmation`");
            Select($"{alias}.`ChannelGroupId` AS `ChannelGroupId`");
            Select($"{alias}.`PaymentTargetId` AS `PaymentTargetId`");
            Select($"{alias}.`LineQuantity` AS `LineQuantity`");
            Select($"{alias}.`CableKilometers` AS `CableKilometers`");
            Select($"{alias}.`FlexiblePricingTypeId` AS `FlexiblePricingTypeId`");
            Select($"{alias}.`MinSubTotal` AS `MinSubTotal`");
            Select($"{alias}.`MaxSubTotal` AS `MaxSubTotal`");
            Select($"{alias}.`HasDistinguishBandwidth` AS `HasDistinguishBandwidth`");
            Select($"{alias}.`Note` AS `Note`");
        }
        public void SelectOutputChannel(string alias)
        {
            Select($"{alias}.Id AS `Id`");
            Select($"{alias}.CurrencyUnitId AS `CurrencyUnitId`");
            Select($"{alias}.CurrencyUnitCode AS `CurrencyUnitCode`");
            Select($"{alias}.PointType AS `PointType`");
            Select($"{alias}.InstallationFee AS `InstallationFee`");
            Select($"{alias}.OtherFee AS `OtherFee`");
            Select($"{alias}.MonthlyCost AS `MonthlyCost`");
            Select($"{alias}.EquipmentAmount AS `EquipmentAmount`");

            SelectChannelInstallAddress(alias);
        }

        private void SelectChannelInstallAddress(string alias)
        {
            Select("'' AS InstallationAddressSpliter");
            Select($"{alias}.`InstallationAddress_Building` AS `Building`");
            Select($"{alias}.`InstallationAddress_Floor` AS `Floor`");
            Select($"{alias}.`InstallationAddress_RoomNumber` AS `RoomNumber`");
            Select($"{alias}.`InstallationAddress_Street` AS `Street`");
            Select($"{alias}.`InstallationAddress_District` AS `District`");
            Select($"{alias}.`InstallationAddress_DistrictId` AS `DistrictId`");
            Select($"{alias}.`InstallationAddress_City` AS `City`");
            Select($"{alias}.`InstallationAddress_CityId` AS `CityId`");
            Select($"{alias}.`InstallationAddress_Country` AS `Country`");
            Select($"{alias}.`InstallationAddress_CountryId` AS `CountryId`");
        }

        public void SelectServicePackageTimeLine(string alias)
        {
            Select($"{alias}.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            Select($"{alias}.`TimeLine_PrepayPeriod` AS `PrepayPeriod`");
            Select($"{alias}.`TimeLine_Signed` AS `Signed`");
            Select($"{alias}.`TimeLine_Effective` AS `Effective`");
            Select($"{alias}.`TimeLine_LatestBilling` AS `LatestBilling`");
            Select($"{alias}.`TimeLine_NextBilling` AS `NextBilling`");
            Select($"{alias}.`TimeLine_StartBilling` AS `StartBilling`");
            Select($"{alias}.`TimeLine_SuspensionStartDate` AS `SuspensionStartDate`");
            Select($"{alias}.`TimeLine_SuspensionEndDate` AS `SuspensionEndDate`");
            Select($"{alias}.`TimeLine_DaysSuspended` AS `DaysSuspended`");
            Select($"{alias}.`TimeLine_DaysPromotion` AS `DaysPromotion`");
            Select($"{alias}.`TimeLine_PaymentForm` AS `PaymentForm`");
        }

        public void SelectEquipment(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.CurrencyUnitId");
            Select($"{alias}.CurrencyUnitCode");
            Select($"{alias}.OutputChannelPointId");
            Select($"{alias}.OutContractPackageId");
            Select($"{alias}.EquipmentName");
            Select($"{alias}.EquipmentPictureUrl");
            Select($"{alias}.EquipmentUom");
            Select($"{alias}.UnitPrice");

            Select($"{alias}.ExaminedUnit");
            Select($"{alias}.RealUnit");
            Select($"{alias}.ActivatedUnit");
            Select($"{alias}.ReclaimedUnit");
            Select($"{alias}.SupporterHoldedUnit");
            Select($"{alias}.WillBeReclaimUnit");
            Select($"{alias}.WillBeHoldUnit");

            Select($"{alias}.IsInSurveyPlan");
            Select($"{alias}.IsFree");
            Select($"{alias}.IsOld");
            Select($"{alias}.HasToReclaim");
            Select($"{alias}.SerialCode");
            Select($"{alias}.DeviceCode");
            Select($"{alias}.Manufacturer");
            Select($"{alias}.Specifications");
            Select($"{alias}.StatusId");
            Select($"{alias}.EquipmentId");
            Select($"{alias}.SubTotal");
            Select($"{alias}.GrandTotal");
            Select($"{alias}.ExaminedSubTotal");
            Select($"{alias}.ExaminedGrandTotal");
            Select($"{alias}.ContractEquipmentId");
            Select($"{alias}.OldEquipmentId");
            Select($"{alias}.IsAcceptanced");
        }

        public void SelectPaymentTarget(string alias)
        {
            Select($"{alias}.`Id`");
            Select($"{alias}.`IdentityGuid`");
            Select($"{alias}.`ContractorCode`");
            Select($"{alias}.`ContractorFullName`");
            Select($"{alias}.`ContractorPhone`");
            Select($"{alias}.`ContractorEmail`");
            Select($"{alias}.`ContractorFax`");
            Select($"{alias}.`ContractorAddress`");
            Select($"{alias}.`ContractorIdNo`");
            Select($"{alias}.`ContractorTaxIdNo`");
            Select($"{alias}.`IsEnterprise`");
            Select($"{alias}.`IsBuyer`");
            Select($"{alias}.`IsPartner`");
            Select($"{alias}.`ApplicationUserIdentityGuid`");
            Select($"{alias}.`UserIdentityGuid`");
            Select($"{alias}.`AccountingCustomerCode`");
        }

        public void SelectContractor(string alias)
        {
            Select($"{alias}.`Id`");
            Select($"{alias}.`IdentityGuid`");
            Select($"{alias}.`ContractorCode`");
            Select($"{alias}.`ContractorFullName`");
            Select($"{alias}.`ContractorShortName`");
            Select($"{alias}.`ContractorPhone`");
            Select($"{alias}.`ContractorEmail`");
            Select($"{alias}.`ContractorFax`");
            Select($"{alias}.`ContractorAddress`");
            Select($"{alias}.`ContractorIdNo`");
            Select($"{alias}.`ContractorTaxIdNo`");
            Select($"{alias}.`IsEnterprise`");
            Select($"{alias}.`IsBuyer`");
            Select($"{alias}.`IsPartner`");
            Select($"{alias}.`ApplicationUserIdentityGuid`");
            Select($"{alias}.`UserIdentityGuid`");
            Select($"{alias}.`AccountingCustomerCode`");
        }
    }

    public class TransactionQueries : QueryRepository<Transaction, int>, ITransactionQueries
    {
        public const string SUPPLIER_VIEW_PERMISSIONS = "VIEW_TRANSACTION_OF_SERVICE_SUPPLIER";
        public const string SUPPORTER_VIEW_PERMISSIONS = "VIEW_TRANSACTION_OF_SUPPORTER";
        public const string VIEW_ALL_PERMISSIONS = "VIEW_TRANSACTION";
        private readonly ContractDbContext _currentContext;

        public TransactionQueries(ContractDbContext context) : base(context)
        {
            _currentContext = context;
        }

        private DapperExecution<TransactionDTO, TransactionSqlBuilder> BuildCompleteQuery()
        {
            var dapperExecution = BuildByTemplate<TransactionDTO, TransactionSqlBuilder>();

            //1 Select channel info
            dapperExecution.SqlBuilder.SelectServicePackage("t2");
            //2 
            dapperExecution.SqlBuilder.SelectServicePackageTimeLine("t2");

            // SelectPaymentTarget
            dapperExecution.SqlBuilder.SelectPaymentTarget("t3");

            // Select start point
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");
            dapperExecution.SqlBuilder.SelectEquipment("spce");
            dapperExecution.SqlBuilder.Select("t2.Id AS `TransactionServicePackageId`");

            // Select end point
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");
            dapperExecution.SqlBuilder.SelectEquipment("epce");
            dapperExecution.SqlBuilder.Select("t2.Id AS `TransactionServicePackageId`");

            //12: OutContractServicePackageTaxes
            dapperExecution.SqlBuilder.Select("t12.TransactionId");
            dapperExecution.SqlBuilder.Select("t12.TransactionServicePackageId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryName");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryCode");
            dapperExecution.SqlBuilder.Select("t12.TaxValue");

            //5 Select attachament files
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

            dapperExecution.SqlBuilder.SelectPaymentTarget("ct");

            //: Channel price bus table
            dapperExecution.SqlBuilder.Select("bt.`Id` AS `Id`");
            dapperExecution.SqlBuilder.Select("bt.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("bt.`TransactionServicePackageId` AS `TransactionServicePackageId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFrom` AS `UsageValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFromUomId` AS `UsageValueFromUomId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueTo` AS `UsageValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueToUomId` AS `UsageValueToUomId`");
            dapperExecution.SqlBuilder.Select("bt.`BasedPriceValue` AS `BasedPriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceValue` AS `PriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceUnitUomId` AS `PriceUnitUomId`");
            dapperExecution.SqlBuilder.Select("bt.`IsDomestic` AS `IsDomestic`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueFrom` AS `UsageBaseUomValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueTo` AS `UsageBaseUomValueTo`");


            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionServicePackages AS t2 ON t2.TransactionId = t1.Id AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "Contractors AS t3 ON t3.Id = t2.PaymentTargetId");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionChannelPoints csp ON csp.Id = t2.StartPointChannelId"
                );

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionChannelPoints cep ON cep.Id = t2.EndPointChannelId"
                );

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionPriceBusTables bt ON bt.TransactionServicePackageId = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionEquipments spce ON t2.StartPointChannelId = spce.OutputChannelPointId AND spce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionEquipments epce ON t2.EndPointChannelId = epce.OutputChannelPointId AND epce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionSLAs sla ON (t2.IsDefaultSLAByServiceId = 1 AND sla.ServiceId = t2.ServiceId ) OR (t2.IsDefaultSLAByServiceId = 0 AND t2.Id = sla.TransactionServicePackageId) AND sla.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "TransactionChannelTaxes t12 ON t2.Id = t12.TransactionServicePackageId");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON af.TransactionId = t1.Id AND af.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("Contractors ct ON ct.Id = t1.ContractorId");

            return dapperExecution;
        }

        public TransactionDTO Find(int id, IEnumerable<OrganizationUnitDTO> allowedOus)
        {
            return GetDetail(id, allowedOus);
        }

        public TransactionDTO FindByCode(string code, IEnumerable<OrganizationUnitDTO> allowedOus)
        {
            return GetDetail(code, allowedOus);
        }

        public TransactionDTO GetDetail(object key, IEnumerable<OrganizationUnitDTO> allowedOus = null, bool ignorePermission = false)
        {
            if (
                !ignorePermission &&
                (UserIdentity.Permissions == null ||
                UserIdentity.Permissions.All(p => p != VIEW_ALL_PERMISSIONS
                    && p != SUPPLIER_VIEW_PERMISSIONS
                    && p != SUPPORTER_VIEW_PERMISSIONS)) &&
                (UserIdentity.Organizations == null || UserIdentity.Organizations.Length == 0)
            )
            {
                return default;
            }

            var cache = new Dictionary<int, TransactionDTO>();
            var dapperExecution = this.BuildCompleteQuery();
            if (key is int id)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else if (key is string code)
            {
                dapperExecution.SqlBuilder.Where("t1.Code = @code", new { code });
            }

            if (!ignorePermission)
            {
                if (UserIdentity.Permissions.All(p => p != VIEW_ALL_PERMISSIONS))
                {
                    if (UserIdentity.Permissions.Any(p => p == SUPPLIER_VIEW_PERMISSIONS))
                    {
                        dapperExecution.SqlBuilder.Where("t1.IsSupplierConfirmation = TRUE");
                    }

                    if (UserIdentity.Permissions.Any(p => p == SUPPORTER_VIEW_PERMISSIONS))
                    {
                        dapperExecution.SqlBuilder.Where("t1.IsTechnicalConfirmation = TRUE");
                    }
                    else if (allowedOus != null && allowedOus.Any(o => UserIdentity.Organizations.Contains(o.Code)))
                    {
                        dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians pt ON pt.ProjectId = t1.ProjectId");
                        dapperExecution.SqlBuilder.Where(
                            "pt.UsertechnicianId = @userSupporterId", new { userSupporterId = UserIdentity.UniversalId });
                    }
                }
            }

            return WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql, new[] {
                typeof(TransactionDTO), //0
                typeof(TransactionServicePackageDTO), //1
                typeof(BillingTimeLine), //2
                typeof(ContractorDTO), //3
                typeof(TransactionChannelPointDTO), //4
                typeof(InstallationAddress), //5
                typeof(TransactionEquipmentDTO), //6
                typeof(TransactionChannelPointDTO), //7
                typeof(InstallationAddress), //8
                typeof(TransactionEquipmentDTO), //9
                typeof(TransactionChannelTaxDTO), //10
                typeof(AttachmentFileDTO), //11
                typeof(ContractorDTO), //12
                typeof(TransactionPriceBusTableDTO), //13
            }, (results) =>
            {
                var transaction = results[0] as TransactionDTO;
                if (!cache.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    cache.Add(transactionEntry.Id, transactionEntry);
                }

                if (results[1] is TransactionServicePackageDTO channel)
                {
                    var existedSrvPackage = transactionEntry.TransactionServicePackages.FirstOrDefault(s => s.Id == channel.Id);
                    if (existedSrvPackage == null)
                    {
                        transactionEntry.TransactionServicePackages.Add(channel);
                    }
                    else
                    {
                        channel = existedSrvPackage;
                    }

                    channel.TimeLine = results[2] as BillingTimeLine;
                    channel.PaymentTarget = results[3] as ContractorDTO;

                    var channelStartPoint = results[4] as TransactionChannelPointDTO;
                    if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                    if (channel.StartPoint != null)
                    {
                        channel.StartPoint.InstallationAddress = results[5] as InstallationAddress;
                        if (results[6] is TransactionEquipmentDTO equipment)
                        {
                            equipment.ChannelCId = channel.CId;
                            equipment.InstallationFullAddress = channel.StartPoint.InstallationAddress.FullAddress;
                            if (channel.StartPoint.Equipments.All(s => s.Id != equipment.Id))
                            {
                                channel.StartPoint.Equipments.Add(equipment);
                            }
                        }
                    }

                    var channelEndPoint = results[7] as TransactionChannelPointDTO;
                    if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                    if (channel.EndPoint != null)
                    {
                        channel.EndPoint.InstallationAddress = results[8] as InstallationAddress;
                        if (results[9] is TransactionEquipmentDTO equipment)
                        {
                            equipment.ChannelCId = channel.CId;
                            equipment.InstallationFullAddress = channel.EndPoint.InstallationAddress.FullAddress;
                            if (channel.EndPoint.Equipments.All(s => s.Id != equipment.Id))
                            {
                                channel.EndPoint.Equipments.Add(equipment);
                            }
                        }
                    }

                    if (results[10] is TransactionChannelTaxDTO channelTaxValue)
                    {
                        if (channelTaxValue != null && channel.TransactionChannelTaxes.All(s => s.TaxCategoryId != channelTaxValue.TaxCategoryId))
                        {
                            channel.TransactionChannelTaxes.Add(channelTaxValue);
                        }
                    }

                    if (results[13] is TransactionPriceBusTableDTO channelPrice)
                    {
                        if (channel.PriceBusTables.All(s => s.Id != channelPrice.Id))
                            channel.PriceBusTables.Add(channelPrice);
                    }
                }

                var transactionAttachmentFile = results[11] as AttachmentFileDTO;
                if (transactionAttachmentFile != null && transactionEntry.AttachmentFiles.All(s => s.Id != transactionAttachmentFile.Id))
                    transactionEntry.AttachmentFiles.Add(transactionAttachmentFile);

                var contractor = results[12] as ContractorDTO;
                transactionEntry.Contractor = contractor;

                return transactionEntry;
            }, dapperExecution.ExecutionTemplate.Parameters,
            null,
            true,
            "Id,PaymentPeriod,Id,Id,InstallationAddressSpliter,Id,Id,InstallationAddressSpliter,Id,TransactionId,Id,Id,Id"))
            .FirstOrDefault();
        }
        public TransactionDTO FindCanAcceptancedById(int id)
        {
            var transaction = this.GetDetail(id, ignorePermission: true);
            if (transaction.IsTechnicalConfirmation != true || transaction.StatusId != TransactionStatus.WaitAcceptanced.Id) return null;
            return transaction;
        }
        public TransactionDTO FindFromSupporterService(int id)
        {
            return this.GetDetail(id, ignorePermission: true);
        }
        public IEnumerable<TransactionDTO> FindByIds(int[] ids)
        {
            var cache = new Dictionary<int, TransactionDTO>();

            var dapperExecution = this.BuildCompleteQuery();
            dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });

            return WithCurrentConnection(
                (conn) => conn.Query(dapperExecution.ExecutionTemplate.RawSql, new[] {
                typeof(TransactionDTO), //0
                typeof(TransactionServicePackageDTO), //1
                typeof(BillingTimeLine), //2
                typeof(ContractorDTO), //3
                typeof(TransactionChannelPointDTO), //4
                typeof(InstallationAddress), //5
                typeof(TransactionEquipmentDTO), //6
                typeof(TransactionChannelPointDTO), //7
                typeof(InstallationAddress), //8
                typeof(TransactionEquipmentDTO), //9
                typeof(TransactionChannelTaxDTO), //10
                typeof(AttachmentFileDTO), //11
                typeof(ContractorDTO) //12
            }, (results) =>
            {
                var transaction = results[0] as TransactionDTO;
                if (!cache.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    cache.Add(transactionEntry.Id, transactionEntry);
                }

                if (results[1] is TransactionServicePackageDTO channel)
                {
                    var existedSrvPackage = transactionEntry.TransactionServicePackages.FirstOrDefault(s => s.Id == channel.Id);
                    if (existedSrvPackage == null)
                    {
                        transactionEntry.TransactionServicePackages.Add(channel);
                    }
                    else
                    {
                        channel = existedSrvPackage;
                    }

                    channel.TimeLine = results[2] as BillingTimeLine;
                    channel.PaymentTarget = results[3] as ContractorDTO;

                    var channelStartPoint = results[4] as TransactionChannelPointDTO;
                    if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                    if (channel.StartPoint != null)
                    {
                        channel.StartPoint.InstallationAddress = results[5] as InstallationAddress;
                        if (results[6] is TransactionEquipmentDTO equipment)
                        {
                            equipment.ChannelCId = channel.CId;
                            equipment.InstallationFullAddress = channel.StartPoint.InstallationAddress.FullAddress;
                            if (channel.StartPoint.Equipments.All(s => s.Id != equipment.Id))
                            {
                                channel.StartPoint.Equipments.Add(equipment);
                            }
                        }
                    }

                    var channelEndPoint = results[7] as TransactionChannelPointDTO;
                    if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                    if (channel.EndPoint != null)
                    {
                        channel.EndPoint.InstallationAddress = results[8] as InstallationAddress;
                        if (results[9] is TransactionEquipmentDTO equipment)
                        {
                            equipment.ChannelCId = channel.CId;
                            equipment.InstallationFullAddress = channel.EndPoint.InstallationAddress.FullAddress;
                            if (channel.EndPoint.Equipments.All(s => s.Id != equipment.Id))
                            {
                                channel.EndPoint.Equipments.Add(equipment);
                            }
                        }
                    }

                    if (results[10] is TransactionChannelTaxDTO channelTaxValue)
                    {
                        if (channelTaxValue != null && channel.TransactionChannelTaxes.All(s => s.TaxCategoryId != channelTaxValue.TaxCategoryId))
                        {
                            channel.TransactionChannelTaxes.Add(channelTaxValue);
                        }
                    }
                }

                var transactionAttachmentFile = results[11] as AttachmentFileDTO;
                if (transactionAttachmentFile != null && transactionEntry.AttachmentFiles.All(s => s.Id != transactionAttachmentFile.Id))
                    transactionEntry.AttachmentFiles.Add(transactionAttachmentFile);

                var contractor = results[12] as ContractorDTO;
                transactionEntry.Contractor = contractor;

                return transactionEntry;
            }, dapperExecution.ExecutionTemplate.Parameters,
            _currentContext.GetCurrentTransaction().GetDbTransaction(),
            true,
            "Id,PaymentPeriod,Id,Id,InstallationAddressSpliter,Id,Id,InstallationAddressSpliter,Id,TransactionId,Id,Id"))
            .Distinct();
        }
        public IPagedList<TransactionDTO> GetList(TransactionRequestFilterModel requestFilterModel, IEnumerable<OrganizationUnitDTO> allowedOus, bool ignorePermission = false)
        {
            if (!ignorePermission && (
                (UserIdentity.Permissions == null ||
                UserIdentity.Permissions.All(p => p != VIEW_ALL_PERMISSIONS
                    && p != SUPPLIER_VIEW_PERMISSIONS
                    && p != SUPPORTER_VIEW_PERMISSIONS)) &&
                (UserIdentity.Organizations == null || UserIdentity.Organizations.Length == 0)
                )
            )
            {
                return default;
            }

            var cache = new Dictionary<int, TransactionDTO>();
            var dapperExecution = BuildByTemplate<TransactionDTO, TransactionSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("ct.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("IFNULL(oc.`ContractCode`, ic.`ContractCode`) AS `ContractCode`");

            dapperExecution.SqlBuilder.SelectContractor("ct");

            dapperExecution.SqlBuilder.InnerJoin("Contractors ct ON ct.Id = t1.ContractorId");

            if (requestFilterModel.Any("contractorFullName"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("ct.`ContractorFullName`",
                    requestFilterModel.GetProperty("contractorFullName"));
            }

            if (requestFilterModel
                .Any("outContractId"))
            {
                dapperExecution.SqlBuilder.InnerJoin(
                    "OutContracts oc ON t1.OutContractId = oc.Id");
            }
            else if (requestFilterModel.Any("inContractId"))
            {
                dapperExecution.SqlBuilder.LeftJoin(
                    "InContracts ic ON t1.InContractId = ic.Id");
            }
            else
            {
                dapperExecution.SqlBuilder.LeftJoin(
                    "InContracts ic ON t1.InContractId = ic.Id");
                dapperExecution.SqlBuilder.LeftJoin(
                    "OutContracts oc ON t1.OutContractId = oc.Id");
            }

            if (requestFilterModel.StatusIds != null && !string.IsNullOrEmpty(requestFilterModel.StatusIds))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId IN @statusIds", new
                {
                    statusIds = requestFilterModel.StatusIds.SplitToArray(',')
                });
            }

            if (requestFilterModel.AcceptanceTypes != null && !string.IsNullOrEmpty(requestFilterModel.AcceptanceTypes))
            {
                dapperExecution.SqlBuilder.Where("t1.Type IN @typeIds", new
                {
                    typeIds = requestFilterModel.AcceptanceTypes.SplitToArray(',')
                });
            }

            if (requestFilterModel.Keywords != null && !string.IsNullOrEmpty(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder.Where("(t1.ContractCode LIKE @keywords " +
                    "OR t1.Code LIKE @keywords OR ct.ContractorFullName LIKE @keywords " +
                    "OR ct.ContractorPhone LIKE @keywords)", new
                    {
                        keywords = $"%{requestFilterModel.Keywords}%"
                    });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                var projectIds = requestFilterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t1.ProjectId IN @projectIds Or t1.ProjectId IS NULL)", new { projectIds });
            }

            if (ignorePermission)
            {
                dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians pt ON pt.ProjectId = t1.ProjectId");
                dapperExecution.SqlBuilder.Where(
                    "pt.UsertechnicianId = @userSupporterId", new { userSupporterId = requestFilterModel.SupporterId });
            }
            else
            {
                if (UserIdentity.Permissions.All(p => p != VIEW_ALL_PERMISSIONS))
                {
                    if (UserIdentity.Permissions.Any(p => p == SUPPLIER_VIEW_PERMISSIONS))
                    {
                        dapperExecution.SqlBuilder.Where("t1.IsSupplierConfirmation = TRUE");
                    }

                    if (UserIdentity.Permissions.Any(p => p == SUPPORTER_VIEW_PERMISSIONS))
                    {
                        dapperExecution.SqlBuilder.Where("t1.IsTechnicalConfirmation = TRUE");
                    }
                    else if (allowedOus != null && allowedOus.Any(o => UserIdentity.Organizations.Contains(o.Code)))
                    {
                        dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians pt ON pt.ProjectId = t1.ProjectId");
                        dapperExecution.SqlBuilder.Where(
                            "pt.UsertechnicianId = @userSupporterId", new { userSupporterId = UserIdentity.UniversalId });
                    }
                }
            }

            return dapperExecution.ExecutePaginateQuery<TransactionDTO, ContractorDTO>((transaction, contractor) =>
            {
                if (!cache.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    cache.Add(transactionEntry.Id, transactionEntry);
                }
                transactionEntry.Contractor = contractor;

                return transactionEntry;
            }, "Id");
        }
        public int GetOrderNumberByContractId(int contractId, bool isAppendix, bool isOutContract = true)
        {
            if (isAppendix == true)
            {
                return WithConnection(conn =>
                    conn.QueryFirst<int>(
                        $"SELECT (COUNT(1) + 1) FROM Transactions t WHERE t.IsAppendix = TRUE AND {(isOutContract ? "t.OutContractId" : "t.InContractId")} = @contractId", new { contractId }));
            }
            else
            {
                return WithConnection(conn =>
                    conn.QueryFirst<int>(
                        $"SELECT (COUNT(1) + 1) FROM Transactions t WHERE (t.IsAppendix IS NULL OR t.IsAppendix = FALSE) AND {(isOutContract ? "t.OutContractId" : "t.InContractId")} = @contractId", new { contractId }));
            }
        }
        public int GetOrderNumberByContractCode(string contractCode, bool isAppendix)
        {
            string latestTransactionCode = string.Empty;
            if (isAppendix == true)
            {
                latestTransactionCode = WithConnection(conn =>
                    conn.QueryFirstOrDefault<string>(
                        "SELECT t.Code FROM Transactions t WHERE t.IsAppendix = TRUE AND t.ContractCode = @contractCode ORDER BY t.Id DESC", new { contractCode }));
            }
            else
            {
                latestTransactionCode = WithConnection(conn =>
                    conn.QueryFirstOrDefault<string>(
                        "SELECT t.Code FROM Transactions t WHERE (t.IsAppendix IS NULL OR t.IsAppendix = FALSE) AND t.ContractCode = @contractCode ORDER BY t.Id DESC", new { contractCode }));
            }

            if (string.IsNullOrEmpty(latestTransactionCode))
            {
                return 1;
            }

            var rawTransactionIndex = latestTransactionCode.Substring(2, 2);
            if (int.TryParse(rawTransactionIndex, out var transactionIdx))
            {
                return transactionIdx + 1;
            }
            return 1;
        }
        public IEnumerable<TransactionSupporterDTO> GetTransactionSupporterReport(string userId, string strProjectIds, DateTime startDate, DateTime endDate)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<TransactionSupporterDTO>();
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.StatusId =1,1,0)) AS PendingTask");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.StatusId =2,1,0)) AS DoneTask");
            dapperExecution.SqlBuilder.Select("SUM(IF(t1.StatusId =3,1,0)) AS CancelTask");
            dapperExecution.SqlBuilder.Select("COUNT(t1.Id) AS TotalTask");
            //dapperExecution.SqlBuilder.Select("t1.Type as TransactionType");
            //dapperExecution.SqlBuilder.Select("tt.Name as TransactionTypeName");

            dapperExecution.SqlBuilder.InnerJoin("OutContracts AS t3 ON t3.Id = t1.OutContractId");
            dapperExecution.SqlBuilder.LeftJoin("ProjectTechnicians AS t4 ON t3.ProjectId = t4.ProjectId ");
            dapperExecution.SqlBuilder.InnerJoin("TransactionType AS tt ON t1.Type = tt.Id");
            dapperExecution.SqlBuilder.Where("t4.UserTechnicianId = @userId", new { userId });
            dapperExecution.SqlBuilder.Where("t1.IsTechnicalConfirmation = true");
            if (!string.IsNullOrEmpty(strProjectIds))
            {
                var projectIds = strProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t3.ProjectId IN @projectIds Or t3.ProjectId IS NULL)", new { projectIds });
            }
            if (startDate != null)
            {
                dapperExecution.SqlBuilder.Where("t1.TransactionDate >= @startDate", new { startDate });
            }
            if (endDate != null)
            {
                var end = endDate.AddDays(1);
                dapperExecution.SqlBuilder.Where("t1.TransactionDate <= @end", new { end });
            }
            dapperExecution.SqlBuilder.GroupBy("t4.UserTechnicianId");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<TransactionPendingTaskDTO> GetCurrentPendingTaskByProject(int marketId, DateTime startDate, DateTime endDate)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<TransactionPendingTaskDTO>();
            dapperExecution.SqlBuilder.Select("t2.ProjectId");
            dapperExecution.SqlBuilder.Select("t2.ProjectName");
            dapperExecution.SqlBuilder.Select("COUNT(t1.Id) AS PendingTasks");
            dapperExecution.SqlBuilder.Select("COUNT(t1.Id)/t3.NumberOfSupporters AS TaskPerProjectAVG ");

            dapperExecution.SqlBuilder.InnerJoin("OutContracts AS t2 ON t2.Id = t1.OutContractId");
            dapperExecution.SqlBuilder.InnerJoin("Projects AS t3 ON t3.Id = t2.ProjectId ");

            dapperExecution.SqlBuilder.Where("t1.StatusId = 1");
            dapperExecution.SqlBuilder.Where("IFNULL(t3.NumberOfSupporters,0) > 0");
            dapperExecution.SqlBuilder.Where("t2.MarketAreaId = @marketId", new { marketId });
            dapperExecution.SqlBuilder.GroupBy("ProjectName");
            dapperExecution.SqlBuilder.OrderBy("TaskPerProjectAVG");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<int> GetProjectIdsBySupporter(string userSupporterId)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("DISTINCT t1.ProjectId");
            dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians pt ON  pt.ProjectId = t1.ProjectId");
            dapperExecution.SqlBuilder.Where("pt.UsertechnicianId = @userSupporterId", new { userSupporterId });
            return dapperExecution.ExecuteQuery();
        }

        public bool IsTransactionCodeExistsed(string transactionCode)
        {
            if (string.IsNullOrWhiteSpace(transactionCode)) return false;

            var existedRecords = WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT COUNT(1) FROM Transactions t WHERE LOWER(t.Code) LIKE LOWER(@transactionCode)", new
                    {
                        transactionCode = transactionCode.Trim()
                    }));

            return existedRecords > 0;
        }



        public IEnumerable<(string, string)> GetRadiusAccountOfTransactions(int[] transactionIds)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<(string, string)>();

            dapperExecution.SqlBuilder.Select("t2.RadiusAccount AS RadiusAccount");
            dapperExecution.SqlBuilder.Select("t2.RadiusPassword AS RadiusPassword");
            dapperExecution.SqlBuilder.InnerJoin("TransactionServicePackages t2 ON t2.TransactionId = t1.Id");

            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE AND t1.Id IN @transactionIds", new { transactionIds });
            dapperExecution.SqlBuilder.Where("t2.IsDeleted = FALSE");

            var rawResult = dapperExecution.ExecuteQuery(
                    new Type[]
                    {
                        typeof(TransactionServicePackageDTO)
                    },
                    (objects) =>
                    {
                        var channel = objects[0] as TransactionServicePackageDTO;
                        return (channel.RadiusAccount, channel.RadiusPassword);
                    }
            );

            return rawResult.Where(c => !string.IsNullOrEmpty(c.Item1))
                .Distinct()
                .ToList();
        }

        public string GenerateTransactionCode(string contractCode, bool isAppendix = false)
        {
            var transactionIndex = this.GetOrderNumberByContractCode(contractCode, isAppendix);
            return $"{(isAppendix ? "PL" : "TS")}{transactionIndex.ToString("D2")}_{contractCode}";
        }
    }
}
