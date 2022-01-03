using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RentReady.Common.Entity;

namespace RentReady.Functions.Functions
{
    public static class AddDateRange
    {
        [FunctionName("AddDateRange")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody) || !TryParseJson<DateRange>(requestBody, out var dateRange))
                return new BadRequestObjectResult("Please pass a StartOn,EndOn in the request body");
                
            
            return new JsonResult(new { Id = 1 });
        }
        
        private static bool TryParseJson<T>(this string data, out T result)
        {
            var success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(data, settings);
            return success;
        }
    }
}