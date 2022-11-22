using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models.Reports;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.PagedList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ReportsRepository
{
    public class ReportsRepository : CrudRepository<ContractEquipment, int>, IReportsRepository
    {
        private readonly ContractDbContext _context;
        public ReportsRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }

        //public IEnumerable<ReportEquipmentInProjectDTO> GetEquipmentInProjectOfOutcontract(string projectName, string equipmentName)
        //{
        //    var cached = new Dictionary<int, ReportEquipmentInProjectDTO>();
        //    return WithConnection(conn =>
        //            conn.Query<OutContractSharingRevenueDTO, OutContractServicePackageDTO, BillingTimeLine, ContractSharingRevenueLineDTO, OutContractSharingRevenueDTO
        //            >("GetOutContractSharingRevenueByInContract",
        //                (outContractSharingRevenue, contractServicePackage, billingTimeLine, sharingRevenueLine) =>
        //                {
        //                    if (outContractSharingRevenue == null)
        //                    {
        //                        return null;
        //                    }

        //                    if (!cached.TryGetValue(outContractSharingRevenue.OutContractId, out var cachedItem))
        //                    {
        //                        cachedItem = outContractSharingRevenue;
        //                        cached.Add(outContractSharingRevenue.OutContractId, cachedItem);
        //                    }

        //                    if (cachedItem.ServicePackages.All(s => s.Id != contractServicePackage.Id))
        //                    {
        //                        contractServicePackage.TimeLine = billingTimeLine;
        //                        cachedItem.ServicePackages.Add(contractServicePackage);
        //                    }

        //                    if (cachedItem.SharingRevenueLines.All(c => c.Id != sharingRevenueLine.Id))
        //                    {
        //                        cachedItem.SharingRevenueLines.Add(sharingRevenueLine);
        //                    }

        //                    return cachedItem;
        //                },
        //                new { inContractId, inContractType = inContractType ?? InContractType.InChannelRental.Id },
        //                commandType: CommandType.StoredProcedure,
        //                splitOn: "Id,PaymentPeriod,Id"))
        //        .Distinct();
        //}

    }
}
