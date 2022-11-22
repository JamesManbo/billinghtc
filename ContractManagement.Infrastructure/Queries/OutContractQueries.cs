using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.Reports;
using ContractManagement.Utility;
using Dapper;
using GenericRepository;
using GenericRepository.DapperSqlBuilder;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IOutContractQueries : IQueryRepository
    {
        int GetOrderNumberByNow(DateTime? signedDate = null);
        int GetOrderNumberByProject(int projectId);
        int GetLatestId();
        int GetTotalNumber();
        int FindStatusById(int id);
        bool IsCodeExisted(string contractCode, int contractId);
        string GenerateContractCode(bool isEnterprise, string abbreviatedName, int[] srvIds, int? marketAreaId = null, int? projectId = null, string marketAreaCode = "", string projectCode = "",
            DateTime? signedDate = null, string area = "");
        string OutContractCodeById(int contractId);
        OutContractDTO FindById(int id);
        OutContractDTO FindByContractorId(string id);
        OutContractDTO FindByContractCode(string code);
        OutContractDTO FindByChannelId(int channelId);
        OutContractDTO FindByChannelCId(string cId);
        /// <summary>
        /// DEPRECATED: Out of date func
        /// </summary>
        /// <param name="requestFilterModel"></param>
        /// <returns></returns>
        //Task<IEnumerable<OutContractSimpleDTO>> GetSimpleAll(RequestFilterModel requestFilterModel);
        Task<IEnumerable<SelectionItem>> Autocomplete(RequestFilterModel requestFilterModel);
        IEnumerable<SelectionItem> AutocompletePayable(RequestFilterModel requestFilterModel);
        Task<IPagedList<OutContractGridDTO>> GetPagedList(ContactsFilterModel requestFilterModel);
        IEnumerable<OutContractSimpleDTO> GetOutContractSimpleAllByInContractId(int inContractId, int currencyUnitId);
        IEnumerable<int> OutContractIdByIds(List<int> contractIds, int serviceId, int servicePackageId);
        List<OutContractSimpleDTO> GetOutContractSimpleAllByIds(string ids);
        List<OutContractDTO> GetExpired();
        HashSet<string> GetContractCodes();

        Task<int> CountingContractExpirationSoon(bool enterpriseOnly = false);
        Task<int> CountChannelExpirationSoon(int daysBeforeExpiration);
        IEnumerable<ContractStatusReportModel> GetReportContractStatus(ContractStatusReportFilter filter);
    }
    public class OutContractSqlBuilder : SqlBuilder
    {
        public OutContractSqlBuilder()
        {
        }

        public OutContractSqlBuilder(string tableName) : base(tableName) { }

        public void WhereFullTextKeyword(string keywords)
        {
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t1.AgentCode LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t1.AgentContractCode LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t2.ContractorFullName LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t2.ContractorCode LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t2.ContractorPhone LIKE @keywords", new { keywords = $"%{keywords}%" })
                .OrWhere("t2.ContractorAddress LIKE @keywords", new { keywords = $"%{keywords}%" });
            }
        }

        public void SelectTimeLine()
        {
            Select("t1.`TimeLine_Signed` AS `Signed`");
            Select("t1.`TimeLine_Effective` AS `Effective`");
            Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
            Select("t1.`TimeLine_Expiration` AS `Expiration`");
            Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");
        }

        public void SelectServicePackage(string alias)
        {
            Select($"{alias}.`Id` AS `Id`");
            Select($"{alias}.`ProjectId` AS `ProjectId`");
            Select($"{alias}.`StartPointChannelId` AS `StartPointChannelId`");
            Select($"{alias}.`EndPointChannelId` AS `EndPointChannelId`");
            Select($"{alias}.`OutContractId` AS `OutContractId`");
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
            Select($"{alias}.`Type` AS `Type`");
            Select($"{alias}.`TaxAmount` AS `TaxAmount`");
            Select($"{alias}.`SubTotalBeforeTax` AS `SubTotalBeforeTax`");
            Select($"{alias}.`SubTotal` AS `SubTotal`");
            Select($"{alias}.`GrandTotalBeforeTax` AS `GrandTotalBeforeTax`");
            Select($"{alias}.`GrandTotal` AS `GrandTotal`");
            Select($"{alias}.`StatusId` AS `StatusId`");
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
            Select($"{alias}.`IsDefaultSLAByServiceId` AS `IsDefaultSLAByServiceId`");
            Select($"{alias}.`Note` AS `Note`");
            Select($"{alias}.`OtherNote` AS `OtherNote`");
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
            Select($"{alias}.ConnectionPoint AS `ConnectionPoint`");

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
            Select($"{alias}.ActivatedUnit");
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

        public void SelectContractTotalByCurrency(string alias)
        {
            Select($"{alias}.Id AS `Id`");
            Select($"{alias}.CurrencyUnitId AS `CurrencyUnitId`");
            Select($"{alias}.CurrencyUnitCode AS `CurrencyUnitCode`");
            Select($"{alias}.OutContractId AS `OutContractId`");
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

        public OutContractSqlBuilder WhereValidChannel(string alias)
        {
            Where($"{alias}.IsDeleted = FALSE AND {alias}.StatusId IN @outContractServicePackageStatus",
                new
                {
                    outContractServicePackageStatus = OutContractServicePackageStatus.CanBeListedStatuses()
                });

            return this;
        }
    }

    public class OutContractQueries : QueryRepository<OutContract, int>, IOutContractQueries
    {
        private readonly IProjectQueries _projectQueries;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IServicesQueries _servicesQueries;
        public OutContractQueries(ContractDbContext context,
            IProjectQueries projectQueries,
            IMarketAreaQueries marketAreaQueries,
            IServicesQueries servicesQueries) : base(context)
        {
            this._projectQueries = projectQueries;
            this._marketAreaQueries = marketAreaQueries;
            this._servicesQueries = servicesQueries;
        }

        public bool IsCodeExisted(string contractCode, int contractId)
        {
            return WithConnection(conn
                => conn.ExecuteScalar<bool>(
                    "SELECT COUNT(1) FROM OutContracts WHERE TRIM(ContractCode) = @contractCode AND IsDeleted = FALSE AND Id <> @contractId",
                    new
                    {
                        contractId,
                        contractCode = contractCode.Trim()
                    }));
        }

        public OutContractDTO FindById(int id)
        {
            return GetDetail(contractId: id);
        }

        /// <summary>
        /// DEPRECATED: Out of date func
        /// </summary>
        /// <param name="requestFilterModel"></param>
        /// <returns></returns>
        //public Task<IEnumerable<OutContractSimpleDTO>> GetSimpleAll(RequestFilterModel requestFilterModel)
        //{
        //    var cache = new Dictionary<int, OutContractSimpleDTO>();
        //    var dapperExecution = BuildByTemplate<OutContractSimpleDTO>(requestFilterModel);
        //    dapperExecution.SqlBuilder.Select(
        //        "EXISTS(SELECT 1 FROM AttachmentFiles WHERE OutContractId = t1.Id AND IsDeleted = FALSE LIMIT 1) AS `HasUploadedFiles`");

        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_Signed` AS `Signed`");
        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_Effective` AS `Effective`");
        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_Liquidation` AS `Liquidation`");
        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_Expiration` AS `Expiration`");
        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
        //    dapperExecution.SqlBuilder.Select("t1.`TimeLine_RenewPeriod` AS `RenewPeriod`");

        //    dapperExecution.SqlBuilder.Select("t2.`ServiceName` AS `ServiceName`");
        //    dapperExecution.SqlBuilder.Select("t2.`CurrencyUnitId` AS `CurrencyUnitId`");
        //    dapperExecution.SqlBuilder.Select("t2.`CurrencyUnitCode` AS `CurrencyUnitCode`");
        //    dapperExecution.SqlBuilder.Select("t2.`PackageName` AS `PackageName`");
        //    dapperExecution.SqlBuilder.Select("t2.`PackagePrice` AS `PackagePrice`");
        //    dapperExecution.SqlBuilder.Select("t2.`BandwidthLabel` AS `BandwidthLabel`");
        //    dapperExecution.SqlBuilder.Select("t2.`InstallationFee`");
        //    dapperExecution.SqlBuilder.Select("t2.`EquipmentAmount`");
        //    dapperExecution.SqlBuilder.Select("t2.`OtherFee`");
        //    dapperExecution.SqlBuilder.Select("t2.`TaxAmount` AS `TaxAmount`");
        //    dapperExecution.SqlBuilder.Select("t2.`GrandTotalBeforeTax` AS `GrandTotalBeforeTax`");
        //    dapperExecution.SqlBuilder.Select("t2.`GrandTotal` AS `GrandTotal`");

        //    dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_Street` AS `Street`");
        //    dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_District` AS `District`");
        //    dapperExecution.SqlBuilder.Select("t2.`InstallationAddress_City` AS `City`");

        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_PrepayPeriod` AS `PrepayPeriod`");
        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_Signed` AS `Signed`");
        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_Effective` AS `Effective`");
        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_LatestBilling` AS `LatestBilling`");
        //    dapperExecution.SqlBuilder.Select("t2.`TimeLine_NextBilling` AS `NextBilling`");

        //    dapperExecution.SqlBuilder.Select("t3.`Id`");
        //    dapperExecution.SqlBuilder.Select("t3.`ContractorFullName`");
        //    dapperExecution.SqlBuilder.Select("t3.`ContractorPhone`");

        //    dapperExecution.SqlBuilder.InnerJoin(
        //        "OutContractServicePackages AS t2 ON t2.OutContractId = t1.Id AND t2.IsDeleted = FALSE AND t2.StatusId NOT IN (2,3)");// OutContractServicePackageStatus

        //    dapperExecution.SqlBuilder.InnerJoin(
        //        "Contractors AS t3 ON t3.Id = t1.ContractorId AND t3.IsDeleted = FALSE");

        //    dapperExecution.SqlBuilder.OrderBy("t1.Id DESC");

        //    if (requestFilterModel.Any("contractorFullName"))
        //    {
        //        var propertyFilter = requestFilterModel.GetProperty("contractorFullName");
        //        dapperExecution.SqlBuilder.AppendPredicate<string>("t3.ContractorFullName", propertyFilter);
        //    }

        //    if (requestFilterModel.Any("Ids"))
        //    {
        //        var ids =
        //            requestFilterModel.Get("Ids").ToString().SplitToInt(',');
        //        dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
        //    }

        //    return dapperExecution
        //        .ExecuteQueryAsync<OutContractSimpleDTO, ContractTimeLine, ContractPackageSimpleDTO, InstallationAddress,
        //            BillingTimeLine,
        //            ContractorSimpleDTO>(
        //            (contract, contractTimeLine, contractPackage, installAddress, billingTimeLine, contractor) =>
        //            {
        //                if (!cache.TryGetValue(contract.Id, out var contractEntry))
        //                {
        //                    contractEntry = contract;
        //                    contractEntry.Contractor = contractor;
        //                    contractEntry.TimeLine = contractTimeLine;

        //                    cache.Add(contractEntry.Id, contractEntry);
        //                }

        //                contractPackage.TimeLine = billingTimeLine;
        //                contractPackage.InstallationAddress = installAddress;
        //                contractEntry.ServicePackages.Add(contractPackage);

        //                return contractEntry;
        //            }, "Signed,ServiceName,Street,PaymentPeriod,Id");
        //}

        public Task<IEnumerable<SelectionItem>> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem, OutContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', 'Số HĐ: ', t1.ContractCode, ', Khách hàng: ', t2.ContractorFullName, ', Đ/c: ', t2.ContractorAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.InnerJoin("Contractors t2 ON t2.Id = t1.ContractorId");
            dapperExecution.SqlBuilder.WhereFullTextKeyword(requestFilterModel.Keywords);
            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQueryAsync();
        }

        public async Task<IPagedList<OutContractGridDTO>> GetPagedList(ContactsFilterModel requestFilterModel)
        {
            requestFilterModel.RestrictOrderBy = true;
            var cache = new Dictionary<int, OutContractGridDTO>();
            var dapperExecution = BuildByTemplate<OutContractGridDTO, OutContractSqlBuilder>(requestFilterModel);

            #region Select statements
            dapperExecution.SqlBuilder.Select(
                "EXISTS(SELECT 1 FROM AttachmentFiles WHERE OutContractId = t1.Id AND IsDeleted = FALSE LIMIT 1) AS `HasUploadedFiles`");

            dapperExecution.SqlBuilder.Select("t1.`SignedUserName` AS `SignedUserName`");
            dapperExecution.SqlBuilder.Select("t1.`OrganizationUnitName` AS `OrganizationUnitName`");
            dapperExecution.SqlBuilder.Select("t1.`SignedUserId` AS `SignedUserId`");
            dapperExecution.SqlBuilder.Select("t1.`CurrencyUnitId` AS `CurrencyUnitId`");
            dapperExecution.SqlBuilder.Select("t1.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("t1.`OrganizationUnitId` AS `OrganizationUnitId`");

            dapperExecution.SqlBuilder.SelectTimeLine();

            dapperExecution.SqlBuilder.SelectServicePackage("t2");
            dapperExecution.SqlBuilder.Select("pj.ProjectName AS ProjectName");
            dapperExecution.SqlBuilder.Select("t1.`ContractCode` AS `ContractCode`");
            dapperExecution.SqlBuilder.SelectServicePackageTimeLine("t2");
            // Select start point
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");
            // Select end point
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");

            dapperExecution.SqlBuilder.SelectContractor("t5");

            //dapperExecution.SqlBuilder.SelectContractTotalByCurrency("tol");

            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t5 ON t5.Id = t1.ContractorId");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutContractServicePackages AS t2 ON t2.OutContractId = t1.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "Projects AS pj ON pj.Id = t2.ProjectId AND pj.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints csp ON csp.Id = t2.StartPointChannelId"
                );

            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints cep ON cep.Id = t2.EndPointChannelId"
                );

            dapperExecution.SqlBuilder.LeftJoin("ContractorProperties cp ON cp.ContractorId = t5.Id");

            #endregion

            #region Where conditions
            //if (requestFilterModel.Any("currencyUnitId"))
            //{
            //    dapperExecution.SqlBuilder.AppendPredicate<int>("t1.CurrencyUnitId",
            //        requestFilterModel.GetProperty("currencyUnitId"));
            //}
            if (requestFilterModel.Any("timeline_RenewPeriod"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<int>("t1.Timeline_RenewPeriod",
                    requestFilterModel.GetProperties("timeline_RenewPeriod"));
            }

            if (requestFilterModel.Any("timeLine_PaymentPeriod"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<int>("t2.TimeLine_PaymentPeriod",
                    requestFilterModel.GetProperties("timeLine_PaymentPeriod"));
            }

            if (requestFilterModel.Any("timeLine_Signed"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<DateTime>("t2.Timeline_Signed",
                    requestFilterModel.GetProperties("timeLine_Signed"));
            }

            if (requestFilterModel.Any("timeLine_Expiration"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<DateTime>("t1.Timeline_Expiration",
                    requestFilterModel.GetProperties("timeLine_Expiration"));
            }

            if (requestFilterModel.Any("timeLine_Effective"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<DateTime>("t2.timeLine_Effective",
                    requestFilterModel.GetProperties("timeLine_Effective"));
            }

            if (requestFilterModel.Any("timeLine_StartBilling"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<DateTime>("t2.TimeLine_StartBilling",
                    requestFilterModel.GetProperties("timeLine_StartBilling"));
            }

            if (requestFilterModel.Any("timeLine_NextBilling"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<DateTime>("t2.TimeLine_NextBilling",
                    requestFilterModel.GetProperties("timeLine_NextBilling"));
            }

            if (requestFilterModel.Any("contractorFullName"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("t5.ContractorFullName",
                    requestFilterModel.GetProperty("contractorFullName"));
            }

            if (requestFilterModel.Any("contractorPhone"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("t5.ContractorPhone",
                    requestFilterModel.GetProperty("contractorPhone"));
            }

            if (requestFilterModel.Any("contractorCode"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("t5.ContractorCode",
                    requestFilterModel.GetProperty("contractorCode"));
            }

            if (requestFilterModel.Any("Ids"))
            {
                var ids =
                    requestFilterModel.Get("Ids").ToString().SplitToInt(',');
                dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.Where("t2.ServiceId = @serviceId",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            //Loại hợp đồng
            if (requestFilterModel.Any("contractType"))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractTypeId = @contractTypeId",
                    new { contractTypeId = requestFilterModel.Get("contractType") });
            }

            //Dịch vụ 
            if (requestFilterModel.Any("service"))
            {
                dapperExecution.SqlBuilder.Where("t2.ServiceId = @serviceId",
                    new { serviceId = requestFilterModel.Get("service") });
            }

            //Gói cước
            if (requestFilterModel.Any("packageId"))
            {
                dapperExecution.SqlBuilder.Where("t2.ServicePackageId = @packageId",
                    new { packageId = requestFilterModel.Get("packageId") });
            }

            //Phòng ban chịu trách nhiệm
            if (requestFilterModel.Any("organizationUnit"))
            {
                dapperExecution.SqlBuilder.Where("t1.OrganizationUnitId = @organizationUnit",
                    new { organizationUnit = requestFilterModel.Get("organizationUnit") });
            }
            //Nhân viên kinh doanh
            if (requestFilterModel.Any("signedUser"))
            {
                dapperExecution.SqlBuilder.Where("t1.SignedUserId = @signedUser",
                    new { signedUser = requestFilterModel.Get("signedUser") });
            }
            //Băng thông quốc tế
            if (requestFilterModel.Any("internationalBandwidth"))
            {
                var propertyFilter = requestFilterModel.GetProperty("internationalBandwidth");
                dapperExecution.SqlBuilder.AppendPredicate<float>("t2.`InternationalBandwidth`", propertyFilter);
            }
            //Băng thông trong nước
            if (requestFilterModel.Any("domesticBandwidth"))
            {
                var propertyFilter = requestFilterModel.GetProperty("domesticBandwidth");
                dapperExecution.SqlBuilder.AppendPredicate<float>("t2.`DomesticBandwidth`", propertyFilter);
            }

            if (requestFilterModel.Any("agentContractCode"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("t1.AgentContractCode",
                    requestFilterModel.GetProperty("agentContractCode"));
            }

            if (requestFilterModel.Any("cids"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.CId",
                    requestFilterModel.GetProperty("cids"));
            }

            if (requestFilterModel.Any("radiusAccount"))
            {
                var propertyFilter = requestFilterModel.GetProperty("radiusAccount");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.RadiusAccount", propertyFilter);
            }

            if (requestFilterModel.Any("radiusPassword"))
            {
                var propertyFilter = requestFilterModel.GetProperty("radiusPassword");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t2.RadiusPassword", propertyFilter);
            }

            //Giá gói cước
            if (requestFilterModel.Any("packagePrice"))
            {
                var propertyFilter = requestFilterModel.GetProperty("packagePrice");
                dapperExecution.SqlBuilder.AppendPredicate<decimal>("t2.PackagePrice", propertyFilter);
            }

            //Địa chỉ lắp đặt
            if (requestFilterModel.Any("installationAddress"))
            {
                var propertyFilter = requestFilterModel.GetProperty("installationAddress");
                dapperExecution.SqlBuilder.OrWhere("cep.InstallationAddress_City LIKE @city", new { city = $"%{propertyFilter.FilterValue}%" });
                dapperExecution.SqlBuilder.OrWhere("cep.InstallationAddress_District LIKE @district", new { district = $"%{propertyFilter.FilterValue}%" });
                dapperExecution.SqlBuilder.OrWhere("cep.InstallationAddress_Street LIKE @street", new { street = $"%{propertyFilter.FilterValue}%" });
                dapperExecution.SqlBuilder.OrWhere("csp.InstallationAddress_City LIKE @city", new { city = $"%{propertyFilter.FilterValue}%" });
                dapperExecution.SqlBuilder.OrWhere("csp.InstallationAddress_District LIKE @district", new { district = $"%{propertyFilter.FilterValue}%" });
                dapperExecution.SqlBuilder.OrWhere("csp.InstallationAddress_Street LIKE @street", new { street = $"%{propertyFilter.FilterValue}%" });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                var lstProjectIds = requestFilterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("t2.ProjectId IN @projectIds",
                    new { projectIds = lstProjectIds });
            }

            if (requestFilterModel.ProjectId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t2.ProjectId = @projectId",
                    new { projectId = requestFilterModel.ProjectId.Value });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.ServiceIds))
            {
                var serviceIds = requestFilterModel.ServiceIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t2.ServiceId IN @serviceIds)", new { serviceIds = serviceIds })
                    ;
            }

            if (requestFilterModel.ContractorId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.ContractorId = @contractorId",
                    new { contractorId = requestFilterModel.ContractorId.Value });
            }
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.AgentCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.AgentContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            if (requestFilterModel.FromDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.CreatedDate >= @fromDate", new { fromDate = requestFilterModel.FromDate.Value });
            }

            if (requestFilterModel.ToDate.HasValue)
            {
                var toD = requestFilterModel.ToDate.Value;
                dapperExecution.SqlBuilder.Where("t1.CreatedDate < @toDate", new { toDate = toD.AddDays(1) });
            }

            if (requestFilterModel.Any("groupCustomerId")) //Nhóm khách hàng
            {
                dapperExecution.SqlBuilder.Where("FIND_IN_SET(@groupCustomerId, cp.ContractorGroupIds)",
                    new { groupCustomerId = requestFilterModel.Get("groupCustomerId") });
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

            if (requestFilterModel.ContractStatusId.HasValue && requestFilterModel.ContractStatusId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.ContractStatusId = @contractStatusId", new { contractStatusId = requestFilterModel.ContractStatusId });
            }

            dapperExecution.SqlBuilder.WhereValidChannel("t2");

            if (requestFilterModel.OrderBy.EqualsIgnoreCase("timeLine_StartBilling"))
            {
                dapperExecution.SqlBuilder.OrderBy($"t2.TimeLine_StartBilling {requestFilterModel.Dir}");
            }
            else if (requestFilterModel.OrderBy.EqualsIgnoreCase("timeLine_NextBilling"))
            {
                dapperExecution.SqlBuilder.OrderBy($"t2.timeLine_NextBilling {requestFilterModel.Dir}");
            }
            else if (requestFilterModel.OrderBy.EqualsIgnoreCase("timeLine_Expiration"))
            {
                dapperExecution.SqlBuilder.OrderBy($"t1.timeLine_Expiration {requestFilterModel.Dir}");
            }

            #endregion

            var result = await WithConnectionAsync(async conn =>
                await conn.QueryAsync(
                    dapperExecution.ExecutionTemplate.RawSql,
                    new[] {
                        typeof(OutContractGridDTO),
                        typeof(ContractTimeLine),
                        typeof(ContractPackageGridDTO),
                        typeof(BillingTimeLine),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(ContractorDTO),
                        //typeof(ContractTotalByCurrencyDTO)
                    },
                    (results) =>
                    {
                        var contract = results[0] as OutContractGridDTO;
                        var contractTimeLine = results[1] as ContractTimeLine;
                        var contractPackage = results[2] as ContractPackageGridDTO;
                        var billingTimeLine = results[3] as BillingTimeLine;
                        var startPoint = results[4] as OutputChannelPointDTO;
                        var startPointAddress = results[5] as InstallationAddress;
                        var endPoint = results[6] as OutputChannelPointDTO;
                        var endPointAddress = results[7] as InstallationAddress;
                        var contractor = results[8] as ContractorDTO;
                        //var total = results[9] as ContractTotalByCurrencyDTO;

                        if (!cache.TryGetValue(contract.Id, out var contractEntry))
                        {
                            contractEntry = contract;
                            contractEntry.ContractStatusName = ContractStatus.From(contract.ContractStatusId).Name;

                            contractEntry.Contractor = contractor;
                            contractEntry.TimeLine = contractTimeLine;

                            cache.Add(contractEntry.Id, contractEntry);
                        }

                        contractPackage.TimeLine = billingTimeLine;

                        if (startPoint != null)
                        {
                            startPoint.InstallationAddress = startPointAddress;
                            contractPackage.StartPoint = startPoint;
                        }

                        if (endPoint != null)
                        {
                            endPoint.InstallationAddress = endPointAddress;
                            contractPackage.EndPoint = endPoint;
                        }

                        if (contractEntry.ServicePackages.All(o => o.Id != contractPackage.Id))
                        {
                            contractEntry.ServicePackages.Add(contractPackage);
                            contractEntry.ProjectName =
                                contractEntry.ProjectName.JoinUnique(contractPackage.ProjectName);
                        }
                        //if (contractEntry.ContractTotalByCurrencies.All(t => t.Id != total.Id))
                        //    contractEntry.ContractTotalByCurrencies.Add(total);

                        return contractEntry;
                    },
                    dapperExecution.ExecutionTemplate.Parameters,
                    null,
                    true,
                    "Signed,Id,PaymentPeriod,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id"));

            var totalRecords = await dapperExecution.ExecuteTotalRecordsQueryAsync();
            return new PagedList<OutContractGridDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = result?.Distinct()?.ToList()
            };
        }

        public IEnumerable<OutContractSimpleDTO> GetOutContractSimpleAllByInContractId(int inContractId, int currencyUnitId)
        {
            var cached = new Dictionary<int, OutContractSimpleDTO>();
            //var validChannelStatuses = OutContractServicePackageStatus.ValidStatuses();
            return WithConnection(conn => conn.Query("GetOutContractSimpleAllByInContractId",
                new[]
                {
                    typeof(OutContractSimpleDTO),
                    typeof(ContractorSimpleDTO),
                    typeof(ContractPackageSimpleDTO),
                    typeof(BillingTimeLine),
                    typeof(OutputChannelPointSimpleDTO),
                    typeof(InstallationAddress),
                    typeof(OutputChannelPointSimpleDTO),
                    typeof(InstallationAddress),
                    typeof(ContractTotalByCurrencyDTO)
                }, results =>
                {
                    var contractEntry = results[0] as OutContractSimpleDTO;
                    if (contractEntry == null) return null;

                    if (!cached.TryGetValue(contractEntry.Id, out var result))
                    {
                        result = contractEntry;
                        cached.Add(contractEntry.Id, contractEntry);
                    }

                    result.Contractor = results[1] as ContractorSimpleDTO;

                    var channel = results[2] as ContractPackageSimpleDTO;
                    channel.TimeLine = results[3] as BillingTimeLine;

                    var existChannel = contractEntry.ServicePackages.FirstOrDefault(c => c.Id == channel.Id);
                    if (existChannel == null)
                    {
                        result.ServicePackages.Add(channel);
                    }
                    else
                    {
                        existChannel = channel;
                    }

                    var channelStartPoint = results[4] as OutputChannelPointSimpleDTO;
                    if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                    if (channel.StartPoint != null)
                    {
                        channel.StartPoint.InstallationAddress = results[5] as InstallationAddress;
                    }

                    var channelEndPoint = results[6] as OutputChannelPointSimpleDTO;
                    if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                    if (channel.EndPoint != null)
                    {
                        channel.EndPoint.InstallationAddress = results[7] as InstallationAddress;
                    }

                    var contractTotal = results[8] as ContractTotalByCurrencyDTO;
                    if (contractEntry.ContractTotalByCurrencies.All(s => s.Id != contractTotal.Id))
                    {
                        contractEntry.ContractTotalByCurrencies.Add(contractTotal);
                    }

                    return result;
                },
                new { inContractId, currencyUnitId }, //, validChannelStatuses },
                null,
                true,
                "Id,Id,PaymentPeriod,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id",
                null,
                CommandType.StoredProcedure)).Distinct().ToList();
        }

        public List<OutContractSimpleDTO> GetOutContractSimpleAllByIds(string ids)
        {
            var cached = new Dictionary<int, OutContractSimpleDTO>();
            return WithConnection(conn => conn.Query("GetOutContractSimpleAllByIds",
                new[]
                {
                    typeof(OutContractSimpleDTO),
                    typeof(ContractorSimpleDTO),
                    typeof(ContractTotalByCurrencyDTO)
                }, results =>
                {
                    var contractEntry = results[0] as OutContractSimpleDTO;
                    if (contractEntry == null) return null;

                    if (!cached.TryGetValue(contractEntry.Id, out var result))
                    {
                        result = contractEntry;
                        cached.Add(contractEntry.Id, contractEntry);
                    }

                    result.Contractor = results[1] as ContractorSimpleDTO;

                    var contractTotal = results[2] as ContractTotalByCurrencyDTO;

                    if (contractEntry.ContractTotalByCurrencies.All(c => c.Id != contractTotal.Id))
                    {
                        contractEntry.ContractTotalByCurrencies.Add(contractTotal);
                    }

                    return result;
                }, new { ids },
                null,
                true,
                "Id,Id",
                null,
                CommandType.StoredProcedure)).Distinct().ToList();
        }

        public List<OutContractDTO> GetExpired()
        {
            var cache = new Dictionary<int, OutContractDTO>();
            return WithConnection(conn =>
                    conn.Query<OutContractDTO, PaymentMethod, ContractTimeLine, ContractorDTO, OutContractDTO
                    >(
                        "GetExpiredOutContract",
                        (outContract, paymentMethod, contractTimeLine, contractor) =>
                        {
                            if (!cache.TryGetValue(outContract.Id, out var result))
                            {
                                result = outContract;
                                cache.Add(outContract.Id, result);
                            }

                            result.Payment = paymentMethod;
                            result.TimeLine = contractTimeLine;
                            result.Contractor = contractor;
                            return result;
                        },
                        commandType: CommandType.StoredProcedure,
                        splitOn: "Form,RenewPeriod,Id"))
                .Distinct()
                .ToList();
        }

        public Task<int> CountingContractExpirationSoon(bool enterpriseOnly = false)
        {
            return WithConnectionAsync<int>(conn =>
                conn.QueryFirst<int>($"SELECT COUNT(1) FROM OutContracts" +
                    $" WHERE {(enterpriseOnly ? "ContractTypeId = " + OutContractType.Enterprise.Id.ToString() : "1 = 1")}" +
                    $" AND DATE(TimeLine_Expiration) <= DATE(DATE_ADD(CURDATE(), INTERVAL 1 MONTH))"
                ));
        }

        public int GetOrderNumberByNow(DateTime? signedDate = null)
        {
            if (!signedDate.HasValue)
            {
                signedDate = DateTime.UtcNow;
            }

            signedDate = signedDate.Value.AddHours(7);
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(1) + 1) FROM OutContracts WHERE DATE(CreatedDate) = Date(@signedDate)",
                    new { signedDate }));
        }
        public int GetOrderNumberByProject(int projectId)
        {
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(DISTINCT t1.Id) + 1) FROM OutContracts t1" +
                    " INNER JOIN OutContractServicePackages t2 ON t1.Id = t2.OutContractId" +
                    " WHERE t2.ProjectId = @projectId",
                    new { projectId }));
        }
        private OutContractDTO GetDetail(int? contractId = null, string contractorId = null, string contractCode = null, int? channelId = null, string cId = null)
        {
            var cached = new Dictionary<int, OutContractDTO>();
            // 0
            var dapperExecution = BuildByTemplate<OutContractDTO, OutContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t1.`IsControlUsageCapacity` AS `IsControlUsageCapacity`");
            // 1
            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");
            dapperExecution.SqlBuilder.Select("t1.`IsIncidentControl` AS `IsIncidentControl`");
            //2
            dapperExecution.SqlBuilder.SelectTimeLine();

            //3: Select contractor
            dapperExecution.SqlBuilder.SelectContractor("t4");

            //4: Select the out contract packages
            dapperExecution.SqlBuilder.SelectServicePackage("t2");
            dapperExecution.SqlBuilder.Select("pj.ProjectName");
            dapperExecution.SqlBuilder.Select("t1.`ContractCode` AS `ContractCode`");
            dapperExecution.SqlBuilder.Select("cg.ChannelGroupName");
            dapperExecution.SqlBuilder.Select("fpt.Name AS PricingTypeName");
            dapperExecution.SqlBuilder.Select("t2.`PromotionAmount` AS `PromotionAmount`");

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

            //12:
            dapperExecution.SqlBuilder.Select("af.Id");
            dapperExecution.SqlBuilder.Select("af.Name");
            dapperExecution.SqlBuilder.Select("af.OutContractId");
            dapperExecution.SqlBuilder.Select("af.FileName");
            dapperExecution.SqlBuilder.Select("af.FilePath");
            dapperExecution.SqlBuilder.Select("af.Size");
            dapperExecution.SqlBuilder.Select("af.FileType");
            dapperExecution.SqlBuilder.Select("af.Extension");
            dapperExecution.SqlBuilder.Select("af.RedirectLink");

            //13: Contact info
            dapperExecution.SqlBuilder.Select("t5.Id");
            dapperExecution.SqlBuilder.Select("t5.OutContractId");
            dapperExecution.SqlBuilder.Select("t5.Name");
            dapperExecution.SqlBuilder.Select("t5.PhoneNumber");
            dapperExecution.SqlBuilder.Select("t5.Email");

            //14: OutContractServicePackageTaxes
            dapperExecution.SqlBuilder.Select("t12.OutContractServicePackageId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryId");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryName");
            dapperExecution.SqlBuilder.Select("t12.TaxCategoryCode");
            dapperExecution.SqlBuilder.Select("t12.TaxValue");

            //15: ContractorHTC
            dapperExecution.SqlBuilder.Select("t13.`Id`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorFullName`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorCode`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorPhone`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorAddress`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorEmail`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorFax`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorTaxIdNo`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorAddress`");
            dapperExecution.SqlBuilder.Select("t13.`ContractorShortName`");

            //16: ContractContent
            dapperExecution.SqlBuilder.Select("ct.`Id`");
            dapperExecution.SqlBuilder.Select("ct.`Content`");
            dapperExecution.SqlBuilder.Select("ct.`ContractFormId`");

            //17: DigitalSignature
            dapperExecution.SqlBuilder.Select("pic1.*");

            //18: ContractFormSignature
            dapperExecution.SqlBuilder.Select("pic2.*");
            //21: paymenttarget
            dapperExecution.SqlBuilder.SelectContractor("pt");

            dapperExecution.SqlBuilder.SelectContractTotalByCurrency("tol");

            //22: Channel price bus table
            dapperExecution.SqlBuilder.Select("bt.`Id` AS `Id`");
            dapperExecution.SqlBuilder.Select("bt.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("bt.`ChannelId` AS `ChannelId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFrom` AS `UsageValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFromUomId` AS `UsageValueFromUomId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueTo` AS `UsageValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueToUomId` AS `UsageValueToUomId`");
            dapperExecution.SqlBuilder.Select("bt.`BasedPriceValue` AS `BasedPriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceValue` AS `PriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceUnitUomId` AS `PriceUnitUomId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueFrom` AS `UsageBaseUomValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueTo` AS `UsageBaseUomValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`IsDomestic` AS `IsDomestic`");

            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t4 ON t4.Id = t1.ContractorId");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutContractServicePackages t2 ON t1.Id = t2.OutContractId");

            dapperExecution.SqlBuilder.LeftJoin(
                "Projects pj ON pj.Id = t2.ProjectId");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints cep ON cep.Id = t2.EndPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints csp ON csp.Id = t2.StartPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ChannelPriceBusTables bt ON bt.ChannelId = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContractEquipments spce ON t2.StartPointChannelId = spce.OutputChannelPointId AND spce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContractEquipments epce ON t2.EndPointChannelId = epce.OutputChannelPointId AND epce.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON t1.Id = af.OutContractId AND af.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ContactInfos t5 ON t1.Id = t5.OutContractId AND t5.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackageTaxes t12 ON t2.Id = t12.OutContractServicePackageId");

            dapperExecution.SqlBuilder.InnerJoin(
                "Contractors AS t13 ON t13.Id = t1.ContractorHTCId AND t13.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.InnerJoin(
                "ContractTotalByCurrencies tol ON tol.OutContractId = t1.Id");

            //dapperExecution.SqlBuilder.LeftJoin("PromotionForContracts pfc ON pfc.OutContractServicePackageId = t2.Id AND pfc.IsDeleted = FALSE");
            //dapperExecution.SqlBuilder.LeftJoin("PromotionDetails pmd ON pmd.Id = pfc.PromotionDetailId  AND pmd.IsDeleted = FALSE");
            //dapperExecution.SqlBuilder.LeftJoin("Promotions pm ON pm.Id = pmd.PromotionId AND  pm.IsDeleted = FALSE");
            //dapperExecution.SqlBuilder.LeftJoin("PromotionTypes pmt ON pmt.Id = pm.PromotionType AND pmt.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("ContractContents ct ON ct.OutContractId = t1.Id AND ct.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("Pictures pic1 ON pic1.Id = ct.DigitalSignatureId AND pic1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("Pictures pic2 ON pic2.Id = ct.ContractFormSignatureId AND pic2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("ChannelGroups cg ON cg.Id = t2.ChannelGroupId AND  cg.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("Contractors pt ON pt.Id = t2.PaymentTargetId AND  pt.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("FlexiblePricingType AS fpt ON fpt.Id = t2.FlexiblePricingTypeId");

            if (contractId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @contractId", new { contractId });
            }
            else if (!string.IsNullOrEmpty(contractorId))
            {
                dapperExecution.SqlBuilder.Where("pt.IdentityGuid = @contractorId", new { contractorId });
            }
            else if (!string.IsNullOrEmpty(contractCode))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode = @contractCode", new { contractCode });
            }
            else if (channelId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t2.Id = @channelId", new { channelId });
            }
            else if (!string.IsNullOrWhiteSpace(cId))
            {
                dapperExecution.SqlBuilder.Where("LOWER(t2.CId) = LOWER(@cId)", new { cId });
            }

            dapperExecution.SqlBuilder.WhereValidChannel("t2");

            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(OutContractDTO), // 0
                    typeof(PaymentMethod), // 1
                    typeof(ContractTimeLine), // 2
                    typeof(ContractorDTO), // 3
                    typeof(OutContractServicePackageDTO), // 4
                    typeof(BillingTimeLine), // 5
                    typeof(OutputChannelPointDTO), // 6
                    typeof(InstallationAddress), // 7
                    typeof(OutputChannelPointDTO), // 8
                    typeof(InstallationAddress), // 9
                    typeof(ContractEquipmentDTO), // 10
                    typeof(ContractEquipmentDTO), // 11
                    typeof(AttachmentFileDTO), // 12
                    typeof(ContactInfoDTO), // 13
                    typeof(OutContractServicePackageTaxDTO), // 14
                    typeof(ContractorDTO), // 15
                    typeof(ContractContentDTO), // 16
                    typeof(PictureDTO), // 17
                    typeof(PictureDTO), // 18
                    typeof(ContractorDTO), // 19
                    typeof(ContractTotalByCurrencyDTO), // 20
                    typeof(ChannelPriceBusTableDTO), // 21
                }, results =>
                {
                    var contractEntry = results[0] as OutContractDTO;
                    if (contractEntry == null) return null;

                    if (!cached.TryGetValue(contractEntry.Id, out var result))
                    {
                        result = contractEntry;
                        cached.Add(contractEntry.Id, contractEntry);
                    }

                    result.Payment = results[1] as PaymentMethod;
                    result.TimeLine = results[2] as ContractTimeLine;
                    result.Contractor = results[3] as ContractorDTO;
                    result.ContractorHTC = results[15] as ContractorDTO;
                    result.ContractContent = results[16] as ContractContentDTO;

                    if (result.ContractContent != null)
                    {
                        result.ContractContent.DigitalSignature = results[17] as PictureDTO;
                        result.ContractContent.ContractFormSignature = results[18] as PictureDTO;
                    }

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
                        channel.PaymentTarget = results[19] as ContractorDTO;

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

                        if (results[14] is OutContractServicePackageTaxDTO outSrvPckTax)
                        {
                            if (outSrvPckTax != null &&
                                channel.OutContractServicePackageTaxes.All(s => s.TaxCategoryId != outSrvPckTax.TaxCategoryId))
                            {
                                channel.OutContractServicePackageTaxes.Add(outSrvPckTax);
                            }
                        }

                        if (results[21] is ChannelPriceBusTableDTO channelPrice)
                        {
                            if (channel.PriceBusTables.All(s => s.Id != channelPrice.Id))
                                channel.PriceBusTables.Add(channelPrice);
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

                    if (results[20] is ContractTotalByCurrencyDTO contractTotal)
                    {
                        if (result.ContractTotalByCurrencies.All(s => s.Id != contractTotal.Id))
                            result.ContractTotalByCurrencies.Add(contractTotal);
                    }

                    return result;

                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                "Form,Signed,Id,Id,PaymentPeriod,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id,Id,Id,Id,OutContractServicePackageId,Id,Id,Id,Id,Id,Id,Id"));

            return cached.Values.FirstOrDefault();
        }
        public OutContractDTO FindByContractorId(string id)
        {
            return GetDetail(contractorId: id);
        }
        public OutContractDTO FindByContractCode(string code)
        {
            return GetDetail(contractCode: code);
        }
        public IEnumerable<SelectionItem> AutocompletePayable(RequestFilterModel requestFilterModel)
        {
            var payableContractStatuses = new int[]
            {
                ContractStatus.Draft.Id,
                ContractStatus.Signed.Id
            };

            var payableSrvPackageStatuses = OutContractServicePackageStatus.ValidStatuses();

            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem, OutContractSqlBuilder>();

            dapperExecution.SqlBuilder.Select(
                "DISTINCT CONCAT_WS('', 'Số HĐ: ', t1.ContractCode, ', Khách hàng: ', t2.ContractorFullName, ', Đ/c: ', t2.ContractorAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.InnerJoin("Contractors t2 ON t2.Id = t1.ContractorId");
            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages t3 ON t1.Id = t3.OutContractId");

            dapperExecution.SqlBuilder.Where("t1.ContractStatusId IN @payableContractStatuses", new { payableContractStatuses });

            dapperExecution.SqlBuilder.Where("t3.IsDeleted = FALSE " +
                "AND t3.StatusId IN @payableSrvPackageStatuses " +
                "AND t3.TimeLine_NextBilling IS NOT NULL", new { payableSrvPackageStatuses });

            dapperExecution.SqlBuilder.WhereFullTextKeyword(requestFilterModel.Keywords);
            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQuery();
        }
        public string GenerateContractCode(bool isEnterprise,
            string abbreviatedName,
            int[] srvIds,
            int? marketAreaId = null,
            int? projectId = null,
            string marketAreaCode = "",
            string projectCode = "",
            DateTime? signedDate = null,
            string area = "")
        {
            signedDate = signedDate ?? DateTime.UtcNow.AddHours(7);
            var servicesPart = string.Empty;
            bool includeFTTHService = false;
            if (srvIds.Length > 0)
            {
                var serviceCodes = _servicesQueries.FindByIds(srvIds).Distinct();
                servicesPart = string.Join("-", serviceCodes).ToUpper();
                servicesPart = string.IsNullOrWhiteSpace(servicesPart) ? string.Empty : $"{servicesPart}/";
                includeFTTHService = servicesPart.Contains("FTTH");
            }

            var datePrefix = signedDate.Value.ToString("ddMMyy");
            var typePrefix = isEnterprise ? "DRDN" : "DRCN";

            string marketAreaCodePart;
            if (string.IsNullOrEmpty(marketAreaCode) && marketAreaId.HasValue)
            {
                marketAreaCodePart = _marketAreaQueries.Find(marketAreaId.Value)?.MarketCode;
            }
            else
            {
                marketAreaCodePart = marketAreaCode;
            }

            if (projectId.HasValue && includeFTTHService)
            {
                datePrefix = DateTime.Now.ToString("yy");
                string projectCodePart;
                if (string.IsNullOrEmpty(projectCode))
                {
                    projectCodePart = _projectQueries.Find(projectId.Value)?.ProjectCode ?? string.Empty;
                }
                else
                {
                    projectCodePart = projectCode;
                }
                marketAreaCodePart = string.IsNullOrWhiteSpace(marketAreaCodePart) ? string.Empty : $"{marketAreaCodePart}/";
                var indexByProject = this.GetOrderNumberByProject(projectId.Value);
                return $"{datePrefix}-{indexByProject.ToString("D5")}/{marketAreaCodePart}{projectCodePart}".ToUpper();
            }
            var indexByDay = this.GetOrderNumberByNow(signedDate);

            marketAreaCodePart = string.IsNullOrWhiteSpace(marketAreaCodePart) ? string.Empty : $"/{marketAreaCodePart}";
            return $"{datePrefix}-{indexByDay}{marketAreaCodePart}-{typePrefix}/{servicesPart}VTQT{(string.IsNullOrEmpty(abbreviatedName) ? "" : $"-{abbreviatedName}")}{(string.IsNullOrEmpty(area) ? "" : "-" + area)}".ToUpper();
        }

        public int FindStatusById(int id)
        {
            return WithConnection<int>(conn => conn.ExecuteScalar<int>(
                    "SELECT ContractStatusId FROM OutContracts WHERE IsDeleted = FALSE AND Id = @id",
                    new
                    {
                        id
                    }));
        }
        public string OutContractCodeById(int contractId)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<string, OutContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t1.ContractCode");
            dapperExecution.SqlBuilder.Where("t1.Id = @contractId", new { contractId });
            return dapperExecution.ExecuteScalarQuery();
        }
        public IEnumerable<int> OutContractIdByIds(List<int> contractIds, int ServiceId, int ServicePackageId)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int, OutContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages t2 ON t1.Id = t2.OutContractId AND t2.IsDeleted = FALSE " +
                "AND t2.StatusId = @outContractServicePackageStatus AND t2.ServiceId = @serviceId AND t2.ServicePackageId = @servicePackageId"
                , new { outContractServicePackageStatus = OutContractServicePackageStatus.Developed.Id, serviceId = ServiceId, servicePackageId = ServicePackageId });
            dapperExecution.SqlBuilder.Where("t1.Id IN @contractIds", new { contractIds });
            dapperExecution.SqlBuilder.Where("t1.ContractStatusId NOT IN @contractStatuses", new { contractStatuses = new[] { ContractStatus.Liquidated.Id, ContractStatus.Cancelled.Id } });
            return dapperExecution.ExecuteQuery();
        }

        public int GetLatestId()
        {
            return WithConnection<int>(conn => conn.ExecuteScalar<int>(
                   "SELECT Id FROM OutContracts ORDER BY Id DESC LIMIT 1"));
        }

        public int GetTotalNumber()
        {
            return WithConnection(conn =>
                conn.ExecuteScalar<int>("SELECT COUNT(1) FROM OutContracts WHERE IsDeleted = FALSE"));
        }

        public HashSet<string> GetContractCodes()
        {
            return WithConnection(conn =>
                conn.Query<string>("SELECT ContractCode FROM OutContracts")
                ).ToHashSet();
        }
        public OutContractDTO FindByChannelId(int channelId)
        {
            return GetDetail(channelId: channelId);
        }

        public OutContractDTO FindByChannelCId(string cId)
        {
            return GetDetail(cId: cId);
        }

        public IEnumerable<ContractStatusReportModel> GetReportContractStatus(ContractStatusReportFilter filter)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<ContractStatusReportModel, OutContractSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t1.ContractStatusId");
            dapperExecution.SqlBuilder.Select("Count(t1.Id) as Amount");
            dapperExecution.SqlBuilder.GroupBy("t1.ContractStatusId");
            if (filter.FromDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.CreatedDate >= @fromDate", new { fromDate = filter.FromDate.Value });
            }

            if (filter.ToDate.HasValue)
            {
                var toD = filter.ToDate.Value;
                dapperExecution.SqlBuilder.Where("t1.CreatedDate < @toDate", new { toDate = toD.AddDays(1) });
            }
            var lstReport = dapperExecution.ExecuteQuery();
            if (lstReport != null && lstReport.Any())
            {
                foreach (ContractStatusReportModel model in lstReport)
                {
                    model.ContractStatusName = ContractStatus.From(model.ContractStatusId).Name;
                }
            }
            return lstReport;
        }

        public async Task<int> CountChannelExpirationSoon(int daysBeforeExpiration)
        {
            return await WithConnectionAsync<int>(conn =>
                conn.ExecuteScalar<int>("SELECT COUNT(1) FROM OutContractServicePackages " +
                "WHERE OutContractId IS NOT NULL " +
                "AND TimeLine_NextBilling IS NOT NULL " +
                "AND DATE(TimeLine_NextBilling) > CURDATE() " +
                "AND DATE(TimeLine_NextBilling) <= DATE(DATE_ADD(NOW(), INTERVAL @numberOfDays DAY)) " +
                "AND StatusId IN @validStatus",
                new
                {
                    numberOfDays = daysBeforeExpiration,
                    validStatus = OutContractServicePackageStatus.ValidStatuses()
                }
                ));
        }
    }
}