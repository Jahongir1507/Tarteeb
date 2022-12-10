using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams.Excaptions;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Teamss
{
    public partial class TeamServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedTeamStorageException =
                new FaildTeamStorageException(sqlException);

            var expectedTeamDependencyException =
                new TeamDependencyException(failedTeamStorageException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllTeams())
                   .Throws(sqlException);

            //when
            Action retrieveAllTeamsAction = () =>
                this.teamService.RetrieveAllTeams();

            //then
            Assert.Throws<TeamDependencyException>(retrieveAllTeamsAction);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeamDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
