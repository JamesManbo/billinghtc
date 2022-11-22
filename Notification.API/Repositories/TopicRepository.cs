using MongoClusterRepository;
using MongoDB.Driver;
using Notification.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Repositories
{
    public interface ITopicRepository
    {
        Task<long> BulkInsert(List<Topic> notifications);
        Task<Topic> Add(Topic obj);
        Task<Topic> GetByName(string name);
    }
    public class TopicRepository : ITopicRepository
    {
        protected readonly IMongoClusterCollection<Topic> Collection;
        public TopicRepository(INotificationDbContext context)
        {
            this.Collection = context.GetCollection<Topic>();
        }
        public virtual async Task<Topic> Add(Topic obj)
        {
            obj.CreatedDate = DateTime.UtcNow;
            await Collection.InsertOneAsync(obj);
            return obj;
        }

        public virtual async Task<long> BulkInsert(List<Topic> notifications)
        {
            var writeModels = new List<WriteModel<Topic>>();
            for (int i = 0; i < notifications.Count; i++)
            {
                writeModels.Add(new InsertOneModel<Topic>(notifications.ElementAt(i)));
            }

            var writeResult
                = await this.Collection.BulkWriteAsync(writeModels);

            return writeResult.InsertedCount;
        }

        public async Task<Topic> GetByName(string name)
        {
            var data = await Collection.FindAsync<Topic>(FilterName(name));
            return data.FirstOrDefault();
        }

        private static FilterDefinition<Topic> FilterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Builders<Topic>.Filter.Empty;
            }

            return Builders<Topic>.Filter.Where(r => r.Name == name.Trim());
        }
    }
}
