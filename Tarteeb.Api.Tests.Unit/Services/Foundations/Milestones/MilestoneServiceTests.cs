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
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Tarteeb.Api.Models.Foundations.Teams;
using Tarteeb.Api.Services.Foundations.Milestones;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Milestones
{
    public partial class MilestoneServiceTests
    {

        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IMilestoneService milestoneService;

        public MilestoneServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.milestoneService = new MilestoneService(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object);
        }
        private Expression<Func<Exception, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTime() =>
           new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Milestone CreateRandomMilestone(DateTimeOffset date) =>
          CreateMilestoneFiller(date).Create();

        private static Filler<Milestone> CreateMilestoneFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Milestone>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
