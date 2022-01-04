using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RentReady.Common.Entity;
using RentReady.Common.Helper;
using RentReady.Common.Interfaces;
using RentReady.Common.Interfaces.Repository;

namespace RentReady.Functions.Functions
{
    public class GetTimeEntities
    {
        private readonly ITimeEntriesRepository _timeEntriesRepository;

        public GetTimeEntities(ITimeEntriesRepository timeEntriesRepository)
        {
            _timeEntriesRepository = timeEntriesRepository;
        }


        [FunctionName("GetTimeEntities")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "timeEntity")] HttpRequest req, ILogger log, CancellationToken token)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody) || !JsonFunctions.TryParseJson<DateRangeEntity>(requestBody, out var dateRange))
                return new BadRequestObjectResult("Please pass a startOn, endOn in the request body");

            if (dateRange.StartOn > dateRange.EndOn)
                return new BadRequestObjectResult("Param endOn must be greater than startOn");

            var existingRecords = await _timeEntriesRepository.GetAsync(dateRange.StartOn, dateRange.EndOn, token);
            return new JsonResult(existingRecords);
        }
    }
}