//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq.Expressions;
using Moq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Services.Foundations.TimeSlots;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.TimeSlots
{
    public partial class TimeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ITimeService timeService;

        public TimeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.timeService = new TimeService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private Time CreateRandomModifyTime(DateTimeOffset randomDate)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            Time randomTime = CreateRandomTime(randomDate);

            randomTime.CreatedDate =
                randomTime.CreatedDate.AddDays(randomDaysInPast);

            return randomTime;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 9).GetValue();

        private static Time CreateRandomTime(DateTimeOffset dates) =>
           CreateTimeFiller(dates).Create();

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<int> InvalidSeconds()
        {
            int secondsInPast = -1 * new IntRange(
                min: 60,
                max: short.MaxValue).GetValue();

            int secondsInFuture = new IntRange(
                min: 0,
                max: short.MaxValue).GetValue();

            return new TheoryData<int>
            {
                secondsInPast,
                secondsInFuture
            };
        }

        private static Filler<Time> CreateTimeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Time>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
