using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CreateFunction
    {
        public IMongoCollection<Machine> MaCollection { get; set; }
        public MyMongoDb Db { get; set; }
        private dynamic Result { get; set; }
        public CreateFunction()
        {
            Db = new MyMongoDb();
            MaCollection = Db.MaCollection;
            
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            bool result = true;
            string id = Guid.NewGuid().ToString();
            DateTime date = DateTime.Now;        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Machine input = JsonConvert.DeserializeObject<Machine>(requestBody);
            //ICollection<ValidationResult> ErrorList = null;

                        
            while (Db.MaCollection.Find(m => m.Id == id).Any() == true) 
            {
                id = Guid.NewGuid().ToString();
               
            }
            
            
            try { 
                    if(input.Name!= null)
                    {
                        input.Id = id;
                        input.TimeCreated = DateTime.Now;
                        Db.MaCollection.InsertOne(input);
                        Result = new OkObjectResult($"Inserted Machine Successfull{requestBody}");
                    }
                            
                /*if (Validator.TryValidateObject(input, new ValidationContext(input), ErrorList, true)) 
                {
                    Db.MaCollection.InsertOne(input);
                    Result = new OkObjectResult($"Inserted Machine Successfull{requestBody}");
                }*/
                else
                {
                    Result = new BadRequestObjectResult("Try again!, input not Allowed , Name required");
                }
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

