using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionApp4
{
    public class MyMongoDb
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public IMongoDatabase Db { get; set; }

        public IMongoClient Client { get; set; }
        public MyMongoDb()
        {

            Client = new MongoClient(Environment.GetEnvironmentVariable("Connection_String"));
            Db = Client.GetDatabase("test");
            MaCollection = Db.GetCollection<Machine>("Machines");
        }
       
    }
}
