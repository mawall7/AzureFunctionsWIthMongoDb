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
    public class DeleteFunction
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public MyMongoDb Db { get; set; }
        private dynamic Result { get; set; }
        public DeleteFunction()
        {
            Db = new MyMongoDb();
            MaCollection = Db.MaCollection;

        }

        [FunctionName("Function4")]

        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "delete/{Id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                //Machine updatedItem = JsonConvert.DeserializeObject<Machine>(requestBody);
                //updatedItem.Id = id;
                var DeletedItem = Db.MaCollection.Find(m => m.Id == id).FirstOrDefault();
                var item = JsonConvert.SerializeObject(DeletedItem);
                Db.MaCollection.DeleteOne(m => m.Id == id);
                

                if (id == null)
                {
                    Result = new BadRequestObjectResult("Id can't be found, Wrong input?");
                }
                Result = new OkObjectResult($"item: {item}, deleted successfully");

            }
       
            catch
            {

            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return Result;

        }
    }
}

