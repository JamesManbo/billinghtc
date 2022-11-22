using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ProjectTechnicianEntityConfiguration : IEntityTypeConfiguration<ProjectTechnician>
    {
        public void Configure(EntityTypeBuilder<ProjectTechnician> builder)
        {
            builder.HasKey(k => k.Id);
        }
    }
}
