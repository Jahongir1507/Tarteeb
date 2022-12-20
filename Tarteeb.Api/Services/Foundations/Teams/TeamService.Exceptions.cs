//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService
    {
        private delegate ValueTask<Team> ReturningTeamFunction();
        private delegate IQueryable<Team> ReturningTeamsFunction();

        private async ValueTask<Team> TryCatch(ReturningTeamFunction returningTeamFunction)
        private IQueryable<Team> TryCatch(ReturningTeamsFunction returningTeamsFunction)
        {
            try
            {
                return await returningTeamFunction();
            }
            catch (NullTeamException nullTeamException)
            {
                throw CreateAndLogValidationException(nullTeamException);
                return returningTeamsFunction();
            }
            catch (InvalidTeamException invalidTeamException)
            {
                throw CreateAndLogValidationException(invalidTeamException);
            }
            catch (SqlException sqlException)
            {
                var failedTeamStorageException = new FailedTeamStorageException(sqlException);
                var failedTeamStorageException =
                    new FailedTeamStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTeamStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            catch (Exception exception)
            {
                var alreadyExistsTeamException = new AlreadyExistsTeamException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsTeamException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTeamException = new LockedTeamException(dbUpdateConcurrencyException);
                var failedTeamServiceException =
                    new FailedTeamServiceException(exception);

                throw CreateAndDependencyValidationException(lockedTeamException);
            }
            catch (Exception serviceException)
            {
                var failedTeamServiceException = new FailedTeamServiceException(serviceException);

                throw CreateAndLogServiceException(failedTeamServiceException);
            }
        }

        private TeamDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        private TeamValidationException CreateAndLogValidationException(Xeption exception)
        {
            var teamValidationExpcetion = new TeamValidationException(exception);
            this.loggingBroker.LogError(teamValidationExpcetion);

            return teamValidationExpcetion;
        }
        var teamDependencyException =
            new TeamDependencyException(exception);

        private TeamDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var teamDependencyException = new TeamDependencyException(exception);
            this.loggingBroker.LogCritical(teamDependencyException);

            return teamDependencyException;
        }

        private TeamDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        private TeamServiceException CreateAndLogServiceException(Xeption exception)
        {
            var teamDependencyValidationException = new TeamDependencyValidationException(exception);
            this.loggingBroker.LogError(teamDependencyValidationException);

            return teamDependencyValidationException;
        }
        var teamServiceException =
            new TeamServiceException(exception);

        private TeamServiceException CreateAndLogServiceException(Xeption exception)
        {
            var teamServiceException = new TeamServiceException(exception);
            this.loggingBroker.LogError(teamServiceException);

            return teamServiceException;
        }
    }
}