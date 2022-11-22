using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoClusterRepository;

namespace Notification.API
{
    public interface INotificationDbContext
    {
        IMongoClusterCollection<TDocument> GetCollection<TDocument>();
    }
    public class NotificationDbContext : INotificationDbContext
    {
        public List<IMongoDatabase> Databases { get; set; }
        public NotificationDbContext(IOptions<MongoDbSettings> settingOpt)
        {
            Databases = new List<IMongoDatabase>();
            var cnnSettings = settingOpt.Value;

            foreach (var setting in cnnSettings.Servers)
            {
                var internalIdentity = new MongoInternalIdentity("admin", setting.User);
                var passwordEvidence = new PasswordEvidence(setting.Password);
                var mongoCredential = new MongoCredential("SCRAM-SHA-1", internalIdentity, passwordEvidence);

                var address = new MongoServerAddress(setting.Host, setting.Port);
                var settings = new MongoClientSettings
                {
                    Server = address,
                    ConnectionMode = ConnectionMode.Standalone,
                    Credential = mongoCredential
                };

                var mongoClient = new MongoClient(settings);
                Databases.Add(mongoClient.GetDatabase(setting.DatabaseName));
            }
        }

        public IMongoClusterCollection<TDocument> GetCollection<TDocument>()
        {
            return new MongoClusterCollection<TDocument>(Databases);
        }
    }

}
