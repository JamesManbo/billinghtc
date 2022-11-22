using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Events.DebtEvents;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using Dapper;
using GenericRepository;
using GenericRepository.DapperSqlBuilder;
using Global.Models.Filter;
using Global.Models.PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Queries
{
    public class ChannelRequestFilterModel : RequestFilterModel
    {
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public int? ProjectId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public int[] StatusIds { get; set; }
        public int? ReclaimEquipmentTypeId { get; set; }
    }
    public interface IOutContractServicePackageQueries : IQueryRepository
    {
        Task<IPagedList<OutContractServicePackageDTO>> GetPagedList(ChannelRequestFilterModel requestFilterModel);
        OutContractServicePackageDTO FindById(int id);
        IEnumerable<OutContractServicePackageDTO> GetAllByOutContractIds(string ids, int? statusId = null);
        IEnumerable<OutContractServicePackageDTO> GetAllNotYetTerminateByOutContractIds(string ids, int currencyUnitId = 0);
        IEnumerable<OutContractServicePackageDTO> GetAllNotAvailableStartAndEndPointByOutContractIds(string ids);
        List<OutContractServicePackageIntegrationEvent> GetOutContractServicePackageEventByTransactionIds(int[] transactionIds, bool isRestore);
        IEnumerable<OutContractServicePackageDTO> GetAllAvailableByIds(string ids);
        IEnumerable<OutContractServicePackageDTO> GetAllOutChannels();
        IEnumerable<ChannelAddressModel> GetChannelAddresses(string[] channelCids);
        public IEnumerable<string> GetSelectionList();
        int GetChannelIndexOfCustomer(string contractorGuid);
        Task<bool> IsRadiusAccountExisted(string radiusUserName, int channelId = 0);
        Task<bool> IsCIdExisted(string cId, int channelId = 0);
    }

    public class ChannelSqlBuilder : SqlBuilder
    {
        public ChannelSqlBuilder(string tableName) : base(tableName)
        {
        }

        public ChannelSqlBuilder()
        {
        }

        public void SelectOutputChannel(string alias)
        {
            Select($"IFNULL({alias}.Id, 0) AS `Id`");
            Select($"{alias}.CurrencyUnitId AS `CurrencyUnitId`");
            Select($"{alias}.CurrencyUnitCode AS `CurrencyUnitCode`");
            Select($"{alias}.PointType AS `PointType`");
            Select($"{alias}.InstallationFee AS `InstallationFee`");
            Select($"{alias}.OtherFee AS `OtherFee`");
            Select($"{alias}.MonthlyCost AS `MonthlyCost`");
            Select($"{alias}.EquipmentAmount AS `EquipmentAmount`");
        }

        public void SelectChannelInstallAddress(string alias)
        {
            Select("'InstallationAddressSpliter' AS `Id`");
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
        public void SelectChannelTimeLine(string alias)
        {
            Select($"IFNULL({alias}.`TimeLine_PaymentPeriod`, 0) AS `PaymentPeriod`");
            Select($"{alias}.`TimeLine_PrepayPeriod` AS `PrepayPeriod`");
            Select($"{alias}.`TimeLine_Signed` AS `Signed`");
            Select($"{alias}.`TimeLine_Effective` AS `Effective`");
            Select($"{alias}.`TimeLine_LatestBilling` AS `LatestBilling`");
            Select($"{alias}.`TimeLine_NextBilling` AS `NextBilling`");
            Select($"{alias}.`TimeLine_StartBilling` AS `StartBilling`");
        }
        public void SelectEquipment(string alias)
        {
            Select($"IFNULL({alias}.Id, 0) AS `Id`");
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

    }

    public class OutContractServicePackageQueries
        : QueryRepository<OutContractServicePackage, int>,
        IOutContractServicePackageQueries
    {
        private readonly ITransactionServicePackageQueries _transactionServicePackageQueries;
        public OutContractServicePackageQueries(ContractDbContext context, ITransactionServicePackageQueries transactionServicePackageQueries) : base(context)
        {
            _transactionServicePackageQueries = transactionServicePackageQueries;
        }

        private IEnumerable<OutContractServicePackageDTO> GetByArrayIds(string outContractIds = "", string channelIds = "", int? currencyUnitId = 0, int? statusId = null, bool? hasStartPoint = null)
        {
            var cached = new Dictionary<int, OutContractServicePackageDTO>();
            var dapperExecution = BuildByTemplate<OutContractServicePackageDTO, ChannelSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t2.ContractCode");
            dapperExecution.SqlBuilder.Select("t2.ContractTypeId AS ContractType");
            dapperExecution.SqlBuilder.Select("t2.ContractorId");

            dapperExecution.SqlBuilder.SelectChannelTimeLine("t1");
            // Select start point
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("csp");
            dapperExecution.SqlBuilder.SelectEquipment("spce");
            //// Select end point
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("cep");
            dapperExecution.SqlBuilder.SelectEquipment("epce");

            dapperExecution.SqlBuilder.InnerJoin("OutContracts as t2 ON t1.OutContractId  = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints csp ON csp.Id = t1.StartPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin("ContractEquipments spce ON csp.Id = spce.OutputChannelPointId");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints cep ON cep.Id = t1.EndPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin("ContractEquipments epce ON cep.Id = epce.OutputChannelPointId");

            #region Build where clauses 
            if (!string.IsNullOrEmpty(outContractIds))
            {
                string sql = $"t1.OutContractId IN ({outContractIds.Trim()})";
                dapperExecution.SqlBuilder.Where(sql);
            }

            if (!string.IsNullOrEmpty(channelIds))
            {
                dapperExecution.SqlBuilder.Where(@$"t1.Id IN ({channelIds.Trim()})");
            }

            var sqlCurrency = $"({currencyUnitId} = 0 OR t1.CurrencyUnitId = {currencyUnitId})";
            dapperExecution.SqlBuilder.Where(sqlCurrency);

            if (statusId.HasValue)
            {
                if (statusId == OutContractServicePackageStatus.Developed.Id)
                {
                    var statusIds = new int[] { OutContractServicePackageStatus.Terminate.Id,
                    OutContractServicePackageStatus.Suspend.Id,
                    OutContractServicePackageStatus.Replaced.Id,
                    OutContractServicePackageStatus.UpgradeBandwidths.Id};
                    dapperExecution.SqlBuilder.Where("(t1.StatusId NOT IN @statusIds)", new { statusIds });
                }
                else
                {
                    dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId", new { statusId });
                }
            }
            else
            {
                var status = @"t1.StatusId NOT IN (" + OutContractServicePackageStatus.Terminate.Id
                    + " ," + OutContractServicePackageStatus.Replaced.Id
                    + " ," + OutContractServicePackageStatus.UpgradeBandwidths.Id
                    + ")";
                dapperExecution.SqlBuilder.Where(status);
            }

            if (hasStartPoint.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.HasStartAndEndPoint = @hasStartPoint", new
                {
                    hasStartPoint
                });
            }
            #endregion

            return WithConnection(conn =>
                conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(OutContractServicePackageDTO),
                    typeof(BillingTimeLine),

                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress),

                    typeof(ContractEquipmentDTO),

                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress),

                    typeof(ContractEquipmentDTO)
                },
                (results) =>
                {
                    var channel = results[0] as OutContractServicePackageDTO;
                    var billingTimeLine = results[1] as BillingTimeLine;
                    var channelStartPoint = results[2] as OutputChannelPointDTO;
                    var startPointInstallAddress = results[3] as InstallationAddress;
                    var startPointEquip = results[4] as ContractEquipmentDTO;

                    var channelEndPoint = results[5] as OutputChannelPointDTO;
                    var endPointInstallAddress = results[6] as InstallationAddress;
                    var endPointEquip = results[7] as ContractEquipmentDTO;

                    if (cached.TryGetValue(channel.Id, out var channelEntity))
                    {
                        channel = channelEntity;
                    }
                    else
                    {
                        channel.TimeLine = billingTimeLine;
                        cached.Add(channel.Id, channel);
                    }

                    if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;

                    if (channel.StartPoint != null)
                    {
                        channel.StartPoint.InstallationAddress = startPointInstallAddress;
                        if (startPointEquip != null && startPointEquip.Id > 0)
                        {
                            startPointEquip.ChannelCId = channel.CId;
                            startPointEquip.OutputChannelPointId = channel.Id;
                            startPointEquip.InstallationFullAddress = channel.StartPoint.InstallationAddress.FullAddress;
                            if (channel.StartPoint.Equipments.All(s => s.Id != startPointEquip.Id))
                            {
                                channel.StartPoint.Equipments.Add(startPointEquip);
                            }
                        }
                    }

                    if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                    if (channel.EndPoint != null)
                    {
                        channel.EndPoint.InstallationAddress = endPointInstallAddress;
                        if (endPointEquip != null && endPointEquip.Id > 0)
                        {
                            endPointEquip.ChannelCId = channel.CId;
                            endPointEquip.OutputChannelPointId = channel.Id;
                            endPointEquip.InstallationFullAddress = channel.EndPoint.InstallationAddress.FullAddress;
                            if (channel.EndPoint.Equipments.All(s => s.Id != endPointEquip.Id))
                            {
                                channel.EndPoint.Equipments.Add(endPointEquip);
                            }
                        }
                    }

                    return channel;
                }, splitOn: "PaymentPeriod,Id,Id,Id,Id,Id,Id")
                .Distinct()//, InstallationPointSpliter,Id") 
            );
        }

        public IEnumerable<OutContractServicePackageDTO> GetAllByOutContractIds(string ids, int? statusId = null)
        {
            return this.GetByArrayIds(outContractIds: ids, statusId: statusId);
        }

        public IEnumerable<OutContractServicePackageDTO> GetAllNotYetTerminateByOutContractIds(string ids, int currencyUnitId)
        {
            return this.GetByArrayIds(outContractIds: ids, currencyUnitId: currencyUnitId);
        }

        public IEnumerable<OutContractServicePackageDTO> GetAllNotAvailableStartAndEndPointByOutContractIds(string ids)
        {
            return this.GetByArrayIds(outContractIds: ids, hasStartPoint: false);
        }

        public List<OutContractServicePackageIntegrationEvent> GetOutContractServicePackageEventByTransactionIds(int[] transactionIds, bool isRestore)
        {
            var cached = new Dictionary<int, OutContractServicePackageIntegrationEvent>();
            var dapperExecution = BuildByTemplate<OutContractServicePackageIntegrationEvent>();
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_NextBilling` AS `NextBilling`");
            dapperExecution.SqlBuilder.Select("t1.`TimeLine_PaymentPeriod` AS `PaymentPeriod`");
            dapperExecution.SqlBuilder.Select("t1.`RadiusAccount` AS `RadiusAccount`");
            dapperExecution.SqlBuilder.Select("t1.`RadiusPassword` AS `RadiusPassword`");

            if (isRestore)
            {
                dapperExecution.SqlBuilder.Select("t1.TimeLine_SuspensionEndDate AS `SuspensionEndDate`");
                dapperExecution.SqlBuilder.Select("t3.Id AS `ServicePackageSuspensionTimeId`");
                dapperExecution.SqlBuilder.Select("t3.RemainingAmount");

                dapperExecution.SqlBuilder.InnerJoin("Transactions t4 ON t4.IsDeleted = FALSE AND t4.StatusId = @statusId AND t4.Id IN @transactionIds",
                    new { statusId = TransactionStatus.Acceptanced.Id, transactionIds });

                dapperExecution.SqlBuilder.InnerJoin("TransactionServicePackages t2 ON t1.Id = t2.OutContractServicePackageId " +
                    "AND t2.IsDeleted = FALSE AND t2.TransactionId = t4.Id");

                dapperExecution.SqlBuilder.InnerJoin("ServicePackageSuspensionTimes t3 ON t3.OutContractServicePackageId = t1.Id " +
                    "AND t3.IsDeleted = FALSE AND t3.SuspensionStartDate = t1.TimeLine_SuspensionStartDate AND t3.RemainingAmount > 0");
            }
            else
            {
                dapperExecution.SqlBuilder.InnerJoin("Transactions t4 ON t4.IsDeleted = FALSE AND t4.StatusId = @statusId AND t4.Id IN @transactionIds",
                    new { statusId = TransactionStatus.Acceptanced.Id, transactionIds });

                dapperExecution.SqlBuilder.InnerJoin("TransactionServicePackages t2 ON t1.Id = t2.OutContractServicePackageId " +
                    "AND t2.IsDeleted = FALSE AND t2.TransactionId = t4.Id");
            }


            return dapperExecution.ExecuteQuery().Distinct().ToList();
        }

        public IEnumerable<OutContractServicePackageDTO> GetAllAvailableByIds(string ids)
        {
            return this.GetByArrayIds(channelIds: ids);
        }

        public IEnumerable<string> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<string>();
            dapperExecution.SqlBuilder.Select("t1.ChannelName AS ChannelName");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE AND t1.ChannelName != '' ");
            return dapperExecution.ExecuteQuery().Distinct();
        }

        public int GetChannelIndexOfCustomer(string customerShortName)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("COUNT(1)");
            dapperExecution.SqlBuilder.InnerJoin("OutContracts oc ON oc.Id = t1.OutContractId");
            dapperExecution.SqlBuilder.InnerJoin("Contractors ct ON oc.ContractorId = ct.Id");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("oc.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("TRIM(ct.ContractorShortName) = @customerShortName", new { customerShortName = customerShortName?.Trim() });
            dapperExecution.SqlBuilder.Where("MONTH(t1.CreatedDate) = MONTH(CURRENT_DATE())");
            var contractChannelCount = dapperExecution.ExecuteQuery().FirstOrDefault();
            var transChannelCount = _transactionServicePackageQueries.GetChannelIndexOfCustomer(customerShortName);
            return contractChannelCount + transChannelCount;
        }

        public OutContractServicePackageDTO FindById(int id)
        {
            var cached = new Dictionary<int, OutContractServicePackageDTO>();
            var dapperExecution = BuildByTemplate<OutContractServicePackageDTO>();

            //2: Channel price bus table
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
            dapperExecution.SqlBuilder.Select("bt.`IsDomestic` AS `IsDomestic`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueTo` AS `UsageBaseUomValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueFrom` AS `UsageBaseUomValueFrom`");

            dapperExecution.SqlBuilder.LeftJoin(
                "ChannelPriceBusTables bt ON bt.ChannelId = t1.Id");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery<OutContractServicePackageDTO, ChannelPriceBusTableDTO>((channel, priceBusTable) =>
            {
                if (!cached.TryGetValue(channel.Id, out var cachedChannel))
                {
                    cachedChannel = channel;
                    cached.Add(cachedChannel.Id, cachedChannel);
                }

                if (priceBusTable != null)
                {
                    cachedChannel.PriceBusTables.Add(priceBusTable);
                }

                return cachedChannel;
            }, "Id");
        }

        public IEnumerable<ChannelAddressModel> GetChannelAddresses(string[] channelCids)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<ChannelAddressModel, ChannelSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t1.CId");

            // Select start point
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("csp");
            // Select end point
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("cep");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints csp ON csp.Id = t1.StartPointChannelId");
            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints cep ON cep.Id = t1.EndPointChannelId");

            dapperExecution.SqlBuilder.Where($"t1.CId IN ({string.Join(",", channelCids.Select(c => $"'{c}'"))})");

            return WithConnection(conn =>
                   conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                   new[]
                   {
                        typeof(string),
                        typeof(InstallationAddress),
                        typeof(InstallationAddress)
                   },
                   (results) =>
                   {
                       var item = new ChannelAddressModel()
                       {
                           Cid = results[0] as string,
                           StartPointAddress = results[1] as InstallationAddress,
                           EndPointAddress = results[2] as InstallationAddress
                       };
                       return item;
                   }, "Id,Id")
               );
        }

        public async Task<bool> IsRadiusAccountExisted(string radiusUserName, int channelId = 0)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("COUNT(1) AS Records");
            dapperExecution.SqlBuilder.Where("t1.RadiusAccount LIKE @radiusUserName AND (@channelId = 0 OR t1.Id <> @channelId)",
                new
                {
                    radiusUserName,
                    channelId
                });

            var existedRecords = await dapperExecution.ExecuteScalarQueryAsync();

            return existedRecords > 0;
        }

        public IEnumerable<OutContractServicePackageDTO> GetAllOutChannels()
        {
            var cached = new Dictionary<int, OutContractServicePackageDTO>();
            var dapperExecution = BuildByTemplate<OutContractServicePackageDTO, ChannelSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t2.Id AS OutContractId");
            dapperExecution.SqlBuilder.Select("t2.ContractCode");
            dapperExecution.SqlBuilder.Select("t2.ContractTypeId AS ContractType");
            dapperExecution.SqlBuilder.Select("t2.ContractorId");
            dapperExecution.SqlBuilder.SelectChannelTimeLine("t1");
            // Select start point
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("csp");
            //// Select end point
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("cep");

            dapperExecution.SqlBuilder.InnerJoin("OutContracts as t2 ON t1.OutContractId  = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints csp ON csp.Id = t1.StartPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints cep ON cep.Id = t1.EndPointChannelId");

            #region Build where clauses
            dapperExecution.SqlBuilder.Where("t1.StatusId IN @validStatus", new { validStatus = OutContractServicePackageStatus.ValidStatuses() });
            dapperExecution.SqlBuilder.Where("t1.Type = @outputChannel", new { outputChannel = (int)ServiceChannelType.Output });
            #endregion

            return WithConnection(conn =>
                conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(OutContractServicePackageDTO),
                    typeof(BillingTimeLine),
                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress),
                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress)
                },
                (results) =>
                {
                    var channel = results[0] as OutContractServicePackageDTO;
                    var billingTimeLine = results[1] as BillingTimeLine;
                    var channelStartPoint = results[2] as OutputChannelPointDTO;
                    var startPointInstallAddress = results[3] as InstallationAddress;

                    var channelEndPoint = results[4] as OutputChannelPointDTO;
                    var endPointInstallAddress = results[5] as InstallationAddress;

                    if (!cached.TryGetValue(channel.Id, out var channelEntity))
                    {
                        channelEntity = channel;
                        channelEntity.TimeLine = billingTimeLine;

                        if (channelEntity.StartPoint == null) channelEntity.StartPoint = channelStartPoint;
                        if (channelEntity.StartPoint != null)
                        {
                            channelEntity.StartPoint.InstallationAddress = startPointInstallAddress;
                        }

                        if (channelEntity.EndPoint == null) channelEntity.EndPoint = channelEndPoint;
                        if (channelEntity.EndPoint != null)
                        {
                            channelEntity.EndPoint.InstallationAddress = endPointInstallAddress;
                        }

                        cached.Add(channelEntity.Id, channelEntity);
                    }

                    return channelEntity;
                },
                param: dapperExecution.ExecutionTemplate.Parameters,
                splitOn: "PaymentPeriod,Id,Id,Id,Id")
            );
        }

        public async Task<IPagedList<OutContractServicePackageDTO>> GetPagedList(ChannelRequestFilterModel requestFilterModel)
        {
            var cached = new Dictionary<int, OutContractServicePackageDTO>();
            var dapperExecution = BuildByTemplate<OutContractServicePackageDTO, ChannelSqlBuilder>();
            dapperExecution.SqlBuilder.Select("t2.Id AS OutContractId");
            dapperExecution.SqlBuilder.Select("t2.ContractCode");
            dapperExecution.SqlBuilder.Select("t2.ContractTypeId AS ContractType");
            dapperExecution.SqlBuilder.Select("t2.ContractorId");
            dapperExecution.SqlBuilder.SelectChannelTimeLine("t1");
            // Select start point
            dapperExecution.SqlBuilder.SelectOutputChannel("csp");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("csp");
            //// Select end point
            dapperExecution.SqlBuilder.SelectOutputChannel("cep");
            dapperExecution.SqlBuilder.SelectChannelInstallAddress("cep");

            dapperExecution.SqlBuilder.InnerJoin("OutContracts as t2 ON t1.OutContractId  = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints csp ON csp.Id = t1.StartPointChannelId");

            dapperExecution.SqlBuilder.LeftJoin("OutputChannelPoints cep ON cep.Id = t1.EndPointChannelId");

            #region Build where clauses
            dapperExecution.SqlBuilder.Where("t1.StatusId IN @validStatus", new { validStatus = OutContractServicePackageStatus.ValidStatuses() });
            dapperExecution.SqlBuilder.Where("t1.Type = @outputChannel", new { outputChannel = (int)ServiceChannelType.Output });

            if (requestFilterModel.ServiceId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.ServiceId = @serviceId", new
                {
                    ServiceId = requestFilterModel.ServiceId.Value
                });
            }

            if (requestFilterModel.ServicePackageId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.ServicePackageId = @servicePackageId", new
                {
                    ServicePackageId = requestFilterModel.ServicePackageId.Value
                });
            }

            if (requestFilterModel.ProjectId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.ProjectId = @projectId", new
                {
                    ProjectId = requestFilterModel.ProjectId.Value
                });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.CountryId))
            {
                dapperExecution.SqlBuilder.Where(
                    "(csp.InstallationAddress_CountryId = @countryId OR cep.InstallationAddress_CountryId = @countryId)",
                    new
                    {
                        CountryId = requestFilterModel.CountryId
                    });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.CityId))
            {
                dapperExecution.SqlBuilder.Where(
                    "(csp.InstallationAddress_CityId = @cityId OR cep.InstallationAddress_CityId = @cityId)",
                    new
                    {
                        CityId = requestFilterModel.CityId
                    });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.DistrictId))
            {
                dapperExecution.SqlBuilder.Where(
                    "(csp.InstallationAddress_DistrictId = @districtId OR cep.InstallationAddress_DistrictId = @districtId)",
                    new
                    {
                        DistrictId = requestFilterModel.DistrictId
                    });
            }

            if (requestFilterModel.StatusIds != null && requestFilterModel.StatusIds.Count() > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId IN @statusIds", new { StatusIds = requestFilterModel.StatusIds });
            }

            if (requestFilterModel.ReclaimEquipmentTypeId.HasValue && requestFilterModel.ReclaimEquipmentTypeId > 0)
            {
                dapperExecution.SqlBuilder.LeftJoin("ContractEquipments sce " +
                    "ON sce.OutputChannelPointId = cep.Id");
                dapperExecution.SqlBuilder.Where("sce.EquipmentId = @equipmentTypeId AND sce.ActivatedUnit > 0", new
                {
                    EquipmentTypeId = requestFilterModel.ReclaimEquipmentTypeId.Value
                });
            }

            #endregion

            var result = await WithConnectionAsync(async conn =>
                await conn.QueryAsync(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(OutContractServicePackageDTO),
                    typeof(BillingTimeLine),
                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress),
                    typeof(OutputChannelPointDTO),
                    typeof(InstallationAddress)
                },
                (results) =>
                {
                    var channel = results[0] as OutContractServicePackageDTO;
                    var billingTimeLine = results[1] as BillingTimeLine;
                    var channelStartPoint = results[2] as OutputChannelPointDTO;
                    var startPointInstallAddress = results[3] as InstallationAddress;

                    var channelEndPoint = results[4] as OutputChannelPointDTO;
                    var endPointInstallAddress = results[5] as InstallationAddress;

                    if (!cached.TryGetValue(channel.Id, out var channelEntity))
                    {
                        channelEntity = channel;
                        channelEntity.TimeLine = billingTimeLine;

                        if (channel.StartPoint == null) channel.StartPoint = channelStartPoint;
                        if (channel.StartPoint != null)
                        {
                            channel.StartPoint.InstallationAddress = startPointInstallAddress;
                        }

                        if (channel.EndPoint == null) channel.EndPoint = channelEndPoint;
                        if (channel.EndPoint != null)
                        {
                            channel.EndPoint.InstallationAddress = endPointInstallAddress;
                        }

                        cached.Add(channelEntity.Id, channelEntity);
                    }

                    return channelEntity;
                },
                param: dapperExecution.ExecutionTemplate.Parameters,
                splitOn: "PaymentPeriod,Id,Id,Id,Id"));

            var totalRecords = await dapperExecution.ExecuteTotalRecordsQueryAsync();
            return new PagedList<OutContractServicePackageDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = result?.Distinct()?.ToList()
            };
        }

        public async Task<bool> IsCIdExisted(string cId, int channelId = 0)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("COUNT(1) AS Records");
            dapperExecution.SqlBuilder.Where("t1.CId LIKE @cId AND (@channelId = 0 OR t1.Id <> @channelId)",
                new
                {
                    cId,
                    channelId
                });

            var existedRecords = await dapperExecution.ExecuteScalarQueryAsync();

            return existedRecords > 0;
        }
    }
}
