using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class InContractServiceEntityConfiguration : IEntityTypeConfiguration<InContractService>
    {
        public void Configure(EntityTypeBuilder<InContractService> builder)
        {

            builder.Ignore(b => b.DomainEvents);
        }
    }
}