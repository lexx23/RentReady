using System;
using System.Threading;
using System.Threading.Tasks;
using RentReady.Common.Entity;

namespace RentReady.Common.Interfaces.Repository
{
    public interface ITimeEntriesRepository
    {
        Task<TimeEntryEntity[]> GetAsync(DateTime start, DateTime end, CancellationToken token);
        Task<TimeEntryEntity[]> GetAsync(DateTime[] dates, CancellationToken token);
        public Task<Guid> InsertAsync(TimeEntryEntity entity, CancellationToken token);
    }
}