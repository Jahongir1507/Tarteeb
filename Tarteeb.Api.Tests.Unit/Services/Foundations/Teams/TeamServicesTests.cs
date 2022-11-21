//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Services.Foundations.Teams;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    internal class TeamServicesTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITeamService teamService;

        public TeamServicesTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.teamService =
                new TeamService(this.storageBrokerMock.Object, this.loggingBrokerMock.Object);
        }
    }
}
