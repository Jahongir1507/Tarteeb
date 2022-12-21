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
using Tarteeb.Api.Models.Tickets;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldModifyTeamAsync()
        {
            //given
            int randomDaysAgo = GetRandomNegativeNumber();
            DateTimeOffset randomDate = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDate);
            randomTeam.CreatedDate = randomDate.AddDays(randomDaysAgo);
            Team inputTeam = randomTeam;
            Team storageTeam = inputTeam;
            Team updatedTeam = inputTeam;
            Team expectedTeam = updatedTeam.DeepClone();
            Guid inputTeamId = inputTeam.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(inputTeamId)).ReturnsAsync(storageTeam);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTeamAsync(inputTeam)).ReturnsAsync(updatedTeam);

            //when
            Team actualTeam = await this.teamService.ModifyTeamAsync(inputTeam);

            //then
            actualTeam.Should().BeEquivalentTo(expectedTeam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(inputTeamId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeamAsync(inputTeam), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
