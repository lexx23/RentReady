using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RentReady.Common.Entity;
using RentReady.Functions.Functions;
using RentReady.Tests.Common;

namespace RentReady.Tests
{
    [TestClass]
    public class AddDateRangeFunctionTest : FunctionTestBase
    {
        private readonly AddTimeEntity _function;
        private readonly TimeEntriesRepositoryMock _timeEntriesRepositoryMock;

        public AddDateRangeFunctionTest()
        {
            _timeEntriesRepositoryMock = new TimeEntriesRepositoryMock();
            
            _function = new AddTimeEntity(_timeEntriesRepositoryMock.Object);
        }


        [TestMethod]
        public async Task EndOnGreaterOrEqualStartOn()
        {
            var body = JsonConvert.SerializeObject(new DateRangeEntity()
            {
                StartOn = DateTime.Now,
                EndOn = DateTime.Now.AddSeconds(-1)
            });

            RequestMock.AddBody(body);

            var result = await _function.RunAsync(RequestMock.Request, Logger, CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));   
        }
        
        [TestMethod]
        public async Task EndOnNotEmpty()
        {
            var body = "{\"startOn\" = \"" + DateTime.Now + "\"}";

            RequestMock.AddBody(body);
            
            var result = await _function.RunAsync(RequestMock.Request, Logger, CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult)); 
        }
        
        [TestMethod]
        public async Task StartOnNotEmpty()
        {
            var body = "{\"endOn\" = \"" + DateTime.Now + "\"}";

            RequestMock.AddBody(body);
            
            var result = await _function.RunAsync(RequestMock.Request, Logger, CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult)); 
        }
        
        
        [TestMethod]
        public async Task IgnoreExistingRecords()
        {
            var baseDate = DateTime.Parse("2021-12-15 08:00:00").Date;
            
            _timeEntriesRepositoryMock.Clear();
            _timeEntriesRepositoryMock.InsertRecord(new TimeEntryEntity(string.Empty, baseDate.AddDays(-9), baseDate.AddDays(-8)));
            _timeEntriesRepositoryMock.InsertRecord(new TimeEntryEntity(string.Empty, baseDate, baseDate));
            _timeEntriesRepositoryMock.InsertRecord(new TimeEntryEntity(string.Empty, baseDate.AddDays(8), baseDate.AddDays(9)));
            
            Assert.AreEqual(3, _timeEntriesRepositoryMock.RecordsCount);
            
            var body = JsonConvert.SerializeObject(new DateRangeEntity()
            {
                StartOn = baseDate.AddDays(-10),
                EndOn = baseDate.AddDays(10)
            });

            RequestMock.AddBody(body);
            
            var result = await _function.RunAsync(RequestMock.Request, Logger, CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            
            Assert.AreEqual(21, _timeEntriesRepositoryMock.RecordsCount);
        }
    }
}