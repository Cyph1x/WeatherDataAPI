using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WeatherDataAPI.Services
{
    public class MongoConnection
    {
        // Dependency Injection Setup
        private readonly IOptions<DefaultMongoConnection> _mongoConnection;
        public MongoConnection(IOptions<DefaultMongoConnection> mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_mongoConnection.Value.ConnectionString);
            return client.GetDatabase(_mongoConnection.Value.DatabaseName);
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            var client = new MongoClient(_mongoConnection.Value.ConnectionString);
            return client.GetDatabase(databaseName);
        }

        public IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}
