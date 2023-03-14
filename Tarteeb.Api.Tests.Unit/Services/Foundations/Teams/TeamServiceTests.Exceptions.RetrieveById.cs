//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedTeamStorageException =
                new FailedTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Team> retrieveTeamByIdTask =
                this.teamService.RetrieveTeamByIdAsync(someId);

            TeamDependencyException actaulTeamDependencyException =
                await Assert.ThrowsAsync<TeamDependencyException>(
                    retrieveTeamByIdTask.AsTask);

            // then
            actaulTeamDependencyException.Should().BeEquivalentTo(
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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTeamServiceException =
                new FailedTeamServiceException(serviceException);

            var expectedTeamServiceExcpetion =
                new TeamServiceException(failedTeamServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            // when
            ValueTask<Team> retrieveTeamById =
            this.teamService.RetrieveTeamByIdAsync(someId);

            TeamServiceException actualTeamServiceException =
                await Assert.ThrowsAsync<TeamServiceException>(retrieveTeamById.AsTask);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(expectedTeamServiceExcpetion);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeamByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedTeamServiceExcpetion))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}