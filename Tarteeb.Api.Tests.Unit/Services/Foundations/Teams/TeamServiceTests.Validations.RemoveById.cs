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

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidTeamId = Guid.Empty;

            var invalidTeamException = new InvalidTeamException();

            invalidTeamException.AddData(
                key: nameof(Team.Id),
                values: "Id is required");

            var expectedTeamValidationException =
                new TeamValidationException(invalidTeamException);

            // when
            ValueTask<Team> removeTeamByIdTask =
                this.teamService.RemoveTeamByIdAsync(invalidTeamId);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    removeTeamByIdTask.AsTask);

            // then
            actualTeamValidationException.Should().BeEquivalentTo(expectedTeamValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeamAsync(It.IsAny<Team>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfTeamIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomTeamId = Guid.NewGuid();
            Guid inputTeamId = randomTeamId;
            Team noTeam = null;

            var notFoundTeamException =
                new NotFoundTeamException(inputTeamId);

            var expectedTeamValidationException =
                new TeamValidationException(notFoundTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noTeam);

            // when
            ValueTask<Team> removeTeamByIdTask =
                this.teamService.RemoveTeamByIdAsync(inputTeamId);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    removeTeamByIdTask.AsTask);

            // then
            actualTeamValidationException.Should().BeEquivalentTo(expectedTeamValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeamAsync(It.IsAny<Team>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}