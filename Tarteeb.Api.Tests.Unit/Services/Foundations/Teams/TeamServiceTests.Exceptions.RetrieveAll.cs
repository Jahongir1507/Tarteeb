//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedTeamStorageException =
                new FailedTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeams()).Throws(sqlException);

            // when
            Action retrieveAllTeamsAction = () =>
                this.teamService.RetrieveAllTeams();

            TeamDependencyException actualTeamDependencyException =
                Assert.Throws<TeamDependencyException>(retrieveAllTeamsAction);

            // then
            actualTeamDependencyException.Should().BeEquivalentTo(expectedTeamDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeams(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedTeamServiceException =
                new FailedTeamServiceException(serviceException);

            var expectedTeamServiceException =
                new TeamServiceException(failedTeamServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeams()).Throws(serviceException);

            // when
            Action retrieveAllTeamAction = () =>
                this.teamService.RetrieveAllTeams();

            TeamServiceException actualTeamServiceException =
                Assert.Throws<TeamServiceException>(retrieveAllTeamAction);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(expectedTeamServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeams(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeamServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}