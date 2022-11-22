using MongoDB.Driver;
using System.Collections.Generic;
using Feedback.API.Models;
using Microsoft.Extensions.Options;
using MongoClusterRepository;

namespace Feedback.API
{
    public interface IFeedbackMongoDbContext
    {
        IMongoClusterCollection<TDocument> GetCollection<TDocument>();
    }

    public class FeedbackMongoDbContext : IFeedbackMongoDbContext
    {
        public List<IMongoDatabase> Databases { get; set; }
        public FeedbackMongoDbContext(IOptions<FeedbackMongoDbSettings> settingOpt)
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
                    Credential = mongoCredential,
                    ConnectTimeout = new System.TimeSpan(0, 0, 60)
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
