//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Moq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Services.Foundations.TimeSlots;
using Tynamix.ObjectFiller;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITimeService timeService;
        public TimeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.timeService = new TimeService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Time CreateRandomModifyTime(DateTimeOffset randomDate)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            Time randomtime = CreateRandomTime(randomDate);

            randomtime.CreatedDate = randomtime.CreatedDate.AddDays(randomDaysInPast);

            return randomtime;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 9).GetValue();

        private static Time CreateRandomTime(DateTimeOffset dates) =>
           CreateTimeFiller(dates).Create();

        private static Filler<Time> CreateTimeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Time>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            return filler;
        }
    }
}
