using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionEquipmentEntityConfigurations : IEntityTypeConfiguration<TransactionEquipment>
    {
        public void Configure(EntityTypeBuilder<TransactionEquipment> builder)
        {
            builder.Ignore(o => o.DomainEvents);
        }
    }
}
