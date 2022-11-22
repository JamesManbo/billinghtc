using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.SharingRevenueModels;
using ContractManagement.Utility;
using GenericRepository;
using GenericRepository.Core;
using GenericRepository.DapperSqlBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IContractSharingRevenueQueries : IQueryRepository
    {
        IEnumerable<ContractSharingRevenueLineDTO> GetAllByOutContractId(int outContractId);
        IEnumerable<ContractSharingRevenueLineDTO> GetAllByInContractId(int inContractId, int inContractType);
    }

    public class ContractSharingRevenueSqlBuilder : SqlBuilder
    {
        public ContractSharingRevenueSqlBuilder(string tableName) : base(tableName)
        {
        }

        public ContractSharingRevenueSqlBuilder()
        {
        }

        public void SelectServicePackage(string alias)
        {
            Select($"{alias}.`Id` AS `Id`");
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
    }

    public class ContractSharingRevenueQueries : QueryRepository<ContractSharingRevenueLine, int>, IContractSharingRevenueQueries
    {
        public ContractSharingRevenueQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {
        }

        public IEnumerable<ContractSharingRevenueLineDTO> GetAllByInContractId(int inContractId, int inContractType)
        {
            var dapperExecution = BuildByTemplate<ContractSharingRevenueLineDTO, ContractSharingRevenueSqlBuilder>();

            dapperExecution.SqlBuilder.Where("t1.InContractId = @inContractId", new { inContractId });

            if (inContractType == InContractType.InChannelRental.Id)
            {
                return GetAllSharingRentalChannel(dapperExecution);
            }
            else if (inContractType == InContractType.InCommission.Id ||
                inContractType == InContractType.InSharingRevenue.Id)
            {
                return GetAllSharingRevenueAndCommission(dapperExecution);
            }
            else
            {
                return GetAllSharingMaintennance(dapperExecution);
            }
        }

        private IEnumerable<ContractSharingRevenueLineDTO> GetAllSharingMaintennance(
            DapperExecution<ContractSharingRevenueLineDTO, ContractSharingRevenueSqlBuilder> dapperExecution)
        {
            dapperExecution.SqlBuilder.SelectServicePackage("oc");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocsp");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocep");

            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages oc ON oc.Id = t1.OutServiceChannelId");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints ocsp ON ocsp.Id = oc.StartPointChannelId"
                );

            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints ocep ON ocep.Id = oc.EndPointChannelId"
                );
            return dapperExecution
                .ExecuteQuery(
                    new Type[]
                    {
                        typeof(ContractSharingRevenueLineDTO),
                        typeof(OutContractServicePackageDTO),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                    }, (results) =>
                    {
                        var sharingRevenueLine = (ContractSharingRevenueLineDTO)results[0];
                        var outletChannel = (OutContractServicePackageDTO)results[1];
                        if (results[2] is OutputChannelPointDTO outChannelStartPoint
                            && outChannelStartPoint != null)
                        {
                            outChannelStartPoint.InstallationAddress = (InstallationAddress)results[3];
                            outletChannel.StartPoint = outChannelStartPoint;
                        }

                        if (results[4] is OutputChannelPointDTO outChannelEndPoint
                            && outChannelEndPoint != null)
                        {
                            outChannelEndPoint.InstallationAddress = (InstallationAddress)results[5];
                            outletChannel.EndPoint = outChannelEndPoint;
                        }
                        sharingRevenueLine.OutletChannel = outletChannel;

                        return sharingRevenueLine;
                    },
                    "Id,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter"
                );
        }
        private IEnumerable<ContractSharingRevenueLineDTO> GetAllSharingRentalChannel(
            DapperExecution<ContractSharingRevenueLineDTO, ContractSharingRevenueSqlBuilder> dapperExecution)
        {
            dapperExecution.SqlBuilder.SelectServicePackage("oc");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocsp");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocep");

            dapperExecution.SqlBuilder.SelectServicePackage("ic");
            dapperExecution.SqlBuilder.SelectOutputChannel("icsp");
            dapperExecution.SqlBuilder.SelectOutputChannel("icep");

            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages ic ON ic.Id = t1.InServiceChannelId");
            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages oc ON oc.Id = t1.OutServiceChannelId");


            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints icep ON icep.Id = ic.EndPointChannelId"
                );
            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints ocep ON ocep.Id = oc.EndPointChannelId"
                );

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints ocsp ON ocsp.Id = oc.StartPointChannelId"
                );
            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints icsp ON icsp.Id = ic.StartPointChannelId"
                );
            return dapperExecution
                .ExecuteQuery(
                    new Type[]
                    {
                        typeof(ContractSharingRevenueLineDTO),
                        typeof(OutContractServicePackageDTO),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutContractServicePackageDTO),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                    }, (results) =>
                    {
                        var sharingRevenueLine = (ContractSharingRevenueLineDTO)results[0];
                        var outletChannel = (OutContractServicePackageDTO)results[1];
                        if (results[2] is OutputChannelPointDTO outChannelStartPoint
                            && outChannelStartPoint != null)
                        {
                            outChannelStartPoint.InstallationAddress = (InstallationAddress)results[3];
                            outletChannel.StartPoint = outChannelStartPoint;
                        }

                        if (results[4] is OutputChannelPointDTO outChannelEndPoint
                            && outChannelEndPoint != null)
                        {
                            outChannelEndPoint.InstallationAddress = (InstallationAddress)results[5];
                            outletChannel.EndPoint = outChannelEndPoint;
                        }

                        var inletChannel = (OutContractServicePackageDTO)results[6];
                        if (results[7] is OutputChannelPointDTO inletChannelStartPoint
                            && inletChannelStartPoint != null)
                        {
                            inletChannelStartPoint.InstallationAddress = (InstallationAddress)results[8];
                            inletChannel.StartPoint = inletChannelStartPoint;
                        }

                        if (results[9] is OutputChannelPointDTO inletChannelEndPoint
                            && inletChannelEndPoint != null)
                        {
                            inletChannelEndPoint.InstallationAddress = (InstallationAddress)results[10];
                            inletChannel.EndPoint = inletChannelEndPoint;
                        }
                        sharingRevenueLine.OutletChannel = outletChannel;
                        sharingRevenueLine.InletChannel = inletChannel;

                        return sharingRevenueLine;
                    },
                    "Id,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter,Id,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter"
                );
        }
        private IEnumerable<ContractSharingRevenueLineDTO> GetAllSharingRevenueAndCommission(
            DapperExecution<ContractSharingRevenueLineDTO, ContractSharingRevenueSqlBuilder> dapperExecution)
        {
            dapperExecution.SqlBuilder.SelectServicePackage("oc");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocsp");
            dapperExecution.SqlBuilder.SelectOutputChannel("ocep");

            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages oc ON oc.Id = t1.OutServiceChannelId");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutputChannelPoints ocep ON ocep.Id = oc.EndPointChannelId"
                );

            dapperExecution.SqlBuilder.LeftJoin(
                "OutputChannelPoints ocsp ON ocsp.Id = oc.StartPointChannelId"
                );
            return dapperExecution
                .ExecuteQuery(
                    new Type[]
                    {
                        typeof(ContractSharingRevenueLineDTO),
                        typeof(OutContractServicePackageDTO),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress),
                        typeof(OutputChannelPointDTO),
                        typeof(InstallationAddress)
                    }, (results) =>
                    {
                        var sharingRevenueLine = (ContractSharingRevenueLineDTO)results[0];
                        var outletChannel = (OutContractServicePackageDTO)results[1];
                        if (results[2] is OutputChannelPointDTO outChannelStartPoint
                            && outChannelStartPoint != null)
                        {
                            outChannelStartPoint.InstallationAddress = (InstallationAddress)results[3];
                            outletChannel.StartPoint = outChannelStartPoint;
                        }

                        if (results[4] is OutputChannelPointDTO outChannelEndPoint
                            && outChannelEndPoint != null)
                        {
                            outChannelEndPoint.InstallationAddress = (InstallationAddress)results[5];
                            outletChannel.EndPoint = outChannelEndPoint;
                        }

                        sharingRevenueLine.OutletChannel = outletChannel;

                        return sharingRevenueLine;
                    },
                    "Id,Id,InstallationAddressSpliter,Id,InstallationAddressSpliter"
                );
        }

        public IEnumerable<ContractSharingRevenueLineDTO> GetAllByOutContractId(int outContractId)
        {
            var cache = new Dictionary<int, ContractSharingRevenueLineDTO>();
            var dapperExecution = BuildByTemplate<ContractSharingRevenueLineDTO>();

            dapperExecution.SqlBuilder.Select("CONCAT_WS('', 'Kênh '," +
            "IF(t3.CId IS NULL OR t3.CId = '', 'chưa nghiệm thu', t3.CId)," +
            "', dịch vụ '," +
            "t3.ServiceName," +
            "IF(t3.ServicePackageId IS NOT NULL && t3.ServicePackageId > 0, CONCAT(', gói cước ', t3.PackageName), '')" +
            ")" +
            " AS `OutletChannelDescription`");

            dapperExecution.SqlBuilder.Select("IF(t1.InServiceChannelId > 0, CONCAT_WS('', 'Kênh '," +
            "IF(t4.CId IS NULL OR t4.CId = '', 'chưa nghiệm thu', t4.CId)," +
            "', dịch vụ '," +
            "t4.ServiceName," +
            "IF(t4.ServicePackageId IS NOT NULL && t4.ServicePackageId > 0, CONCAT(', gói cước ', t4.PackageName), '')" +
            "), '')" +
            " AS `InletChannelDescription`");

            /// Truy vấn chi tiết giá trị phân chia doanh thu
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.SharingLineUid");
            dapperExecution.SqlBuilder.Select("t2.SharingLineId");
            dapperExecution.SqlBuilder.Select("t2.CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t2.SharingType");
            dapperExecution.SqlBuilder.Select("t2.Year");
            dapperExecution.SqlBuilder.Select("t2.Month");
            dapperExecution.SqlBuilder.Select("t2.CreatedDate");
            dapperExecution.SqlBuilder.Select("t2.CreatedBy");
            dapperExecution.SqlBuilder.Select("t2.UpdatedDate");
            dapperExecution.SqlBuilder.Select("t2.UpdatedBy");
            dapperExecution.SqlBuilder.Select("t2.SharingAmount");

            dapperExecution.SqlBuilder.LeftJoin("SharingRevenueLineDetails t2 ON t2.SharingLineId = t1.Id");

            dapperExecution.SqlBuilder.InnerJoin("OutContractServicePackages t3 ON t3.Id = t1.OutServiceChannelId");
            dapperExecution.SqlBuilder.LeftJoin("OutContractServicePackages t4 ON t4.Id = t1.InServiceChannelId");

            dapperExecution.SqlBuilder.Where("t1.OutContractId = @outContractId", new { outContractId });

            return dapperExecution
                .ExecuteQuery<ContractSharingRevenueLineDTO, SharingRevenueLineDetailDTO>(
                (sharingLine, sharingDetail) =>
                {
                    if (cache.TryGetValue(sharingLine.Id, out var cachedValue))
                    {
                        sharingLine = cachedValue;
                    }
                    else
                    {
                        cache.Add(sharingLine.Id, sharingLine);
                    }

                    if (sharingDetail != null &&
                        sharingLine.SharingLineDetails.All(c => c.Id != sharingDetail.Id))
                    {
                        sharingLine.SharingLineDetails.Add(sharingDetail);
                    }

                    return sharingLine;
                },
                "Id"
                )
                .Distinct();
        }
    }
}