//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTeamIsNullAndLogItAsync()
        {
            // given
            Team nullTeam = null;
            var nullTeamException = new NullTeamException();

            var expectedTeamValidationException =
                new TeamValidationException(nullTeamException);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(nullTeam);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    modifyTeamTask.AsTask);

            // then
            actualTeamValidationException.Should().BeEquivalentTo(expectedTeamValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeamAsync(It.IsAny<Team>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTeamIsInvalidAndLogItAsync(string invalidString)
        {
            // given 
            var invalidTeam = new Team
            {
                TeamName = invalidString
            };

            var invalidTeamException =
                new InvalidTeamException();

            invalidTeamException.AddData(
                key: nameof(Team.Id),
                values: "Id is required");

            invalidTeamException.AddData(
                key: nameof(Team.TeamName),
                values: "Text is required");

            invalidTeamException.AddData(
                key: nameof(Team.CreatedDate),
                values: "Value is required");

            invalidTeamException.AddData(
                key: nameof(Team.UpdatedDate),
                 values: "Value is required"  
                );

            var expectedTeamValidationException =
                new TeamValidationException(invalidTeamException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(invalidTeam);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    modifyTeamTask.AsTask);

            //then
            actualTeamValidationException.Should()
                .BeEquivalentTo(expectedTeamValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeamAsync(It.IsAny<Team>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDateTime);
            Team invalidTeam = randomTeam;
            var invalidTeamException = new InvalidTeamException();

            invalidTeamException.AddData(
            key: nameof(Team.UpdatedDate),
                values: $"Date is the same as {nameof(Team.CreatedDate)}");

            var expectedTeamValidationException =
                new TeamValidationException(invalidTeamException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(invalidTeam);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    modifyTeamTask.AsTask);

            // then
            actualTeamValidationException.Should()
                .BeEquivalentTo(expectedTeamValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(invalidTeam.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
