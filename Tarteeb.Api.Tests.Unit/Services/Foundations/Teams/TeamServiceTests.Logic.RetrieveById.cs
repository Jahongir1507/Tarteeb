//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.Extensions.Hosting;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Tarteeb.Api.Models.Teams;
using Force.DeepCloner;
using FluentAssertions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveTeamByIdAsync()
        {
            // given
            Guid randomTeamId = Guid.NewGuid();
            Guid inputTeamId = randomTeamId;
            Team randomTeam = CreateRandomTeam();
            Team storageTeam = randomTeam;
            Team expectedTeam = storageTeam.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(inputTeamId)).ReturnsAsync(storageTeam);

            // when
            Team actualTeam =
                await this.teamService.RetrieveTeamByIdAsync(inputTeamId);

            // then
            actualTeam.Should().BeEquivalentTo(expectedTeam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(inputTeamId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
