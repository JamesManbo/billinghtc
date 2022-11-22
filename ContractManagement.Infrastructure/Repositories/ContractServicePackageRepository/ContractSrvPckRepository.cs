using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository
{
    public interface IContractSrvPckRepository : ICrudRepository<OutContractServicePackage, int>
    {
        Task UpdateNextBillingDate(int channelId, DateTime oldBillingDate, DateTime newBillingDate);
        Task UpdateIsCreatedRadiusAccount(int outContractSrvPckId, bool isCreated = true);
        Task UpdateFirstBillingIsCreated(int[] outContractSrvPckIds);
        Task UpdateFirstBillingIsFailed(int[] outContractSrvPckIds);
        Task<List<OutContractServicePackage>> GetByIdsAsync(int[] ids);
        Task UpdateReplacedChannelState(int channelId, string updatedBy);
        Task UpdateNextBillingDateWhenCancelVoucher(IEnumerable<(int ChannelId, DateTime StartBillingDate, DateTime EndBillingDate)> calceledChannels);
    }

    public class ContractSrvPckRepository : CrudRepository<OutContractServicePackage, int>, IContractSrvPckRepository
    {
        public ContractSrvPckRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
            context, configAndMapper)
        {
        }

        public async Task UpdateIsCreatedRadiusAccount(int outContractSrvPckId, bool isCreated = true)
        {
            var outContractSrvPckEntity = await this.GetByIdAsync(outContractSrvPckId);
            outContractSrvPckEntity.SetIsRadiusUserCreated();
            await SaveChangeAsync();
        }

        public Task<List<OutContractServicePackage>> GetByIdsAsync(int[] ids)
        {
            return DbSet
                .Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public async Task UpdateFirstBillingIsCreated(int[] outContractSrvPckIds)
        {
            if (outContractSrvPckIds.Length == 0) return;

            var entities = await this.GetByIdsAsync(outContractSrvPckIds);
            foreach (var entity in entities)
            {
                entity.SetIsPaidTheFirstBilling();
            }
            await SaveChangeAsync();
        }

        public async Task UpdateNextBillingDateWhenCancelVoucher(IEnumerable<(int ChannelId, DateTime StartBillingDate, DateTime EndBillingDate)> billingCanceledChannels)
        {
            if (!billingCanceledChannels.Any()) return;

            var channelIds = billingCanceledChannels.Select(c => c.ChannelId).ToArray();
            var channels = DbSet.Where(c => channelIds.Contains(c.Id));

            foreach (var channel in channels)
            {
                var canceledVoucherDetail = billingCanceledChannels
                    .First(c => c.ChannelId == channel.Id);
                if (channel.TimeLine.PaymentForm == (int)PaymentMethodForm.Prepaid)
                {
                    if (channel.TimeLine.NextBilling.HasValue &&
                      channel.TimeLine.NextBilling.Value.Date == canceledVoucherDetail.EndBillingDate.AddDays(1).Date)
                    {
                        channel.SetNextBillingWhenCancelVoucher(canceledVoucherDetail.StartBillingDate);
                    }
                }
                else
                {
                    if (channel.TimeLine.NextBilling.HasValue &&
                        channel.TimeLine.NextBilling.Value.Date ==
                            canceledVoucherDetail.EndBillingDate.AddDays(1).AddMonths(channel.TimeLine.PaymentPeriod).Date)
                    {
                        channel.SetNextBillingWhenCancelVoucher(canceledVoucherDetail.EndBillingDate.AddDays(1));
                    }
                }
            }

            await SaveChangeAsync();
        }

        public async Task UpdateFirstBillingIsFailed(int[] outContractSrvPckIds)
        {
            var entities = await this.GetByIdsAsync(outContractSrvPckIds);
            foreach (var entity in entities)
            {
                entity.SetIsFirstBilling();
            }
            await SaveChangeAsync();
        }

        public async Task UpdateReplacedChannelState(int channelId, string updatedBy)
        {
            var outputChannel = await GetByIdAsync(channelId);

            outputChannel.StatusId = OutContractServicePackageStatus.Replaced.Id;
            outputChannel.UpdatedBy = updatedBy;
            outputChannel.UpdatedDate = DateTime.Now;

            Update(outputChannel);
        }

        public async Task UpdateNextBillingDate(int channelId, DateTime oldBillingDate, DateTime newBillingDate)
        {
            var channel = await GetByIdAsync(channelId);

            if (channel == null ||
                !channel.TimeLine.NextBilling.HasValue ||
                channel.TimeLine.NextBilling.Value.Date != oldBillingDate.Date.AddDays(1))
            {
                return;
            }

            channel.SetNextBillingDate(newBillingDate.AddDays(1));
            Update(channel);
            await SaveChangeAsync();
        }
    }
}