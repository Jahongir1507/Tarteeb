//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using System;
using System.Linq.Expressions;
using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Services.Foundations;
using Tarteeb.Api.Services.Foundations.Teams;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests 
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITeamService teamService;

        public TeamServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.teamService = new TeamService(
                this.storageBrokerMock.Object,
                this.loggingBrokerMock.Object);
        }

        private Expression<Func<Xeption,bool>> SameExceptionAs(Xeption expectedExceptoin) =>
           actualException => actualException.SameExceptionAs(expectedExceptoin);

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Team CreateRandomTeam() =>
            CreateTeamFiller().Create();

        private static Filler<Team> CreateTeamFiller()
        {
            var filler = new Filler<Team>();
            DateTimeOffset dates = GetRandomDateTime();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}