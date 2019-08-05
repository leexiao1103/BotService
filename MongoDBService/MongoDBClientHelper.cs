using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDBService
{
    public class MongoDBClientHelper<T>
    {
        public static IMongoCollection<T> GetCollection(MongoDBHostSetting host) {
            var client = new MongoClient(host.ConnectionString);
            var database = client.GetDatabase(host.DatabaseName);

            return database.GetCollection<T>(host.CollectionName);
        }        
    }
}
