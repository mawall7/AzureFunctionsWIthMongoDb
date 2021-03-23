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
    public class UpdateFunction
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public MyMongoDb Db { get; set; }
        private dynamic Result { get; set; }
        public UpdateFunction()
        {
            Db = new MyMongoDb();
            MaCollection = Db.MaCollection;

        }

        [FunctionName("Function3")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "update/{Id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                    Machine updatedItem = JsonConvert.DeserializeObject<Machine>(requestBody);
                    updatedItem.Id = id;
                    var ReplacedItem = Db.MaCollection.ReplaceOne(m=> m.Id == id,updatedItem);


                if (ReplacedItem.MatchedCount == 0)
                {
                    Result = new BadRequestObjectResult("Id can't be found, Wrong input format?");
                }
                else
                {
                    var data = Db.MaCollection.Find(m => m.Id == id).FirstOrDefault();
                    var jsondata = JsonConvert.SerializeObject(data);
                    Result = new OkObjectResult($"Update of an item: {jsondata}, was successfull");
                }
            }
               
            catch
            {

            }

            
            return Result;
        }
    }
}

