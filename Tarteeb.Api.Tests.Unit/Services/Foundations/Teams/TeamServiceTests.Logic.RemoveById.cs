//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

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
        public async Task ShouldRemoveTeamByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTeamId = randomId;
            Team randomTeam = CreateRandomTeam();
            Team storageTeam = randomTeam;
            Team expectedInputTeam = storageTeam;
            Team deletedTeam = expectedInputTeam;
            Team expectedTeam = deletedTeam.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(inputTeamId))
                    .ReturnsAsync(storageTeam);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTeamAsync(expectedInputTeam))
                    .ReturnsAsync(deletedTeam);

            // when
            Team actualTeam = await this.teamService
                .RemoveTeamByIdAsync(inputTeamId);

            // then
            actualTeam.Should().BeEquivalentTo(expectedTeam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(inputTeamId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeamAsync(expectedInputTeam), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
