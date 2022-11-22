using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IVoucherTargetPropertyRepository : ICrudRepository<VoucherTargetProperty, int>
    {
        void SynchronizeVoucherTargetProperties(string customerGuid, int voucherTargetId);
        VoucherTargetProperty FindByApplicationUserId(string id);
        void RemoveAllStructure(int structureId);
        void RemoveAllCategory(int categoryId);
        void RemoveAllGroup(string groupId, string groupName);
        void RemoveAllClass(int classId);
        void RemoveAllType(int typeId);
        void RemoveAllIndustry(int industryId);

        void UpdateAllStructure(int structureId, string structureName);
        void UpdateAllCategory(int categoryId, string categoryName);
        void UpdateAllGroup(string groupId, string oldGroupName, string newGroupName);
        void UpdateAllClass(int classId, string className);
        void UpdateAllType(int typeId, string typeName);
        void UpdateAllIndustry(string industryId, string oldIndustryName, string newIndustryName);
    }
    public class VoucherTargetPropertyRepository : CrudRepository<VoucherTargetProperty, int>, IVoucherTargetPropertyRepository
    {
        protected readonly DebtDbContext DebtDbContext;
        public VoucherTargetPropertyRepository(DebtDbContext context,
            IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            this.DebtDbContext = context;
        }

        public VoucherTargetProperty FindByApplicationUserId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return default;

            return DbSet.FirstOrDefault(c => c.ApplicationUserIdentityGuid.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void RemoveAllCategory(int categoryId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET CategoryId = NULL, CategoryName = NULL " +
            "WHERE CategoryId = @categoryId",
                 new MySqlParameter("@categoryId", MySqlDbType.Int32)
                 {
                     Value = categoryId
                 });
        }

        public void RemoveAllClass(int classId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET ClassId = NULL, ClassName = NULL " +
               "WHERE ClassId = @classId",
                    new MySqlParameter("@classId", MySqlDbType.Int32)
                    {
                        Value = classId
                    });
        }

        public void RemoveAllGroup(string groupId, string groupName)
        {
            var removeGroupNameQuery = @"REPLACE(CONCAT(',', GroupNames, ','), CONCAT(',', @groupName, ','), ',')";
            var removeGroupIdQuery = @"REPLACE(CONCAT(',', GroupIds, ','), CONCAT(',', @groupId, ','), ',')";

            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET " +
                $"GroupIds = SUBSTR({removeGroupIdQuery}, 2, CHAR_LENGTH({removeGroupIdQuery}) - 2), " +
                $"GroupNames = SUBSTR({removeGroupNameQuery}, 2, CHAR_LENGTH({removeGroupNameQuery}) - 2) " +
                $"WHERE FIND_IN_SET(@groupId, GroupIds)",
            new MySqlParameter("@groupId", MySqlDbType.VarChar)
            {
                Value = groupId
            }, new MySqlParameter("@groupName", MySqlDbType.VarChar)
            {
                Value = groupName
            });
        }

        public void RemoveAllIndustry(int industryId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET IndustryId = NULL, IndustryName = NULL " +
               "WHERE IndustryId = @industryId",
                    new MySqlParameter("@industryId", MySqlDbType.Int32)
                    {
                        Value = industryId
                    });
        }

        public void RemoveAllStructure(int structureId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET StructureId = NULL, StructureName = NULL " +
                 "WHERE StructureId = @structureId",
                      new MySqlParameter("@structureId", MySqlDbType.Int32)
                      {
                          Value = structureId
                      });
        }

        public void RemoveAllType(int typeId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET TypeId = NULL, TypeName = NULL " +
                    "WHERE TypeId = @typeId",
                         new MySqlParameter("@typeId", MySqlDbType.Int32)
                         {
                             Value = typeId
                         });
        }

        public void SynchronizeVoucherTargetProperties(string customerGuid, int voucherTargetId)
        {
            Context.Database.ExecuteSqlRaw("CALL SynchronizeVoucherTargetProperties(@customerGuid, @voucherTargetId)",
                new MySqlParameter("@customerGuid", MySqlDbType.VarChar)
                {
                    Value = customerGuid
                },
                new MySqlParameter("@voucherTargetId", MySqlDbType.Int32)
                {
                    Value = voucherTargetId
                });
        }

        public void UpdateAllCategory(int categoryId, string categoryName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET CategoryName = @categoryName " +
            "WHERE CategoryId = @categoryId",
                 new MySqlParameter("@categoryId", MySqlDbType.Int32)
                 {
                     Value = categoryId
                 },
                 new MySqlParameter("@categoryName", MySqlDbType.VarChar)
                 {
                     Value = categoryName
                 });
        }

        public void UpdateAllClass(int classId, string className)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET ClassName = @className " +
               "WHERE ClassId = @classId",
                    new MySqlParameter("@classId", MySqlDbType.Int32)
                    {
                        Value = classId
                    }, new MySqlParameter("@className", MySqlDbType.VarChar)
                    {
                        Value = className
                    });
        }

        public void UpdateAllGroup(string groupId, string oldGroupName, string newGroupName)
        {
            var buildNewGroupNameQuery = @"REPLACE(CONCAT(',', GroupNames, ','), CONCAT(',', @oldGroupName, ','), CONCAT(',', @newGroupName, ','))";

            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET " +
                $"GroupNames = SUBSTR({buildNewGroupNameQuery}, 2, CHAR_LENGTH({buildNewGroupNameQuery}) - 2) " +
                $"WHERE FIND_IN_SET(@groupId, GroupIds)",
            new MySqlParameter("@groupId", MySqlDbType.VarChar)
            {
                Value = groupId
            }, new MySqlParameter("@oldGroupName", MySqlDbType.VarChar)
            {
                Value = oldGroupName
            },
            new MySqlParameter("@newGroupName", MySqlDbType.VarChar)
            {
                Value = newGroupName
            });
        }

        public void UpdateAllIndustry(string industryId, string oldIndustryName, string newIndustryName)
        {
            var buildNewIndustryNameQuery = @"REPLACE(CONCAT(',', IndustryNames, ','), CONCAT(',', @oldIndustryName, ','), CONCAT(',', @newIndustryName, ','))";

            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET " +
                    $"IndustryNames = SUBSTR({buildNewIndustryNameQuery}, 2, CHAR_LENGTH({buildNewIndustryNameQuery}) - 2) " +
                    $"WHERE FIND_IN_SET(@industryId, IndustryIds)",
                    new MySqlParameter("@industryId", MySqlDbType.VarChar)
                    {
                        Value = industryId
                    },
                    new MySqlParameter("@oldIndustryName", MySqlDbType.VarChar)
                    {
                        Value = oldIndustryName
                    },
                    new MySqlParameter("@newIndustryName", MySqlDbType.VarChar)
                    {
                        Value = newIndustryName
                    });
        }

        public void UpdateAllStructure(int structureId, string structureName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET StructureName = @structureName " +
                 "WHERE StructureId = @structureId",
                      new MySqlParameter("@structureId", MySqlDbType.Int32)
                      {
                          Value = structureId
                      }, new MySqlParameter("@structureName", MySqlDbType.VarChar)
                      {
                          Value = structureName
                      });
        }

        public void UpdateAllType(int typeId, string typeName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE VoucherTargetProperties SET TypeName = @typeName " +
                       "WHERE TypeId = @typeId",
                            new MySqlParameter("@typeId", MySqlDbType.Int32)
                            {
                                Value = typeId
                            }, new MySqlParameter("@typeName", MySqlDbType.VarChar)
                            {
                                Value = typeName
                            });
        }
    }
}
