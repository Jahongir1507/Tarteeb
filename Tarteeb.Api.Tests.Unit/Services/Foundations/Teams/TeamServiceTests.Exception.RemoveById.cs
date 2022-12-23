//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someTeamId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTeamException =
                new LockedTeamException(databaseUpdateConcurrencyException);

            var expectedTeamDependencyValidationException =
                new TeamDependencyValidationException(lockedTeamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Team> removeTeamByIdTask =
                this.teamService.RemoveTeamByIdAsync(someTeamId);

            TeamDependencyValidationException actualTeamDependencyValidationException =
                await Assert.ThrowsAsync<TeamDependencyValidationException>(
                    removeTeamByIdTask.AsTask);

            // then
            actualTeamDependencyValidationException.Should().BeEquivalentTo(
                expectedTeamDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
            expectedTeamDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeamAsync(It.IsAny<Team>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTeamId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedTeamStorageException =
                new FailedTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Team> deleteTeamTask =
                this.teamService.RemoveTeamByIdAsync(someTeamId);

            TeamDependencyException actualTeamDependencyException =
                await Assert.ThrowsAsync<TeamDependencyException>(
                    deleteTeamTask.AsTask);

            // then
            actualTeamDependencyException.Should().BeEquivalentTo(
                expectedTeamDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someTeamId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTeamServiceException =
                new FailedTeamServiceException(serviceException);

            var expectedTeamServiceException =
                new TeamServiceException(failedTeamServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Team> removeTeamByIdTask =
                this.teamService.RemoveTeamByIdAsync(someTeamId);

            TeamServiceException actualTeamServiceException =
                await Assert.ThrowsAsync<TeamServiceException>(
                    removeTeamByIdTask.AsTask);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(
                expectedTeamServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
