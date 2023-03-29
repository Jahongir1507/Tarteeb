//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Scores;
using Xeptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private delegate ValueTask<Score> ReturningScoresFunction();

        private async ValueTask<Score> TryCatch(ReturningScoresFunction returningTeamFunction)
        {
            try
            {
                return await returningTeamFunction();
            }
            catch (InvalidScoreException invalidScoreException)
            {
                throw CreateAndLogValidationException(invalidScoreException);
            }
            catch(NotFoundScoreException notFoundScoreException)
            {
                throw CreateAndLogValidationException(notFoundScoreException);
            }
            catch(SqlException sqlException)
            {
                var failedScoreStorageException = new FailedScoreStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedScoreStorageException);
            }
            catch(DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedScoreException = new LockedScoreException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedScoreException);
            }
            catch (Exception serviceException)
            {
                var failedScoreException = new FailedScoreServiceException(serviceException);

                throw CreateAndLogServiceException(failedScoreException);
            }
        }

        private ScoreServiceException CreateAndLogServiceException(Exception exception)
        {
            var scoreServiceException = new ScoreServiceException(exception);
            this.loggingBroker.LogError(scoreServiceException);

            return scoreServiceException;
        }

        private Exception CreateAndLogCriticalDependencyException(FailedScoreStorageException failedScoreStorageException)
        {
            var scoreDependencyException = new ScoreDependencyException(failedScoreStorageException);
            this.loggingBroker.LogCritical(scoreDependencyException);

            return scoreDependencyException;
        }

        private Exception CreateAndLogDependencyValidationException(LockedScoreException lockedScoreException)
        {
            var scoreDependencyValidationException = new ScoreDependencyValidationException(lockedScoreException);
            this.loggingBroker.LogError(scoreDependencyValidationException);

            return scoreDependencyValidationException;
        }

        private ScoreValidationException CreateAndLogValidationException(Xeption exception)
        {
            var scoreValidationExpcetion = new ScoreValidationException(exception);
            this.loggingBroker.LogError(scoreValidationExpcetion);

            return scoreValidationExpcetion;
        }
    }
}
