using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public class Function1
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public MyMongoDb Db { get; set; }
        public IMongoClient Client { get; set; }
        public dynamic Result { get; set; }

        public Function1()
        {
            var Db = new MyMongoDb();
            MaCollection = Db.MaCollection;

            /*Client = new MongoClient(Environment.GetEnvironmentVariable("Connection_String").ToString());
            Db = Client.GetDatabase("test");
            MaCollection = Db.GetCollection<Machine>("Machines*/
            
            
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getAll")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
                               
           // string requestBody = new StreamReader(req.Body).ReadToEnd();
           // dynamic data = JsonConvert.DeserializeObject(requestBody);

            try
            {
                Result= MaCollection.Find(m => true).ToList();
                
            }
            catch(Exception e)
            {
                throw new Exception("FindException", e);
                
            }
            return new OkObjectResult(Result);
            
        }
    }
}

