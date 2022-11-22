using Feedback.API.Models;
using Global.Models.Filter;
using Global.Models.PagedList;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using GenericRepository.Extensions;
using System.Threading.Tasks;
using MongoClusterRepository;

namespace Feedback.API.Queries
{
    public interface IFeedbackAndRequestQueries
    {
        Task<IPagedList<FeedbackAndRequest>> GetList(FeedbackAndRequestFilterModel map);
        Task<FeedbackAndRequest> Get(string id);
        IEnumerable<FeedbackAndRequest> GetByIds(string cIds);
        IEnumerable<(string, int)> CountingFeedbacksByCId(string cIds);
        IEnumerable<FeedbackAndRequest> GetUnresolvedFeedbacksByCIds(string cIds);
        IEnumerable<FeedbackAndRequest> GetAllUnhandledByCIdsNotReductionYet(string cIds, int includeReceiptLineId);
    }
    public class FeedbackAndRequestQueries : IFeedbackAndRequestQueries
    {
        private readonly IMongoClusterCollection<FeedbackAndRequest> _collection;

        public FeedbackAndRequestQueries(IFeedbackMongoDbContext context)
        {
            _collection = context.GetCollection<FeedbackAndRequest>();
        }

        public async Task<FeedbackAndRequest> Get(string id)
        {
            var filterBuilder = Builders<FeedbackAndRequest>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            var result = await _collection.FindAsync<FeedbackAndRequest>(filter);
            return result.FirstOrDefault();
        }

        public async Task<IPagedList<FeedbackAndRequest>> GetList(FeedbackAndRequestFilterModel filterModel)
        {
            var builder = Builders<FeedbackAndRequest>.Filter;

            FilterDefinition<FeedbackAndRequest> filters = builder.Empty;
            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }

            if (!string.IsNullOrEmpty(filterModel.CId))
            {
                filters = filters & builder.Eq("cId", filterModel.CId);
            }

            if (!string.IsNullOrEmpty(filterModel.CustomerId))
            {
                filters = filters & builder.Eq("customerId", filterModel.CustomerId);
            }

            if (!string.IsNullOrEmpty(filterModel.Source))
            {
                filters = filters & builder.Eq("source", filterModel.Source);
            }

            var totalRecords = (int)await _collection.CountDocumentsAsync(filters);
            var subSetResult = await _collection.FindAsync(filters,
                new FindOptions<FeedbackAndRequest>()
                {
                    Skip = filterModel.Skip,
                    Limit = filterModel.Take,
                    Sort = Sort("DateCreated", false)
                });

            var result = new PagedList<FeedbackAndRequest>(filterModel.Skip, filterModel.Take, totalRecords)
            {
                Subset = subSetResult?.ToList()
            };
            return result;
        }

        private static SortDefinition<Models.FeedbackAndRequest> Sort(string key, bool asc = true)
        {
            return asc
                ? Builders<Models.FeedbackAndRequest>.Sort.Ascending(key)
                : Builders<Models.FeedbackAndRequest>.Sort.Descending(key);
        }

        private void CreateFilter(RequestFilterModel filterModel, FilterDefinitionBuilder<FeedbackAndRequest> builder,
                                        ref FilterDefinition<FeedbackAndRequest> filters)
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

        public IEnumerable<FeedbackAndRequest> GetUnresolvedFeedbacksByCIds(string cIds)
        {
            if (string.IsNullOrWhiteSpace(cIds))
            {
                return Enumerable.Empty<FeedbackAndRequest>();
            }

            var ignoreCaseCIds = cIds.ToUpper().Split(',');

            return _collection.AsQueryable()
                .Where(c => !string.IsNullOrEmpty(c.CId)
                    && ignoreCaseCIds.Contains(c.CId)
                    && c.Status == 0)
                .OrderByDescending(c => c.DateCreated)
                .ToList();

        }

        public IEnumerable<FeedbackAndRequest> GetByIds(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return Enumerable.Empty<FeedbackAndRequest>();
            }

            var idArray = ids.Split(',');

            return _collection.AsQueryable()
                .Where(c => idArray.Contains(c.Id))
                .OrderByDescending(c => c.DateCreated)
                .ToList();

        }

        public IEnumerable<(string, int)> CountingFeedbacksByCId(string cIds)
        {
            if (string.IsNullOrEmpty(cIds))
            {
                return Enumerable.Empty<(string, int)>();
            }

            var cIdArray = cIds.ToUpper().Split(',');

            var query = _collection
                .AsQueryable()
                .Where(c => cIdArray.Contains(c.CId) && !c.Handled && c.Status == (int)FeedbackAndRequestStatus.Pending)
                .GroupBy(c => c.CId)
                .Select(g => new
                {
                    CId = g.Key,
                    FeedbackCount = g.Count()
                });

            return query
                .ToList()
                .Select(r => (r.CId, r.FeedbackCount));
        }

        public IEnumerable<FeedbackAndRequest> GetAllUnhandledByCIdsNotReductionYet(string cIds, int includeReceiptLineId)
        {
            if (string.IsNullOrWhiteSpace(cIds))
            {
                return Enumerable.Empty<FeedbackAndRequest>();
            }

            var ignoreCaseCIds = cIds.ToUpper().Split(',');

            var queryable = _collection.AsQueryable()
                .Where(c => !string.IsNullOrEmpty(c.CId)
                    && ignoreCaseCIds.Contains(c.CId)
                    && (c.ReceiptLineId == 0 || c.ReceiptLineId == includeReceiptLineId));

            return queryable;
        }
    }
}
