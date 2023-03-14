//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models.Teams;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldAddTeamAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDateTime);
            Team inputTeam = randomTeam;
            Team persistedTeam = inputTeam;
            Team expectedTeam = persistedTeam.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeamAsync(inputTeam))
                    .ReturnsAsync(persistedTeam);

            // when
            Team actuaTeam = await this.teamService
                .AddTeamAsync(inputTeam);

            // then
            actuaTeam.Should().BeEquivalentTo(expectedTeam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeamAsync(inputTeam), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}