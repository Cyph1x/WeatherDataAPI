using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WeatherDataAPI.Services
{
    public class MongoConnection
    {
        // Dependency Injection Setup
        private string _connectionString;
        private string _databaseName;
        public MongoConnection(IOptions<DefaultMongoConnection> mongoConnection)
        {
            // Add services to the container.
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
            {
                _connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoCollection");
                _databaseName = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoConnection");
            }
            else
            {
                _connectionString = mongoConnection.Value.ConnectionString;
                _databaseName = mongoConnection.Value.DatabaseName;
            }
        }

        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_connectionString);
            return client.GetDatabase(_databaseName);
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            var client = new MongoClient(_connectionString);
            return client.GetDatabase(databaseName);
        }

        public IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}
