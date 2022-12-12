//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.Data.SqlClient;
using Moq;
using System;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teams
{
    public partial class TeamServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedTeamStorageException =
                new FailedTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllTeams()).Throws(sqlException);

            //when
            Action retrieveAllTeamsAction = () =>
                this.teamService.RetrieveAllTeams();

            //then
            Assert.Throws<TeamDependencyException>(retrieveAllTeamsAction);

            this.storageBrokerMock.Verify(broker => broker.SelectAllTeams());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomString();

            var serviceException = new Exception(exceptionMessage);

            var failedTeamServiceException =
               new FailedTeamServiceException(serviceException);

            var expectedteamServiceException =
                new TeamServiceException(failedTeamServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeams()).Throws(serviceException);

            //when
            Action retrieveAllTeamAction = () =>
                this.teamService.RetrieveAllTeams();

            //then
            Assert.Throws<TeamServiceException>(retrieveAllTeamAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeams(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedteamServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}