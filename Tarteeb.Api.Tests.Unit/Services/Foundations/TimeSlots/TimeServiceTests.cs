//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Services.Foundations.Times;
using Tynamix.ObjectFiller;
using Xeptions;

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

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedExceptoin) =>
            actualException => actualException.SameExceptionAs(expectedExceptoin);

        private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private IQueryable<Time> CreateRandomTimes()
        {
            return CreateTimeFiller(dates: GetRandomDateTime())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static int GetRandomNumber() =>
             new IntRange(min: 2, max: 99).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomMessage() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Filler<Time> CreateTimeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Time>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
