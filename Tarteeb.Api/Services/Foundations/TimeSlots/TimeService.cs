//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.TimeSlots
{
    public partial class TimeService : ITimeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TimeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)

        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Time> ModifyTimeAsync(Time time) =>
        TryCatch(async () =>
        {
            ValidateTimeOnModify(time);
            var maybeTime = await this.storageBroker.SelectTimeByIdAsync(time.Id);

            ValidateStorageTime(maybeTime, maybeTime.Id);

            return await this.storageBroker.UpdateTimeAsync(time);
        });
    }
}
