using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public class GetImageFunction
    {
        [FunctionName("GetImageFunction")]
        public  IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            IActionResult result;
            byte[] b = null;
            try
            {
                b = File.ReadAllBytes(@"C:\Users\matte\Downloads\Hello.jp");
                result = new FileContentResult(b, "image/jpg");
            }
            catch(FileNotFoundException e) 
            {
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return result; 
        }
    }
}

