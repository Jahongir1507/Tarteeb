//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService
    {
        private delegate ValueTask<Team> ReturningTeamFunction();

        private async ValueTask<Team> TryCatch(ReturningTeamFunction returningTeamFunction      )
        {
            try
            {
                return await returningTeamFunction();
            }
            catch (NullTeamException nullTeamException)
            {
                throw CreateAndLogValidationException(nullTeamException);
            }
            catch(InvalidTeamException invalidTeamException)
            {
                throw CreateAndLogValidationException(invalidTeamException);
            }catch(SqlException sqlException)
            {
                var failedTeamStorageException = new FailedTeamStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTeamStorageException);
            }
        }

        private Exception CreateAndLogValidationException(SqlException sqlException)
        {
            throw new NotImplementedException();
        }

        private TeamValidationException CreateAndLogValidationException(Xeption exception)
        {
            var teamValidationExpcetion = new TeamValidationException(exception);
            this.loggingBroker.LogError(teamValidationExpcetion);

            return teamValidationExpcetion;
        }

        private TeamDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var teamDependencyException = new TeamDependencyException(exception);
            this.loggingBroker.LogCritical(teamDependencyException);

            return teamDependencyException;
        }
    }
}
