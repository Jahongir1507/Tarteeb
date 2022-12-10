using Microsoft.Data.SqlClient;
using System.Linq;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Teamss
{
    public partial class TeamService
    {
        private delegate IQueryable<Team> ReturningTeamsFunction();

        private IQueryable<Team> TryCatch(ReturningTeamsFunction returningTeamsFunction)
        {
            try
            {
                return returningTeamsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTeamStorageException =
                    new FailedTeamStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTeamStorageException);
            }
        }

        private TeamDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var teamDependencyException = new TeamDependencyException(exception);
            this.loggingBroker.LogCritical(teamDependencyException);

            return teamDependencyException;
        }
    }
}
