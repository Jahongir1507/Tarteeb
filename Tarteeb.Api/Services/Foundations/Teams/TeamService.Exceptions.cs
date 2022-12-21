//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService
    {
        private delegate ValueTask<Team> ReturningTeamFunction();

        private async ValueTask<Team> TryCatch(ReturningTeamFunction returningTeamFunction)
        {
            try
            {
                return await returningTeamFunction();
            }
            catch (NullTeamException nullTeamException)
            {
                throw CreateAndLogValidationException(nullTeamException);
            }
            catch (InvalidTeamException invalidTeamException)
            {
                throw CreateAndLogValidationException(invalidTeamException);
            }
            catch (SqlException sqlException)
            {
                var failedTeamStorageException = new FailedTeamStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTeamStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTeamException = new AlreadyExistsTeamException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsTeamException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeamException = new LockedTeamException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedTeamException);
            }
            catch (NotFoundTeamException notFoundTeamException)
            {
                throw CreateAndLogValidationException(notFoundTeamException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTeamStorageException = new FailedTeamStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedTeamStorageException);
            }
            catch (Exception serviceException)
            {
                var failedTeamServiceException = new FailedTeamServiceException(serviceException);

                throw CreateAndLogServiceException(failedTeamServiceException);
            }
        }

        private TeamDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var teamDependencyException = new TeamDependencyException(exception);
            this.loggingBroker.LogError(teamDependencyException);

            return teamDependencyException;
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

        private TeamDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var teamDependencyValidationException = new TeamDependencyValidationException(exception);
            this.loggingBroker.LogError(teamDependencyValidationException);

            return teamDependencyValidationException;
        }

        private TeamServiceException CreateAndLogServiceException(Xeption exception)
        {
            var teamServiceException = new TeamServiceException(exception);
            this.loggingBroker.LogError(teamServiceException);

            return teamServiceException;
        }
    }
}