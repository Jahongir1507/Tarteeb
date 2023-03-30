//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
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

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));


        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedExceptoin) =>
            actualException => actualException.SameExceptionAs(expectedExceptoin);

        private DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Time CreateRandomTime() =>
            CreateTimeFiller(GetRandomDateTimeOffset()).Create();

        private Filler<Time> CreateTimeFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Time>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
