using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public class GetLastItem
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public MyMongoDb Db { get; set; }
        public IMongoClient Client { get; set; }
        public dynamic Result { get; set; }

        public GetLastItem()
        {
            var Db = new MyMongoDb();
            MaCollection = Db.MaCollection;
           

        }

        [FunctionName("Function5")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="LastItem")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
                                  

            try
            {
                Result = MaCollection.Find(m => true).SortByDescending(m => m.TimeCreated).FirstOrDefault();
              
                                
            }
            catch (Exception e)
            {
                throw new Exception("FindException", e);

            }
            return new OkObjectResult(Result);
        }
    }
}

