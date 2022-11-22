using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Location.API.Location;
using Location.API.Model;
using MongoClusterRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.ObjectModel;

namespace Location.API.Reponsitory
{
    public interface ILocationQueries
    {
        Task<SupportLocation> Create(SupportLocation supportRequest);
        Task<IPagedList<SupportLocation>> GetList(SupportLocationRequestFilterModel filterModel);
        Task<LocationModel> CreateLocation(LocationModel location);
        Task<LocationModel> UpdateLocation(LocationModel supportRequest);
        Task<LocationModel> GetById(string id);
        Task<LocationModel> GetByLocationId(string locationId);
        Task<LocationModel> GetByLocationCode(string locationCode);
        Task<IEnumerable<LocationSelectionItem>> GetSelectionLocation(LocationRequestFilterModel filterModel);
        Task<IPagedList<LocationModel>> GetListLocation(LocationRequestFilterModel filterModel);
        Task<IEnumerable<LocationModel>> GetLocationByLevel(int level);
        Task<bool> IsExisted(string id);
        Task<string> DeleteLocation(string id);
        Task<IEnumerable<LocationModel>> GetAll();
    }
    public class LocationQueries : ILocationQueries
    {

        private readonly IMongoClusterCollection<SupportLocation> _collection;
        private readonly IMongoClusterCollection<LocationModel> _locationCollection;
        private readonly IWrappedConfigAndMapper _wrappedConfig;

        public LocationQueries(ILocationkMongoDbContext context,
            IWrappedConfigAndMapper wrappedConfig)
        {
            _collection = context.GetCollection<SupportLocation>();
            _locationCollection = context.GetCollection<LocationModel>();
            _wrappedConfig = wrappedConfig;
        }

        #region SupportLocation

        public async Task<SupportLocation> Create(SupportLocation supportRequest)
        {
            await _collection.InsertOneAsync(supportRequest);
            return supportRequest;
        }

        public async Task<IPagedList<SupportLocation>> GetList(SupportLocationRequestFilterModel filterModel)
        {
            var builder = Builders<SupportLocation>.Filter;

            FilterDefinition<SupportLocation> filters = builder.Empty;
            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }
            if (!string.IsNullOrEmpty(filterModel.IdentityGuid))
            {
                filters = filters & builder.Eq("identityGuid", filterModel.IdentityGuid);
            }

            var totalRecords = await _collection.CountDocumentsAsync(filters);
            var subSetResult = await _collection.FindAsync(filters,
                new FindOptions<SupportLocation>()
                {
                    Skip = filterModel.Skip,
                    Limit = filterModel.Take
                });

            var result = new PagedList<SupportLocation>(filterModel.Skip, filterModel.Take, int.Parse(totalRecords.ToString()))
            {
                Subset = subSetResult?.ToList()
            };

            return result;
        }
        #endregion

        #region Location

