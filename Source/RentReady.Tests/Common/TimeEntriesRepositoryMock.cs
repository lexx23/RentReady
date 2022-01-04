using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using RentReady.Common.Entity;
using RentReady.Common.Interfaces;
using RentReady.Common.Interfaces.Repository;

namespace RentReady.Tests.Common
{
    public class TimeEntriesRepositoryMock
    {
        private readonly Mock<ITimeEntriesRepository> _timeEntriesRepositoryMock;
        private readonly List<TimeEntryEntity> _data;

        public int RecordsCount => _data.Count;
        public ITimeEntriesRepository Object => _timeEntriesRepositoryMock.Object;

        public TimeEntriesRepositoryMock()
        {
            _data = new List<TimeEntryEntity>();

            _timeEntriesRepositoryMock = new Mock<ITimeEntriesRepository>();
            _timeEntriesRepositoryMock.Setup(x => x.GetAsync(It.IsAny<DateTime[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DateTime[] dates, CancellationToken token) => _data.ToArray());


            _timeEntriesRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<TimeEntryEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TimeEntryEntity entity, CancellationToken token) => InsertRecord(entity));
        }

        public Guid InsertRecord(TimeEntryEntity entity)
        {
            _data.Add(entity);
            return Guid.NewGuid();
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}