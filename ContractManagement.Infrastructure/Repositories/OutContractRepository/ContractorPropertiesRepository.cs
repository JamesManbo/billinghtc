using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IContractorPropertiesRepository : ICrudRepository<ContractorProperties, int>
    {
        ContractorProperties FindByApplicationUserId(string id);
        void RemoveAllContractorStructure(int structureId);
        void RemoveAllContractorCategory(int categoryId);
        void RemoveAllContractorGroup(string groupId, string groupName);
        void RemoveAllContractorClass(int classId);
        void RemoveAllContractorType(int typeId);
        void RemoveAllContractorIndustry(int industryId);

        void UpdateAllContractorStructure(int structureId, string structureName);
        void UpdateAllContractorCategory(int categoryId, string categoryName);
        void UpdateAllContractorGroup(string groupId, string oldGroupName, string newGroupName);
        void UpdateAllContractorClass(int classId, string className);
        void UpdateAllContractorType(int typeId, string typeName);
        void UpdateAllContractorIndustry(string industryId, string oldIndustryName, string newIndustryName);
    }

    public class ContractorPropertiesRepository : CrudRepository<ContractorProperties, int>, IContractorPropertiesRepository
    {
        public readonly ContractDbContext _contractDbContext;
        public ContractorPropertiesRepository(ContractDbContext context,
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public ContractorProperties FindByApplicationUserId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return default;

            return DbSet.FirstOrDefault(c => c.ApplicationUserIdentityGuid.Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void RemoveAllContractorCategory(int categoryId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorCategoryId = NULL, ContractorCategoryName = NULL " +
            "WHERE ContractorCategoryId = @categoryId",
                 new MySqlParameter("@categoryId", MySqlDbType.Int32)
                 {
                     Value = categoryId
                 });
        }

        public void RemoveAllContractorClass(int classId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorClassId = NULL, ContractorClassName = NULL " +
               "WHERE ContractorClassId = @classId",
                    new MySqlParameter("@classId", MySqlDbType.Int32)
                    {
                        Value = classId
                    });
        }

        public void RemoveAllContractorGroup(string groupId, string groupName)
        {
            var removeGroupNameQuery = @"REPLACE(CONCAT(',', ContractorGroupNames, ','), CONCAT(',', @groupName, ','), ',')";
            var removeGroupIdQuery = @"REPLACE(CONCAT(',', ContractorGroupIds, ','), CONCAT(',', @groupId, ','), ',')";

            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET " +
                $"ContractorGroupIds = SUBSTR({removeGroupIdQuery}, 2, CHAR_LENGTH({removeGroupIdQuery}) - 2), " +
                $"ContractorGroupNames = SUBSTR({removeGroupNameQuery}, 2, CHAR_LENGTH({removeGroupNameQuery}) - 2) " +
                $"WHERE FIND_IN_SET(@groupId, ContractorGroupIds)",
            new MySqlParameter("@groupId", MySqlDbType.VarChar)
            {
                Value = groupId
            }, new MySqlParameter("@groupName", MySqlDbType.VarChar)
            {
                Value = groupName
            });
        }

        public void RemoveAllContractorIndustry(int industryId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorIndustryId = NULL, ContractorIndustryName = NULL " +
               "WHERE ContractorIndustryId = @industryId",
                    new MySqlParameter("@industryId", MySqlDbType.Int32)
                    {
                        Value = industryId
                    });
        }

        public void RemoveAllContractorStructure(int structureId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorStructureId = NULL, ContractorStructureName = NULL " +
                 "WHERE ContractorStructureId = @structureId",
                      new MySqlParameter("@structureId", MySqlDbType.Int32)
                      {
                          Value = structureId
                      });
        }

        public void RemoveAllContractorType(int typeId)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorTypeId = NULL, ContractorTypeName = NULL " +
                    "WHERE ContractorTypeId = @typeId",
                         new MySqlParameter("@typeId", MySqlDbType.Int32)
                         {
                             Value = typeId
                         });
        }

        public void UpdateAllContractorCategory(int categoryId, string categoryName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorCategoryName = @categoryName " +
            "WHERE ContractorCategoryId = @categoryId",
                 new MySqlParameter("@categoryId", MySqlDbType.Int32)
                 {
                     Value = categoryId
                 },
                 new MySqlParameter("@categoryName", MySqlDbType.VarChar)
                 {
                     Value = categoryName
                 });
        }

        public void UpdateAllContractorClass(int classId, string className)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorClassName = @className " +
               "WHERE ContractorClassId = @classId",
                    new MySqlParameter("@classId", MySqlDbType.Int32)
                    {
                        Value = classId
                    }, new MySqlParameter("@className", MySqlDbType.VarChar)
                    {
                        Value = className
                    });
        }

        public void UpdateAllContractorGroup(string groupId, string oldGroupName, string newGroupName)
        {
            var buildNewGroupNameQuery = @"REPLACE(CONCAT(',', ContractorGroupNames, ','), CONCAT(',', @oldGroupName, ','), CONCAT(',', @newGroupName, ','))";

            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET " +
                $"ContractorGroupNames = SUBSTR({buildNewGroupNameQuery}, 2, CHAR_LENGTH({buildNewGroupNameQuery}) - 2) " +
                $"WHERE FIND_IN_SET(@groupId, ContractorGroupIds)",
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

        public void UpdateAllContractorIndustry(string industryId, string oldIndustryName, string newIndustryName)
        {
            var buildNewIndustryNameQuery = @"REPLACE(CONCAT(',', ContractorIndustryNames, ','), CONCAT(',', @oldIndustryName, ','), CONCAT(',', @newIndustryName, ','))";

            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET " +
                    $"ContractorIndustryNames =SUBSTR({buildNewIndustryNameQuery}, 2, CHAR_LENGTH({buildNewIndustryNameQuery}) - 2) " +
                    $"WHERE FIND_IN_SET(@industryId, ContractorIndustryIds)",
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

        public void UpdateAllContractorStructure(int structureId, string structureName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorStructureName = @structureName " +
                 "WHERE ContractorStructureId = @structureId",
                      new MySqlParameter("@structureId", MySqlDbType.Int32)
                      {
                          Value = structureId
                      }, new MySqlParameter("@structureName", MySqlDbType.VarChar)
                      {
                          Value = structureName
                      });
        }

        public void UpdateAllContractorType(int typeId, string typeName)
        {
            Context.Database.ExecuteSqlRaw("UPDATE ContractorProperties SET ContractorTypeName = @typeName " +
                       "WHERE ContractorTypeId = @typeId",
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
