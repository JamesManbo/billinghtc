using FeedbackAndReports.API.Context;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Context
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;
        private readonly IConfiguration _configuration;

        public MongoContext(IConfiguration configuration)
        {
            _configuration = configuration;


            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
        }
        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();
            return Database.GetCollection<T>(name);
        }

        public async Task<int> SaveChanges()
        {
            ConfigureMongo();

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null)
                return;

            MongoInternalIdentity internalIdentity = new MongoInternalIdentity("admin", "admin");
            PasswordEvidence passwordEvidence = new PasswordEvidence("123@123a");
            MongoCredential mongoCredential = new MongoCredential("SCRAM-SHA-1", internalIdentity, passwordEvidence);
            //List<MongoCredential> credentials = new List<MongoCredential>() { mongoCredential };

            MongoClientSettings settings = new MongoClientSettings();

            settings.Credential = mongoCredential;
            String mongoHost = "location.mongo";
            MongoServerAddress address = new MongoServerAddress(mongoHost);
            settings.Server = address;

            //MongoClient = new MongoClient(_configuration["MongoSettings:Connection"]);
            MongoClient = new MongoClient(settings);

            Database = MongoClient.GetDatabase(_configuration["MongoSettings:DatabaseName"]);

        }
    }
}
