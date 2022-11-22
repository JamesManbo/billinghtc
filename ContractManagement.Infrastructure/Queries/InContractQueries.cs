using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Utility;
using Dapper;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models.SharingRevenueModels;
using Global.Models.Response;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using ContractManagement.Domain.Seed;
using GenericRepository.DapperSqlBuilder;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IInContractQueries : IQueryRepository
    {
        int GetOrderNumberByNow();
        string GenerateContractCode(int contractType, string contractorFullName, int? marketAreaId);
        bool IsCodeExisted(string contractCode, int contractId);
        InContractDTO FindById(int id);
        IEnumerable<InContractSimpleDTO> GetSimpleAll(RequestFilterModel requestFilterModel);
        IEnumerable<InContractDTO> Autocomplete(RequestFilterModel requestFilterModel);
        IEnumerable<SelectionItem> AutocompleteSimple(RequestFilterModel requestFilterModel);
        IPagedList<InContractGridDTO> GetPagedList(RequestFilterModel requestFilterModel);
        IEnumerable<OutContractSharingRevenueDTO> GetOutContractSharingRevenues(int inContractId, int? inContractType, int currencyUnitId);
    }

    public class InContractSqlBuilder: SqlBuilder
    {
        public InContractSqlBuilder()
        {
        }
        public InContractSqlBuilder(string tableName): base(tableName)
        {
        }

        public void SelectServicePackage(string alias)
        {
            Select($"{alias}.`Id` AS `Id`");
            Select($"{alias}.`StartPointChannelId` AS `StartPointChannelId`");
            Select($"{alias}.`EndPointChannelId` AS `EndPointChannelId`");
            Select($"{alias}.`InContractId` AS `InContractId`");
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
            Select($"{alias}.`StatusId` AS `StatusId`");
            Select($"{alias}.`HasStartAndEndPoint` AS `HasStartAndEndPoint`");
            Select($"{alias}.`TransactionServicePackageId` AS `TransactionServicePackageId`");
            Select($"{alias}.`IsTechnicalConfirmation` AS `IsTechnicalConfirmation`");
            Select($"{alias}.`ChannelGroupId` AS `ChannelGroupId`");
            Select($"{alias}.`PaymentTargetId` AS `PaymentTargetId`");
            Select($"{alias}.`LineQuantity` AS `LineQuantity`");
            Select($"{alias}.`CableKilometers` AS `CableKilometers`");
            Select($"{alias}.`FlexiblePricingTypeId` AS `FlexiblePricingTypeId`");
            Select($"{alias}.`MinSubTotal` AS `MinSubTotal`");
            Select($"{alias}.`MaxSubTotal` AS `MaxSubTotal`");
            Select($"{alias}.`HasDistinguishBandwidth` AS `HasDistinguishBandwidth`");
            Select($"{alias}.`IsDefaultSLAByServiceId` AS `IsDefaultSLAByServiceId`");
            Select($"{alias}.`Note` AS `Note`");
        }

        public void SelectOutputChannel(string alias)
        {
            Select($"{alias}.Id AS `Id`");
            Select($"{alias}.LocationId AS `LocationId`");
            Select($"{alias}.CurrencyUnitId AS `CurrencyUnitId`");
            Select($"{alias}.CurrencyUnitCode AS `CurrencyUnitCode`");
            Select($"{alias}.PointType AS `PointType`");
            Select($"{alias}.InstallationFee AS `InstallationFee`");
            Select($"{alias}.OtherFee AS `OtherFee`");
            Select($"{alias}.MonthlyCost AS `MonthlyCost`");
            Select($"{alias}.EquipmentAmount AS `EquipmentAmount`");
            Select($"{alias}.ApplyFeeToChannel AS `ApplyFeeToChannel`");

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
        }
        public void SelectServicePackageTimeLine(string alias)
        {
            Select($"{alias}.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            Select($"{alias}.`TimeLine_PrepayPeriod` AS `PrepayPeriod`");
            Select($"{alias}.`TimeLine_Signed` AS `Signed`");
            Select($"{alias}.`TimeLine_Effective` AS `Effective`");
            Select($"{alias}.`TimeLine_StartBilling` AS `StartBilling`");
            Select($"{alias}.`TimeLine_NextBilling` AS `NextBilling`");
            Select($"{alias}.`TimeLine_LatestBilling` AS `LatestBilling`");
            Select($"{alias}.`TimeLine_SuspensionStartDate` AS `SuspensionStartDate`");
            Select($"{alias}.`TimeLine_SuspensionEndDate` AS `SuspensionEndDate`");
            Select($"{alias}.`TimeLine_DaysSuspended` AS `DaysSuspended`");
            Select($"{alias}.`TimeLine_DaysPromotion` AS `DaysPromotion`");
            Select($"{alias}.`TimeLine_PaymentForm` AS `PaymentForm`");
            Select($"{alias}.`TimeLine_TerminateDate` AS `TerminateDate`");
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
            Select($"{alias}.ReclaimedUnit");
            Select($"{alias}.SupporterHoldedUnit");
            Select($"{alias}.IsInSurveyPlan");
            Select($"{alias}.IsFree");
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
        }

        public void SelectContractTotalByCurrency(string alias)
        {
            Select($"{alias}.Id AS `Id`");
            Select($"{alias}.CurrencyUnitId AS `CurrencyUnitId`");
            Select($"{alias}.CurrencyUnitCode AS `CurrencyUnitCode`");
            Select($"{alias}.InContractId AS `InContractId`");
            Select($"{alias}.PromotionTotalAmount AS `PromotionTotalAmount`");
            Select($"{alias}.ServicePackageAmount AS `ServicePackageAmount`");
            Select($"{alias}.TotalTaxAmount AS `TotalTaxAmount`");
            Select($"{alias}.InstallationFee AS `InstallationFee`");
            Select($"{alias}.OtherFee AS `OtherFee`");
            Select($"{alias}.EquipmentAmount AS `EquipmentAmount`");
            Select($"{alias}.SubTotalBeforeTax AS `SubTotalBeforeTax`");
            Select($"{alias}.SubTotal AS `SubTotal`");
            Select($"{alias}.GrandTotalBeforeTax AS `GrandTotalBeforeTax`");
            Select($"{alias}.GrandTotal AS `GrandTotal`");
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

    public class InContractQueries : QueryRepository<InContract, int>, IInContractQueries
    {
        private readonly IMarketAreaQueries _marketAreaQueries;
        public InContractQueries(ContractDbContext context,
            IMarketAreaQueries marketAreaQueries) : base(context)
        {
            _marketAreaQueries = marketAreaQueries;
        }
        public bool IsCodeExisted(string contractCode, int contractId)
        {
            return WithConnection(conn
                => conn.ExecuteScalar<bool>(
                    "SELECT COUNT(1) FROM InContracts WHERE TRIM(ContractCode) = @contractCode AND IsDeleted = FALSE AND Id <> @contractId",
                    new
                    {
                        contractId,
                        contractCode = contractCode.Trim()
                    }));
        }

        public InContractDTO FindById(int id)
        {
            var cached = new Dictionary<int, InContractDTO>();
            var dapperExecution = BuildByTemplate<InContractDTO, InContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select(@"CASE  WHEN t1.TimeLine_NextBillingDate IS NULL THEN t1.TimeLine_StartBillingDate
                                                        ELSE t1.TimeLine_NextBillingDate
                                                END AS BillingDate");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");
            dapperExecution.SqlBuilder.Select("t1.`IsIncidentControl` AS `IsIncidentControl`");
            dapperExecution.SqlBuilder.Select("t1.`IsControlUsageCapacity` AS `IsControlUsageCapacity`");

            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Signed` AS `Signed`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Effective` AS `Effective`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Expiration` AS `Expiration`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");


            // Select contractor
            dapperExecution.SqlBuilder.SelectContractor("t4");

            // Select the out contract channel
            dapperExecution.SqlBuilder.SelectServicePackage("t2");

            //5
            dapperExecution.SqlBuilder.SelectServicePackageTimeLine("t2");

            //7, 8
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");

            //9, 10
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");

            //11: Select the out contract equipments
            dapperExecution.SqlBuilder.SelectEquipment("spce");
            dapperExecution.SqlBuilder.Select("t2.Id AS `OutContractPackageId`");
            dapperExecution.SqlBuilder.SelectEquipment("epce");
            dapperExecution.SqlBuilder.Select("t2.Id AS `OutContractPackageId`");

            // 12
            dapperExecution.SqlBuilder.Select("af.Id");
            dapperExecution.SqlBuilder.Select("af.Name");
            dapperExecution.SqlBuilder.Select("af.InContractId");
            dapperExecution.SqlBuilder.Select("af.FileName");
            dapperExecution.SqlBuilder.Select("af.FilePath");
            dapperExecution.SqlBuilder.Select("af.Size");
            dapperExecution.SqlBuilder.Select("af.FileType");
            dapperExecution.SqlBuilder.Select("af.Extension");
            dapperExecution.SqlBuilder.Select("af.RedirectLink");
            //ContactInfo 13
            dapperExecution.SqlBuilder.Select("t5.Id");
            dapperExecution.SqlBuilder.Select("t5.InContractId");
            dapperExecution.SqlBuilder.Select("t5.Name");
            dapperExecution.SqlBuilder.Select("t5.PhoneNumber");
            dapperExecution.SqlBuilder.Select("t5.Email");
            //14 InConTractTax
            dapperExecution.SqlBuilder.Select("t8.Id");
            dapperExecution.SqlBuilder.Select("t8.TaxCategoryId");
            dapperExecution.SqlBuilder.Select("t8.InContractId");

            //15 TaxCategory
            dapperExecution.SqlBuilder.Select("t9.Id");
            dapperExecution.SqlBuilder.Select("t9.TaxValue");
            dapperExecution.SqlBuilder.Select("t9.TaxName");

            ////16: InContractServices
            //dapperExecution.SqlBuilder.Select("ics.Id");
            //dapperExecution.SqlBuilder.Select("ics.ServiceId");
            //dapperExecution.SqlBuilder.Select("ics.ShareType");
            //dapperExecution.SqlBuilder.Select("ics.ServiceName");
            //dapperExecution.SqlBuilder.Select("ics.PointType");
            //dapperExecution.SqlBuilder.Select("ics.SharedInstallFeePercent");
            //dapperExecution.SqlBuilder.Select("ics.SharedPackagePercent");

            //16: OutContractServicePackageTaxes
            dapperExecution.SqlBuilder.Select("t12.OutContractServicePackageId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryName");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryCode");
            dapperExecution.SqlBuilder.Select("t12.TaxValue");

            //17: ContractContent
            dapperExecution.SqlBuilder.Select("ct.`Id`");
            dapperExecution.SqlBuilder.Select("ct.`Content`");
            dapperExecution.SqlBuilder.Select("ct.`ContractFormId`");

            dapperExecution.SqlBuilder.SelectContractTotalByCurrency("tol");


            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t4 ON t4.Id = t1.ContractorId AND t4.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackages t2 ON t1.Id = t2.InContractId AND t2.IsDeleted = FALSE AND t2.StatusId IN @validChannelStatuses",
                new
                {
                    validChannelStatuses = OutContractServicePackageStatus.CanBeListedStatuses()
                });

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints cep ON cep.Id = t2.EndPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints csp ON csp.Id = t2.StartPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContractEquipments spce ON t2.StartPointChannelId = spce.OutputChannelPointId AND spce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContractEquipments epce ON t2.EndPointChannelId = epce.OutputChannelPointId AND epce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackageTaxes t12 ON t2.Id = t12.OutContractServicePackageId");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON t1.Id = af.InContractId AND af.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContactInfos t5 ON t1.Id = t5.InContractId AND t5.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "InContractTaxes t8 ON t1.Id = t8.InContractId AND t8.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "TaxCategories t9 ON t8.TaxCategoryId = t9.Id AND t9.IsDeleted = FALSE");

            //dapperExecution.SqlBuilder.LeftJoin(
            //    "InContractServices ics ON ics.InContractId = t1.Id AND ics.IsDeleted = FALSE");
            
            dapperExecution.SqlBuilder.LeftJoin(
                "ContractContents ct ON ct.InContractId = t1.Id AND ct.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContractTotalByCurrencies tol ON tol.InContractId = t1.Id");

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(InContractDTO),
                    typeof(PaymentMethod),
                    typeof(ContractTimeLine),
                    typeof(ContractorDTO),
                    typeof(OutContractServicePackageDTO), // 4
                    typeof(BillingTimeLine), // 5
                    typeof(OutputChannelPointDTO), // 6
                    typeof(InstallationAddress), // 7
                    typeof(OutputChannelPointDTO), // 8
                    typeof(InstallationAddress), // 9
                    typeof(ContractEquipmentDTO), // 10
                    typeof(ContractEquipmentDTO), // 11
                    typeof(AttachmentFileDTO), //12
                    typeof(ContactInfoDTO), //13
                    typeof(ContractOfTaxDTO), //14
                    typeof(TaxCategoryDTO), //15
                    //typeof(InContractServiceDTO), //16
                    typeof(OutContractServicePackageTaxDTO), //16
                    typeof(ContractContentDTO), // 17
                    typeof(ContractTotalByCurrencyDTO), // 18
                }, results =>
                {
                    var contractEntry = results[0] as InContractDTO;
                    if (contractEntry == null) return null;

                    if (!cached.TryGetValue(contractEntry.Id, out var result))
                    {
                        result = contractEntry;
                        cached.Add(contractEntry.Id, contractEntry);
                    }

                    result.Payment = results[1] as PaymentMethod;
                    result.TimeLine = results[2] as ContractTimeLine;
                    result.Contractor = results[3] as ContractorDTO;
                    result.ContractContent = results[17] as ContractContentDTO;

                    if (results[4] is OutContractServicePackageDTO channel)
                    {
                        var existedSrvPackage = result.ServicePackages.FirstOrDefault(s => s.Id == channel.Id);

                        if (existedSrvPackage == null)
                        {
                            result.ServicePackages.Add(channel);
                        }
                        else
                        {
                            channel = existedSrvPackage;
                        }

                        channel.TimeLine = results[5] as BillingTimeLine;
                        channel.PaymentTarget = result.Contractor;

                        var channelStartPoint = results[6] as OutputChannelPointDTO;
                        if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                        if (channel.StartPoint != null)
                        {
                            channel.StartPoint.InstallationAddress = results[7] as InstallationAddress;
                            if (results[10] is ContractEquipmentDTO equipment)
                            {
                                equipment.ChannelCId = channel.CId;
                                equipment.InstallationFullAddress = channel.StartPoint.InstallationAddress.FullAddress;
                                if (channel.StartPoint.Equipments.All(s => s.Id != equipment.Id))
                                {
                                    channel.StartPoint.Equipments.Add(equipment);
                                }
                            }
                        }

                        var channelEndPoint = results[8] as OutputChannelPointDTO;
                        if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                        if (channel.EndPoint != null)
                        {
                            channel.EndPoint.InstallationAddress = results[9] as InstallationAddress;
                            if (results[11] is ContractEquipmentDTO equipment)
                            {
                                equipment.ChannelCId = channel.CId;
                                equipment.InstallationFullAddress = channel.EndPoint.InstallationAddress.FullAddress;
                                if (channel.EndPoint.Equipments.All(s => s.Id != equipment.Id))
                                {
                                    channel.EndPoint.Equipments.Add(equipment);
                                }
                            }
                        }

                        if (results[16] is OutContractServicePackageTaxDTO outSrvPckTax)
                        {
                            if (outSrvPckTax != null && channel.OutContractServicePackageTaxes.All(s => s.TaxCategoryId != outSrvPckTax.TaxCategoryId))
                            {
                                channel.OutContractServicePackageTaxes.Add(outSrvPckTax);
                            }
                        }

                    }

                    if (results[12] is AttachmentFileDTO attachmentFile)
                    {
                        if (result.AttachmentFiles.All(s => s.Id != attachmentFile.Id))
                            result.AttachmentFiles.Add(attachmentFile);
                    }

                    if (results[13] is ContactInfoDTO contactInfo)
                    {
                        if (result.ContactInfos.All(s => s.Id != contactInfo.Id))
                            result.ContactInfos.Add(contactInfo);
                    }

                    if (results[14] is ContractOfTaxDTO oct)
                    {
                        if (result.ContractOfTaxes.All(s => s.Id != oct.Id))
                        {
                            result.ContractOfTaxes.Add(oct);
                        }
                    }

                    if (results[15] is TaxCategoryDTO tcg)
                    {
                        if (result.TaxCategories.All(s => s.Id != tcg.Id))
                        {
                            result.TaxCategories.Add(tcg);
                        }
                    }

                    if (results[18] is ContractTotalByCurrencyDTO contractTotal)
                    {
                        if (result.ContractTotalByCurrencies.All(s => s.Id != contractTotal.Id))
                            result.ContractTotalByCurrencies.Add(contractTotal);
                    }

                    return result;
                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                "Form,Signed,Id,Id,PaymentPeriod,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id,Id,Id,Id,Id,Id,OutContractServicePackageId,Id,Id"));

            return cached.Values.FirstOrDefault();
        }

        public IEnumerable<InContractSimpleDTO> GetSimpleAll(RequestFilterModel requestFilterModel)
        {
            var cache = new Dictionary<int, InContractSimpleDTO>();
            var dapperExecution = BuildByTemplate<InContractSimpleDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select(
                "EXISTS(SELECT 1 FROM AttachmentFiles WHERE InContractId = t1.Id AND IsDeleted = FALSE LIMIT 1) AS `HasUploadedFiles`");

            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Signed` AS `Signed`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Effective` AS `Effective`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Expiration` AS `Expiration`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");

            dapperExecution.SqlBuilder.Select("t2.`Id`");
            dapperExecution.SqlBuilder.Select("t2.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("t2.`ContractorPhone`");

            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t2 ON t2.Id = t1.ContractorId AND t2.IsDeleted = FALSE");

            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field == "contractor.fullName"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "contractor.fullName");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.FullName", propertyFilter);
            }

            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase)))
            {
                var ids =
                    requestFilterModel.PropertyFilterModels.First(
                            p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase))
                        .FilterValue.ToString().SplitToInt(',');
                dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            }

            return dapperExecution
                .ExecuteQuery<InContractSimpleDTO, ContractTimeLine,
                    ContractorSimpleDTO>(
                    (contract, contractTimeLine, contractor) =>
                    {
                        if (!cache.TryGetValue(contract.Id, out var contractEntry))
                        {
                            contractEntry = contract;
                            contractEntry.ContractStatusName = ContractStatus.From(contract.ContractStatusId).Name;

                            contractEntry.Contractor = contractor;
                            contractEntry.TimeLine = contractTimeLine;

                            cache.Add(contractEntry.Id, contractEntry);
                        }

                        return contractEntry;
                    }, "Signed,PaymentPeriod,Id");
        }

        public IEnumerable<InContractDTO> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var cache = new Dictionary<int, InContractDTO>();
            var dapperExecution = BuildByTemplate<InContractDTO, InContractSqlBuilder>();

            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Signed` AS `Signed`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Effective` AS `Effective`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Expiration` AS `Expiration`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");

            dapperExecution.SqlBuilder.Select("ct.`Id`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorPhone`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorCode`");
            dapperExecution.SqlBuilder.Select("ct.`ContractorAddress`");

            dapperExecution.SqlBuilder.SelectServicePackage("sc");

            // ContractorHTC
            dapperExecution.SqlBuilder.Select("htcc.`Id`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorCode`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorPhone`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorAddress`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorEmail`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorFax`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorTaxIdNo`");
            dapperExecution.SqlBuilder.Select("htcc.`ContractorAddress`");

            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors ct ON ct.Id = t1.ContractorId");

            dapperExecution.SqlBuilder.LeftJoin("Projects pj ON pj.Id = t1.ProjectId AND pj.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackages sc ON sc.InContractId = t1.Id AND sc.IsDeleted = FALSE");

            //dapperExecution.SqlBuilder.LeftJoin(
            //    "InContractServices ics ON ics.InContractId = t1.Id AND ics.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "Contractors AS htcc ON htcc.Id = t1.ContractorHTCId");

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.AgentCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("pj.ProjectName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("pj.ProjectCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("ct.ContractorFullName LIKE @keywords",
                        new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("ct.ContractorCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("ct.ContractorPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            dapperExecution.SqlBuilder.Where("sc.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where($"sc.StatusId IN ({string.Join(',', OutContractServicePackageStatus.ValidStatuses())})");

            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution
                .ExecuteQuery<InContractDTO, ContractTimeLine, ContractorDTO, OutContractServicePackageDTO, ContractorDTO>(
                    (inContract, timeLine, contractor, serviceChannel, htcContractor) =>
                    {
                        if (!cache.TryGetValue(inContract.Id, out var cachedInContract))
                        {
                            cachedInContract = inContract;
                            cachedInContract.TimeLine = timeLine;
                            cachedInContract.Contractor = contractor;
                            cachedInContract.ContractorHTC = htcContractor;

                            var contractTypeName = InContractType.List()
                                .First(t => t.Id == cachedInContract.ContractTypeId)
                                .ToString();
                            cachedInContract.Label = $"{cachedInContract.ContractCode} ({contractTypeName})";

                            cache.Add(inContract.Id, cachedInContract);
                        }

                        if (serviceChannel != null
                            && cachedInContract.ServicePackages.All(s => s.Id != serviceChannel.Id))
                        {
                            cachedInContract.ServicePackages.Add(serviceChannel);
                        }

                        return cachedInContract;
                    }, "Signed,Id,Id,Id")
                .Distinct();
        }

        public IEnumerable<SelectionItem> AutocompleteSimple(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem>();
            dapperExecution.SqlBuilder.Select(
                "CONCAT('Số HĐ: ', t1.ContractCode, ', Khách hàng: ', t2.ContractorFullName, ', Đ/c: ', t2.ContractorAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.InnerJoin("Contractors t2 ON t2.Id = t1.ContractorId");
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.AgentCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    //.OrWhere("t1.AgentContractCode LIKE @keywords", new {keywords = $"%{requestFilterModel.Keywords}%"})
                    .OrWhere("t2.ContractorFullName LIKE @keywords",
                        new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t2.ContractorCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t2.ContractorPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t2.ContractorAddress LIKE @keywords",
                        new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<InContractGridDTO> GetPagedList(RequestFilterModel requestFilterModel)
        {
            var cache = new Dictionary<int, InContractGridDTO>();
            var dapperExecution = BuildByTemplate<InContractGridDTO, InContractSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.Select(
                "EXISTS(SELECT 1 FROM AttachmentFiles WHERE InContractId = t1.Id AND IsDeleted = FALSE LIMIT 1) AS `HasUploadedFiles`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Signed` AS `Signed`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Effective` AS `Effective`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_Expiration` AS `Expiration`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");

            dapperExecution.SqlBuilder.SelectContractor("t2");
            
            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t2 ON t2.Id = t1.ContractorId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("ContractorProperties cp ON cp.ContractorId = t2.Id");

            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field == "contractor.fullName"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "contractor.fullName");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.FullName", propertyFilter);
            }

            //
            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field == "contractType")) //Loại hợp đồng
            {
                dapperExecution.SqlBuilder.Where("t1.ContractTypeId = @contractTypeId",
                    new { contractTypeId = requestFilterModel.Get("contractType") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "TimeLine_RenewPeriod")) //Kì hạn hợp đồng
            {
                dapperExecution.SqlBuilder.Where("t1.TimeLine_RenewPeriod = @timeLineRenewPeriod",
                    new { timeLineRenewPeriod = requestFilterModel.Get("TimeLine_RenewPeriod") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "TimeLine_PaymentPeriod")
            ) //Kì hạn thanh toán
            {
                dapperExecution.SqlBuilder.Where("t1.`TimeLine_PaymentPeriod` = @timeLinePaymentPeriod",
                    new { timeLinePaymentPeriod = requestFilterModel.Get("TimeLine_PaymentPeriod") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "organizationUnit")
            ) //Phòng ban chịu trách nhiệm
            {
                dapperExecution.SqlBuilder.Where("t1.OrganizationUnitId = @organizationUnit",
                    new { organizationUnit = requestFilterModel.Get("organizationUnit") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "signedUser")) //Nhân viên kinh doanh
            {
                //dapperExecution.SqlBuilder.Where("t1.SignedUserName = @signedUser",
                //    new { signedUser = requestFilterModel.Get("signedUser") }); //signedUserName

                dapperExecution.SqlBuilder.Where("t1.SignedUserId = @signedUser",
                    new { signedUser = requestFilterModel.Get("signedUser") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "ContractorCode")
            ) //Mã đối tác/ đại lý
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "ContractorCode");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.ContractorCode", propertyFilter);
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "ContractorPhone")
            ) //SĐT đối tác/ đại lý
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "ContractorPhone");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.ContractorPhone", propertyFilter);
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "ContractorFullName")
            ) //Tên đối tác/ đại lý
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "ContractorFullName");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.ContractorFullName", propertyFilter);
            }

            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field == "ContractStatusName")) //trạng thái hợp đồng
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "ContractStatusName");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t1.ContractStatusId", propertyFilter);
            }

            if (requestFilterModel.Any("groupCustomerId")) //Nhóm khách hàng
            {
                dapperExecution.SqlBuilder.Where("(cp.ContractorGroupIds LIKE @groupCustomerId)",
                    new { groupCustomerId = $"%{requestFilterModel.Get("groupCustomerId")}%" });
            }

            if (requestFilterModel.Any("categoryCustomerId")) //Danh mục khách hàng
            {
                dapperExecution.SqlBuilder.Where("cp.ContractorCategoryId = @categoryCustomerId",
                    new { categoryCustomerId = requestFilterModel.Get("categoryCustomerId") });
            }

            if (requestFilterModel.Any("structureCustomerId")) //Cơ cấu khách hàng
            {
                dapperExecution.SqlBuilder.Where("cp.ContractorStructureId = @structureCustomerId",
                    new { structureCustomerId = requestFilterModel.Get("structureCustomerId") });
            }

            return dapperExecution
                .ExecutePaginateQuery<InContractGridDTO, ContractTimeLine,
                    ContractorDTO>(
                    (contract, contractTimeLine, contractor) =>
                    {
                        if (!cache.TryGetValue(contract.Id, out var contractEntry))
                        {
                            contractEntry = contract;
                            contractEntry.ContractStatusName = ContractStatus.From(contract.ContractStatusId).Name;
                            if (contract.ContractTypeId.HasValue)
                            {
                                contractEntry.ContractTypeName = Enumeration.FromValue<InContractType>(contract.ContractTypeId.Value).ToString();
                            }

                            contractEntry.Contractor = contractor;
                            contractEntry.TimeLine = contractTimeLine;

                            cache.Add(contractEntry.Id, contractEntry);
                        }

                        //if (inContractService != null &&
                        //    contractEntry.InContractServices.All(s => s.ServiceId != inContractService.ServiceId))
                        //{
                        //    contractEntry.InContractServices.Add(inContractService);
                        //}

                        return contractEntry;
                    }, "Signed,Id");
        }

        public IEnumerable<OutContractSharingRevenueDTO> GetOutContractSharingRevenues(int inContractId,
            int? inContractType,int currencyUnitId)
        {
            var cached = new Dictionary<int, OutContractSharingRevenueDTO>();
            return WithConnection(conn =>
                    conn.Query<OutContractSharingRevenueDTO,  ContractSharingRevenueLineDTO, OutContractSharingRevenueDTO
                    >("GetOutContractSharingRevenueByInContract",
                        (outContractSharingRevenue,  sharingRevenueLine) =>
                        {
                            if (outContractSharingRevenue == null)
                            {
                                return null;
                            }

                            if (!cached.TryGetValue(outContractSharingRevenue.OutContractId, out var cachedItem))
                            {
                                cachedItem = outContractSharingRevenue;
                                cached.Add(outContractSharingRevenue.OutContractId, cachedItem);
                            }

                            //if (cachedItem.ServicePackages.All(s => s.Id != contractServicePackage.Id))
                            //{
                            //    contractServicePackage.TimeLine = billingTimeLine;
                            //    cachedItem.ServicePackages.Add(contractServicePackage);
                            //}

                            if (cachedItem.SharingRevenueLines.All(c => c.Id != sharingRevenueLine.Id))
                            {
                                cachedItem.SharingRevenueLines.Add(sharingRevenueLine);
                            }

                            return cachedItem;
                        },
                        new { inContractId, inContractType = inContractType ?? InContractType.InChannelRental.Id,currencyUnitId },
                        commandType: CommandType.StoredProcedure,
                        splitOn: "Id,PaymentPeriod,Id"))
                .Distinct();
        }

        public int GetOrderNumberByNow()
        {
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(1) + 1) FROM InContracts WHERE DATE(CreatedDate) = CURDATE() AND IsDeleted = FALSE"));
        }

        public string GenerateContractCode(int contractType, string contractorFullName, int? marketAreaId)
        {
            var indexNumber = this.GetOrderNumberByNow();
            var contractTypeCode = "DVBTBD";
            if (InContractType.InChannelRental.Id == contractType)
            {
                contractTypeCode = "DVTKT";
            }
            else if (InContractType.InCommission.Id == contractType)
            {
                contractTypeCode = "DVHH";
            }
            else if (InContractType.InSharingRevenue.Id == contractType)
            {
                contractTypeCode = "DVPCDT";
            }

            var marketAreaCodePart = string.Empty;
            if (marketAreaId.HasValue)
            {
                marketAreaCodePart = _marketAreaQueries.Find(marketAreaId.Value)?.MarketCode;
                marketAreaCodePart = string.IsNullOrWhiteSpace(marketAreaCodePart) ? string.Empty : $"/{marketAreaCodePart}";
            }

            var datePart = DateTime.Now.ToString("ddMMyy");
            return $"{datePart}-{indexNumber}{marketAreaCodePart}-{contractTypeCode}/VTQT-{contractorFullName.GetAcronym()}".ToUpper();
        }
    }
}