using System.Collections.Generic;
using Location.API.Location;
using Microsoft.Extensions.Options;
using MongoClusterRepository;
using MongoDB.Driver;

namespace Location.API
{
    public interface ILocationkMongoDbContext
    {
        IMongoClusterCollection<TDocument> GetCollection<TDocument>();
    }
    public class LocationMongoDbContext : ILocationkMongoDbContext
    {
        public List<IMongoDatabase> Databases { get; set; }

        public LocationMongoDbContext(IOptions<LocationMongoDbSettings> settingOpt)
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