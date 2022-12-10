using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Services.Foundations.Teamss;
using Tarteeb.Api.Services.Foundations.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teamss
{
    public partial class TeamServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ITeamService teamService;

        public TeamServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.teamService = new TeamService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static IQueryable<Team> CreateRandomTeam() =>
            CreateTeamFiller().Create(count: GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
        private static Filler<Team> CreateTeamFiller()
        {
            var filler = new Filler<Team>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset());

            return filler;
        }
    }
}
