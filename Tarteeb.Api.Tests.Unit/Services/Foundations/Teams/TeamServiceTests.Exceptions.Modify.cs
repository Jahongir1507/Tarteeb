//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(someDateTime);
            Team someTeam = randomTeam;
            Guid TeamId = someTeam.Id;
            SqlException sqlException = CreateSqlException();

            var failedTeamStorageException =
                new FailedTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(someTeam);

            TeamDependencyException actualTeamDependencyException =
                await Assert.ThrowsAsync<TeamDependencyException>(
                     modifyTeamTask.AsTask);

            // then
            actualTeamDependencyException.Should().BeEquivalentTo(
                expectedTeamDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(TeamId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeamAsync(someTeam), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDateTime);
            Team someTeam = randomTeam;
            Guid TeamId = someTeam.Id;
            someTeam.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedTeamException =
                new FailedTeamStorageException(databaseUpdateException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(TeamId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(someTeam);

            TeamDependencyException actualTeamDependencyException =
                 await Assert.ThrowsAsync<TeamDependencyException>(
                     modifyTeamTask.AsTask);

            // then
            actualTeamDependencyException.Should().BeEquivalentTo(
                expectedTeamDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(TeamId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDateTime);
            Team someTeam = randomTeam;
            someTeam.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid teamId = someTeam.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTeamException =
                new LockedTeamException(databaseUpdateConcurrencyException);

            var expectedTeamDependencyValidationException =
                new TeamDependencyValidationException(lockedTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(teamId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(someTeam);

            TeamDependencyValidationException actualTeamDependencyValidationException =
                await Assert.ThrowsAsync<TeamDependencyValidationException>(
                    modifyTeamTask.AsTask);

            // then
            actualTeamDependencyValidationException.Should().BeEquivalentTo(
                expectedTeamDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(TeamId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Team randomTeam = CreateRandomTeam(randomDateTime);
            Team someTeam = randomTeam;
            someTeam.CreatedDate = randomDateTime.AddMinutes(minuteInPast);
            var serviceException = new Exception();

            var failedTeamException =
                new FailedTeamServiceException(serviceException);

            var expectedTeamServiceException =
                new TeamServiceException(failedTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(someTeam.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Team> modifyTeamTask =
                this.teamService.ModifyTeamAsync(someTeam);

            TeamServiceException actualTeamServiceException =
                await Assert.ThrowsAsync<TeamServiceException>(
                    modifyTeamTask.AsTask);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(
                expectedTeamServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(someTeam.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
