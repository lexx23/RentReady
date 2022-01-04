using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RentReady.Common.Entity;
using RentReady.Common.Interfaces.Repository;
using RentReady.Common.Options;

namespace RentReady.DAL.Repository
{
    internal class TimeEntriesRepository : ITimeEntriesRepository
    {
        // TODO: look for existing constants
        private const string TableName = "msdyn_timeentry";
        private const string IdFieldName = "msdyn_timeentryid";
        private const string StartFieldName = "msdyn_start";
        private const string EndFieldName = "msdyn_end";
        private const string DurationFieldName = "msdyn_duration";

        private readonly ServiceClient _serviceClient;
        private readonly Dataverse _dataverseSettings;

        public TimeEntriesRepository(ILogger<TimeEntriesRepository> logger, IOptions<Dataverse> dataverseOptions)
        {
            _dataverseSettings = dataverseOptions.Value;

            if (string.IsNullOrEmpty(_dataverseSettings.UserName) || string.IsNullOrEmpty(_dataverseSettings.Password) || string.IsNullOrEmpty(_dataverseSettings.Environment))
                throw new ArgumentException("Dataverse username,password or environment variables not found or empty");


            var connectionString = @$"Url=https://{_dataverseSettings.Environment}.dynamics.com;AuthType=OAuth;UserName={_dataverseSettings.UserName};Password={_dataverseSettings.Password};";

            _serviceClient = new ServiceClient(connectionString, logger);
        }


        public async Task<TimeEntryEntity[]> GetAsync(DateTime start, DateTime end, CancellationToken token)
        {
            var collection = await _serviceClient.RetrieveMultipleAsync(new QueryExpression(TableName)
            {
                ColumnSet = new ColumnSet(StartFieldName, EndFieldName),
                Criteria = new FilterExpression
                {
                    Filters =
                    {
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(StartFieldName, ConditionOperator.GreaterEqual, start),
                                new ConditionExpression(EndFieldName, ConditionOperator.LessEqual, end)
                            }
                        }
                    }
                }
            }, token);

            return ToEntity(collection);
        }

        public async Task<TimeEntryEntity[]> GetAsync(DateTime[] dates, CancellationToken token)
        {
            var collection = await _serviceClient.RetrieveMultipleAsync(new QueryExpression(TableName)
            {
                ColumnSet = new ColumnSet(StartFieldName, EndFieldName),
                Criteria = new FilterExpression
                {
                    Filters =
                    {
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(StartFieldName, ConditionOperator.In, dates),
                            }
                        }
                    }
                }
            }, token);


            return ToEntity(collection);
        }

        public async Task<Guid> InsertAsync(TimeEntryEntity entity, CancellationToken token)
        {
            var timeEntity = new Entity(TableName)
            {
                Attributes = new AttributeCollection
                {
                    new(StartFieldName, entity.Start),
                    new(EndFieldName, entity.End),
                    new (DurationFieldName, (entity.End - entity.Start).Minutes)
                }
            };

            return await _serviceClient.CreateAsync(timeEntity, token);
        }


        private TimeEntryEntity[] ToEntity(EntityCollection collection)
        {
            if (collection.Entities.Count == 0)
                return Array.Empty<TimeEntryEntity>();

            return collection.Entities
                .Select(x =>
                    new TimeEntryEntity(
                        x.GetAttributeValue<Guid>(IdFieldName).ToString(),
                        x.GetAttributeValue<DateTime>(StartFieldName),
                        x.GetAttributeValue<DateTime>(EndFieldName)
                    )
                ).ToArray();
        }
    }
}