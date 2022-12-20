//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidTeamId = Guid.Empty;
            var invalidTeamException = new InvalidTeamException();

            invalidTeamException.AddData(
                key: nameof(Team.Id),
                values: "Id is required");

            var expectedTeamValidationException = new 
                TeamValidationException(invalidTeamException);

            // when
            ValueTask<Team> retrieveTeamByIdTask =
                this.teamService.RetrieveTeamByIdAsync(invalidTeamId);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    retrieveTeamByIdTask.AsTask);

            // then
            actualTeamValidationException.Should().BeEquivalentTo(expectedTeamValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTeamIsNotFoundAndLogItAsync()
        {
            //given
            Guid someTeamId = Guid.NewGuid();
            Team noTeam = null;

            var notFoundTeamException =
                new NotFoundTeamException(someTeamId);

            var expectedTeamValidationException =
                new TeamValidationException(notFoundTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noTeam);

            //when
            ValueTask<Team> retrieveTeamByIdTask =
                this.teamService.RetrieveTeamByIdAsync(someTeamId);

            TeamValidationException actualTeamValidationException =
                await Assert.ThrowsAsync<TeamValidationException>(
                    retrieveTeamByIdTask.AsTask);

            // then
            actualTeamValidationException.Should().BeEquivalentTo(expectedTeamValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
