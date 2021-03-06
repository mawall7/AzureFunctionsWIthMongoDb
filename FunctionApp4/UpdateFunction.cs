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

        [FunctionName("UpdateFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route="Update/{Id}")] HttpRequest req,
            ILogger log, string Id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                    Machine updatedItem = JsonConvert.DeserializeObject<Machine>(requestBody);
                    updatedItem.Id = Id;
                    var ReplacedItem = Db.MaCollection.ReplaceOne(m=> m.Id == Id,updatedItem);


                if (ReplacedItem.MatchedCount == 0)
                {
                    Result = new BadRequestObjectResult("Id can't be found, Wrong input format?");
                }
                else
                {
                    var data = Db.MaCollection.Find(m => m.Id == Id).FirstOrDefault();
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

