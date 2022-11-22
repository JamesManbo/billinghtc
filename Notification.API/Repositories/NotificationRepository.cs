using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.PagedList;
using MongoClusterRepository;
using MongoDB.Driver;
using Notification.API.Models;

namespace Notification.API.Repositories
{
    public interface INotificationRepository
    {
        Task<long> BulkInsert(List<Models.Notification> notifications);
        Task<Models.Notification> Add(Models.Notification obj);
        Task<Models.Notification> GetById(string id);
        Task<IEnumerable<Models.Notification>> GetByReceiver(string receiverId, int? take, int? skip);
        Task<IPagedList<Models.Notification>> GetPageListByReceiver(NotificationFilterModel request);
        Task<long> CountUnread(string receiverId);
        Task<Models.Notification> Update(string id, Models.Notification obj);
        Task<bool> Remove(string id);
        Task<bool> UpdateByIds(string[] ids);
    }

    public class NotificationRepository : INotificationRepository
    {
        protected readonly IMongoClusterCollection<Models.Notification> Collection;

        public NotificationRepository(INotificationDbContext context)
        {
            this.Collection = context.GetCollection<Models.Notification>();
        }

        public async Task<long> BulkInsert(List<Models.Notification> notifications)
        {
            var writeModels = new List<WriteModel<Models.Notification>>();
            for (int i = 0; i < notifications.Count; i++)
            {
                writeModels.Add(new InsertOneModel<Models.Notification>(notifications.ElementAt(i)));
            }

            var writeResult
                = await this.Collection.BulkWriteAsync(writeModels);

            return writeResult.InsertedCount;
        }

        public virtual async Task<Models.Notification> Add(Models.Notification obj)
        {
            obj.CreatedDate = DateTime.UtcNow;
            await Collection.InsertOneAsync(obj);
            return obj;
        }

        public virtual async Task<Models.Notification> GetById(string id)
        {
            var data = await Collection.FindAsync<Models.Notification>(FilterId(id));
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<Models.Notification>> GetByReceiver(string receiverId, int? take = 10,
            int? skip = 0)
        {
            var notifications = (await Collection.FindAsync(FilterReceiverId(receiverId),
                new FindOptions<Models.Notification>()
                {
                    Skip = skip,
                    Limit = take,
                    Sort = Sort("CreatedDate", false)
                })).ToList();

            return notifications;
        }


        public virtual async Task<Models.Notification> Update(string id, Models.Notification obj)
        {
            obj.UpdatedDate = DateTime.UtcNow;
            await Collection.ReplaceOneAsync(FilterId(id), obj);

            return obj;
        }

        public virtual async Task<bool> UpdateByIds(string[] ids)
        {
            UpdateDefinition<Models.Notification> updateDefinition = Builders<Models.Notification>.Update.Set(x => x.IsRead, true).Set(x => x.UpdatedDate, DateTime.UtcNow);
            var filter = Builders<Models.Notification>.Filter.Where(x => ids.Contains(x.Id));
            var result = await Collection.UpdateManyAsync(filter, updateDefinition);
            return result.IsAcknowledged;
        }

        public virtual async Task<bool> Remove(string id)
        {
            var result = await Collection.DeleteOneAsync(FilterId(id));
            return result.IsAcknowledged;
        }

        public Task<long> CountUnread(string receiverId)
        {
            var builder = Builders<Models.Notification>.Filter;
            FilterDefinition<Models.Notification> filterDefinition = builder
                .Eq(c => c.ReceiverId, receiverId);

            filterDefinition = filterDefinition & builder.Eq(c => c.IsRead, false);

            return Collection.CountDocumentsAsync(filterDefinition);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private static SortDefinition<Models.Notification> Sort(string key, bool asc = true)
        {
            return asc
                ? Builders<Models.Notification>.Sort.Ascending(key)
                : Builders<Models.Notification>.Sort.Descending(key);
        }

        private static FilterDefinition<Models.Notification> FilterId(string key)
        {
            return Builders<Models.Notification>.Filter.Eq("Id", key);
        }

        private static FilterDefinition<Models.Notification> FilterReceiverId(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Builders<Models.Notification>.Filter.Empty;
            }

            return Builders<Models.Notification>.Filter.Where(r => r.ReceiverId == key.Trim());
        }

        private static FilterDefinition<Models.Notification> FilterType(int key)
        {
            return Builders<Models.Notification>.Filter.Eq("Type", key);
        }
        
        private static FilterDefinition<Models.Notification> FilterIsRead(bool isRead)
        {
            return Builders<Models.Notification>.Filter.Eq("IsRead", isRead);
        }


        public async Task<IPagedList<Models.Notification>> GetPageListByReceiver(NotificationFilterModel request)
        {
            var filterAll = FilterReceiverId(request.ReceiverId);
            if (request.NotificationType.HasValue && request.NotificationType.Value > 0)
            {
                filterAll = filterAll & FilterType(request.NotificationType.Value);
            }
            if (request.IsRead.HasValue)
            {
                filterAll = filterAll & FilterIsRead(request.IsRead.Value);
            }

            var queryable = await Collection.FindAsync(filterAll, new FindOptions<Models.Notification>()
            {
                Skip = request.Skip,
                Limit = request.Take,
                Sort = Sort("CreatedDate", false)
            });

            var totalRecords = (int) await Collection.CountDocumentsAsync(filterAll);
            var result = new PagedList<Models.Notification>(request.Skip, request.Take, totalRecords)
            {
                Subset = queryable.ToList()
            };

            return result;
        }
    }
}