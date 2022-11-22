using System.Collections.Generic;
using System.Threading.Tasks;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Location.API.Model;
using Location.API.Location;
using MongoDB.Bson;
using MongoDB.Driver;
using Location.API.Commands;
using MongoClusterRepository;

namespace Location.API.Reponsitory
{
    public interface ISupportLocationQueries
    {
        Task<IPagedList<SupportLocation>> GetList(RequestFilterModel filterModel);
        Task<SupportLocation> Create(SupportLocation supportRequest);
        Task<SupportLocation> GetById(string id);
        Task<SupportLocation> Update(SupportLocation supportLocation);
        Task<bool> Delete(string id);
    }

    public class SupportLocationQueries : ISupportLocationQueries
    {
        private readonly IMongoClusterCollection<SupportLocation> _collection;
        public SupportLocationQueries(ILocationkMongoDbContext context)
        {
            _collection = context.GetCollection<SupportLocation>();
        }

        public async Task<SupportLocation> Create(SupportLocation supportRequest)
        {
            await _collection.InsertOneAsync(supportRequest);
            return supportRequest;
        }

        public async Task<IPagedList<SupportLocation>> GetList(RequestFilterModel filterModel)
        {
            var builder = Builders<SupportLocation>.Filter;

            FilterDefinition<SupportLocation> filters = builder.Empty;
            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }

            var totalRecords = await _collection.CountDocumentsAsync(filters);
            var query = await _collection.FindAsync(filters,
                new FindOptions<SupportLocation>()
                {
                    Skip = filterModel.Skip,
                    Limit = filterModel.Take
                });

            var result = new PagedList<SupportLocation>(filterModel.Skip, filterModel.Take, (int)totalRecords)
            {
                Subset = query?.ToList()
            };
            return result;
        }

        public async Task<SupportLocation> GetById(string id)
        {
            var filterBuilder = Builders<SupportLocation>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var query = await _collection.FindAsync<SupportLocation>(filter);
            return query?.FirstOrDefault();
        }

        public async Task<SupportLocation> Update(SupportLocation supportLocation)
        {
            var filterBuilder = Builders<SupportLocation>.Filter;
            var filter = filterBuilder.Eq("Id", supportLocation.Id);
            await _collection.ReplaceOneAsync(filter, supportLocation);
            var successResult = await _collection.FindAsync<SupportLocation>(filter);
            return successResult.FirstOrDefault();
        }
        private void CreateFilter(RequestFilterModel filterModel, FilterDefinitionBuilder<SupportLocation> builder, ref FilterDefinition<SupportLocation> filters)
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

        public async Task<bool> Delete(string id)
        {
            var filterBuilder = Builders<SupportLocation>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var deleteResult = await _collection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0 ? true : false;
        }
    }
}
