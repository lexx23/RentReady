using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RentReady.Common.Entity;
using RentReady.Common.Helper;
using RentReady.Common.Interfaces;
using RentReady.Common.Interfaces.Repository;

namespace RentReady.Functions.Functions
{
    public class AddTimeEntity
    {
        private readonly ITimeEntriesRepository _timeEntriesRepository;

        public AddTimeEntity(ITimeEntriesRepository timeEntriesRepository)
        {
            _timeEntriesRepository = timeEntriesRepository;
        }
        
        
        [FunctionName("AddTimeEntity")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "timeEntity")] HttpRequest req, ILogger log, CancellationToken token)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody) || !JsonFunctions.TryParseJson<DateRangeEntity>(requestBody, out var dateRange))
                return new BadRequestObjectResult("Please pass a startOn, endOn in the request body");
                
            if(dateRange.StartOn > dateRange.EndOn)
                return new BadRequestObjectResult("Param endOn must be greater than startOn");
            
            var dates = Enumerable.Range(0, 1 + dateRange.EndOn.Subtract(dateRange.StartOn).Days)
                .Select(offset => dateRange.StartOn.AddDays(offset))
                .ToArray(); 
            
            var existingRecords = await _timeEntriesRepository.GetAsync(dates, token);
            
            foreach (var timeEntryEntity in existingRecords)
            {
                log.LogInformation("Time entity with id:'{Id}' and StartDate:'{Start}' already exist",timeEntryEntity.Id, timeEntryEntity.Start);
            }

            var excludeDates = existingRecords.Select(x => x.Start).ToArray();
            var newDates = dates.Except(excludeDates);
            foreach (var newDate in newDates)
            {
                var id = await _timeEntriesRepository.InsertAsync(new TimeEntryEntity(String.Empty, newDate, newDate.AddMinutes(1)), token);
                log.LogInformation("Time entity with id:'{Id}' and StartDate/EndDate:'{Start}' was created",id, newDate);
            }
            
            return new OkResult();
        }
    }
}