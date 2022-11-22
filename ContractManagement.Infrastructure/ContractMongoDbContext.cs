using ContractManagement.Infrastructure.Repositories.ContractHistoryRepository;
using Microsoft.Extensions.Options;
using MongoClusterRepository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure
{
    public interface IContractMongoDbContext
    {
        IMongoClusterCollection<TDocument> GetCollection<TDocument>();
    }

    public class ContractMongoDbContext : IContractMongoDbContext
    {
        public List<IMongoDatabase> Databases { get; set; }
        public ContractMongoDbContext(IOptions<ContractMongoDbSettings> settingOpt)
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
