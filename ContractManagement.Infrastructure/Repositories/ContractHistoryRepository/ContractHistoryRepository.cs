using ContractManagement.Domain.Models.ChangeHistories;
using ContractManagement.Infrastructure.Repositories.ContractHistoryRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using MongoClusterRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository
{
    public class ContractHistoryRepository : IContractHistoryRepository
    {
        protected readonly IMongoClusterCollection<ContractHistory> ClusterCollection;

        public ContractHistoryRepository(IContractMongoDbContext context)
        {
            ClusterCollection = context.GetCollection<ContractHistory>();
        }

        public async Task<ContractHistory> Get(string id)
        {
            var builder = Builders<ContractHistory>.Filter;
            var filter = builder.Eq("Id", id);
            var queryable = await ClusterCollection.FindAsync<ContractHistory>(filter);
            return queryable.FirstOrDefault();
        }

        public async Task Create(ContractHistory changeHistory)
        {
            await ClusterCollection.InsertOneAsync(changeHistory);
        }

        public async Task Update(string id, ContractHistory changeHistory)
        {
            var builder = Builders<ContractHistory>.Filter;
            var filter = builder.Eq("Id", id);

            await ClusterCollection.ReplaceOneAsync(filter, changeHistory);
        }
        
        //public void Remove(ChangeHistory changeHistory)
        //{
        //    _changeHistories.DeleteOne(c => c.ID == changeHistory.ID);
        //}
        //public void Remove(string id)
        //{
        //    _changeHistories.DeleteOne(c => c.ID == id);
        //}

        public async Task<IPagedList<ContractHistory>> GetList(RequestFilterModel filterModel)
        {
            var builder = Builders<ContractHistory>.Filter;
            FilterDefinition<ContractHistory> filters = builder.Empty;
            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }

            var totalRecords = (int) await ClusterCollection.CountDocumentsAsync(filters);

            var sortBuilder = Builders<ContractHistory>.Sort;
            var queryable = await ClusterCollection.FindAsync(filters, new FindOptions<ContractHistory>() { 
                Skip = filterModel.Skip,
                Limit = filterModel.Take,
                Sort = sortBuilder.Descending("DateCreated")
            });

            var result = new PagedList<ContractHistory>(filterModel.Skip, 
                filterModel.Take, 
                totalRecords)
            {
                Subset = queryable.ToList()
            };

            return result;
        }

        private void CreateFilter(RequestFilterModel filterModel, FilterDefinitionBuilder<ContractHistory> builder, ref FilterDefinition<ContractHistory> filters)
        {
            foreach (string item in filterModel.Filters.Split("|"))
            {
                string[] filter = item.Split("::");
                switch (filter[2])
                {
                    case "contains":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        break;
                    case "doesnotcontain":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        else
                        {
                            filters = !builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        break;
                    case "eq":
                        try
                        {
                            if (bool.Parse(filter[1]))
                            {
                                if (filters != builder.Empty)
                                {
                                    filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                                }
                                else
                                {
                                    filters = builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                                }
                            }
                            else
                            {
                                if (filters != builder.Empty)
                                {
                                    filters = builder.And(filters, builder.Or(builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]), builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value)));
                                }
                                else
                                {
                                    filters = builder.Or(builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]), builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value));
                                }
                            }
                        }
                        catch
                        {
                            if (filters != builder.Empty)
                            {
                                filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                            }
                            else
                            {
                                filters = builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                            }
                        }


                        break;
                    case "neq":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Ne(filter[0].ToUpperFirstLetter(), filter[1]);
                        }
                        else
                        {
                            filters = builder.Ne(filter[0].ToUpperFirstLetter(), filter[1]);
                        }
                        break;
                    case "startswith":
                        string fil = filter[1].ToString();
                        fil = "^" + fil;
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(fil));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(fil));
                        }
                        break;
                    case "endswith":
                        string file = filter[1].ToString();
                        file = file + "^";
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(file));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(file));
                        }
                        break;
                    case "isnull":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        else
                        {
                            filters = builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);

                        }
                        break;
                    case "isnotnull":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        else
                        {
                            filters = !builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        break;
                    case "isempty":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        else
                        {
                            filters = builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        break;
                    case "isnotempty":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        else
                        {
                            filters = !builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        break;

                    default:
                        break;
                }

            }
        }
    }
}

