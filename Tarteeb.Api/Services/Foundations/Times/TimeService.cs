//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService : ITimeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TimeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Time> AddTimeAsync(Time time) =>
        TryCatch(async () =>
        {
            ValidateTimeNotNull(time);

            return await this.storageBroker.InsertTimeAsync(time);

        });

        public IQueryable<Time> RetrieveAllTimes() =>
            TryCatch(() => this.storageBroker.SelectAllTimes());

        public ValueTask<Time> RemoveTimeByIdAsync(Guid timeId) =>
        TryCatch(async () =>
        {
            ValidateTimeId(timeId);

            Time maybeTime =
                await this.storageBroker.SelectTimeByIdAsync(timeId);

            ValidateStorageTimeExists(maybeTime, timeId);

            return await this.storageBroker.DeleteTimeAsync(maybeTime);
        });
    }
}