        public async Task<LocationModel> GetById(string id)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var result = await _locationCollection.FindAsync<LocationModel>(filter);
            return result.FirstOrDefault();
        }
        public async Task<LocationModel> GetByLocationId(string locationId)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("LocationId", locationId);
            var result = await _locationCollection.FindAsync<LocationModel>(filter);
            return result.FirstOrDefault();
        }
        public async Task<LocationModel> GetByLocationCode(string locationCode)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("Code", locationCode);
            var result = await _locationCollection.FindAsync<LocationModel>(filter);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<LocationSelectionItem>> GetSelectionLocation(LocationRequestFilterModel filterModel)
        {
            var builder = Builders<LocationModel>.Filter;

            FilterDefinition<LocationModel> filters = builder.Empty;

            #region Where clauses builder

            CreateFilter(filterModel, builder, ref filters);

            if (!string.IsNullOrWhiteSpace(filterModel.ParentId))
            {
                filters &= (Builders<LocationModel>.Filter.Eq(x => x.ParentId, filterModel.ParentId));
            }

            if (filterModel.MaxLevel.HasValue)
            {
                filters &= (Builders<LocationModel>.Filter.Lte(x => x.Level, filterModel.MaxLevel.Value));
            }

            if (filterModel.MinLevel.HasValue)
            {
                filters &= (Builders<LocationModel>.Filter.Gte(x => x.Level, filterModel.MinLevel.Value));
            }

            #endregion

            var query = await _locationCollection
                .FindAsync(filters, new FindOptions<LocationModel, LocationSelectionItem>()
                {
                    Projection = Builders<LocationModel>.Projection.Expression(x => new LocationSelectionItem()
                    {
                        Code = x.Code,
                        Text = x.Name,
                        ParentId = x.ParentId,
                        GlobalValue = x.LocationId,
                        Value = x.LocationId,
                        Path = x.Path,
                        Level = x.Level,
                        DisplayOrder = x.DisplayOrder
                    })
                });

            return query?.ToList();
        }

        public async Task<IEnumerable<LocationModel>> GetLocationByLevel(int level)
        {
            var builder = Builders<LocationModel>.Filter;

            FilterDefinition<LocationModel> filters = builder.Empty;
            filters = filters & builder.Eq("Level", level);
            return (await _locationCollection.FindAsync<LocationModel>(filters))?.ToList();
        }

        public virtual async Task<IEnumerable<LocationModel>> GetAll()
        {
            var all = await _locationCollection.FindAsync<LocationModel>(Builders<LocationModel>.Filter.Empty);
            return all.ToList();
        }

        public async Task<IPagedList<LocationModel>> GetListLocation(LocationRequestFilterModel filterModel)
        {
            var builder = Builders<LocationModel>.Filter;

            FilterDefinition<LocationModel> filters = builder.Empty;
            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }

            var totalRecords = await _locationCollection.CountDocumentsAsync(filters);
            if (filterModel.Type == RequestType.SimpleAll)
            {
                filterModel.Take = Int32.MaxValue;
            }

            if (!string.IsNullOrWhiteSpace(filterModel.ParentId))
            {
                filters &= (Builders<LocationModel>.Filter.Eq(x => x.ParentId, filterModel.ParentId));
            }

            if (filterModel.MaxLevel.HasValue)
            {
                filters &= (Builders<LocationModel>.Filter.Lte(x => x.Level, filterModel.MaxLevel.Value));
            }

            if (filterModel.MinLevel.HasValue)
            {
                filters &= (Builders<LocationModel>.Filter.Gte(x => x.Level, filterModel.MinLevel.Value));
            }


            var query = await _locationCollection.FindAsync(
                filters,
                new FindOptions<LocationModel>()
                {
                    Skip = filterModel.Skip,
                    Limit = filterModel.Paging ? (int?)filterModel.Take : null
                });

            return new PagedList<LocationModel>(filterModel.Skip, filterModel.Paging ? filterModel.Take : Int32.MaxValue, int.Parse(totalRecords.ToString()))
            {
                Subset = query?.ToList()
            }; ;
        }

        public async Task<LocationModel> CreateLocation(LocationModel supportRequest)
        {
            supportRequest.DateCreated = DateTime.UtcNow;
            await _locationCollection.InsertOneAsync(supportRequest);
            return supportRequest;
        }

        public async Task<LocationModel> UpdateLocation(LocationModel location)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("Id", location.Id);
            await _locationCollection.ReplaceOneAsync(filter, location);
            var successResult = await _locationCollection.FindAsync<LocationModel>(filter);
            return successResult?.FirstOrDefault();
        }

        public async Task<string> DeleteLocation(string id)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var deleteResult = await _locationCollection.DeleteOneAsync(filter);
            return deleteResult.DeletedCount > 0 ? "Success" : "Failed";
        }
        #endregion
        public async Task<bool> IsExisted(string id)
        {
            var filterBuilder = Builders<LocationModel>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var query = await _locationCollection.FindAsync<LocationModel>(filter);
            return query?.Any() == true;
        }

        private void CreateFilter<TProjection>(RequestFilterModel filterModel, FilterDefinitionBuilder<TProjection> builder, ref FilterDefinition<TProjection> filters)
        {
            foreach (var item in filterModel.PropertyFilterModels)
            {
                var safeFilterValue = this.ConvertToSafeValue<TProjection>(item.Field, item.FilterValue);
                switch (item.Operator)
                {
                    case "contains":
                        filters &= builder.Regex(item.Field, new BsonRegularExpression(@$"{item.FilterValue}"));
                        break;
                    case "doesnotcontain":
                        filters &= !builder.Regex(item.Field, new BsonRegularExpression(@$"{item.FilterValue}"));
                        break;
                    case "eq":
                        filters &= builder.Eq(item.Field, safeFilterValue);
                        break;
                    case "neq":
                        filters &= builder.Ne(item.Field, safeFilterValue);
                        break;
                    case "startswith":
                        filters &= builder.Regex(item.Field, new BsonRegularExpression(@$"^{item.FilterValue}"));
                        break;
                    case "endswith":
                        filters &= builder.Regex(item.Field, new BsonRegularExpression(@$"{item.FilterValue}$"));
                        break;
                    case "isnull":
                        filters &= builder.Eq(item.Field, BsonNull.Value);
                        break;
                    case "isnotnull":
                        filters &= builder.Ne(item.Field, BsonNull.Value);
                        break;
                    case "isempty":
                        filters &= builder.Eq(item.Field, string.Empty);
                        break;
                    case "isnotempty":
                        filters &= builder.Ne(item.Field, string.Empty);
                        break;
                    default:
                        filters &= builder.Regex(item.Field, new BsonRegularExpression(@$"{item.FilterValue}"));
                        break;
                }

            }
        }

        private Type GetTypeOfProperty<TResult>(string propertyName)
        {
            var resultType = typeof(TResult);
            return resultType.GetProperty(propertyName.ToUpperFirstLetter())?.PropertyType;

        }
        private object ConvertToSafeValue<TProjection>(string field, object valueObject)
        {
            var valueType = this.GetTypeOfProperty<TProjection>(field);

            if ("null".Equals(valueObject?.ToString(), StringComparison.OrdinalIgnoreCase) ||
                "undefined".Equals(valueObject?.ToString(), StringComparison.OrdinalIgnoreCase) ||
                valueObject == null)
            {
                return BsonNull.Value;
            }

            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.Boolean:
                    return "true".Equals(valueObject?.ToString(), StringComparison.OrdinalIgnoreCase);
                case TypeCode.DateTime:
                    if (!DateTime.TryParse(valueObject?.ToString(), out var safeTypedValue))
                    {
                        throw new FormatException("String was not recognized as a valid DateTime");
                    }

                    return new DateTime(safeTypedValue.Year, safeTypedValue.Month,
                        safeTypedValue.Day, 0, 0, 0);
                default:
                    try
                    {
                        return Convert.ChangeType(valueObject, valueType);
                    }
                    catch (InvalidCastException)
                    {
                        return valueObject;
                    }
                    catch (FormatException)
                    {
                        return valueObject;
                    }
                    catch (OverflowException)
                    {
                        return valueObject;
                    }
            }
        }
    }
}
