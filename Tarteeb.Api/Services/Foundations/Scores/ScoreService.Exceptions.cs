//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private delegate ValueTask<Score> ReturningScoreFuncion();
        private delegate IQueryable<Score> ReturningScoresFunction();

        private async ValueTask<Score> TryCatch(ReturningScoreFuncion
            returningScoreFunction)
        {
            try
            {
                return await returningScoreFunction();
            }
            catch (NullScoreException nullScoreException)
            {
                throw CreateAndLogValidationException(nullScoreException);
            }
            catch (InvalidScoreException invalidScoreException)
            {
                throw CreateAndLogValidationException(invalidScoreException);
            }
            catch (NotFoundScoreException notFoundScoreException)
            {
                throw CreateAndLogValidationException(notFoundScoreException);
            }
            catch (SqlException sqlException)
            {
                var failedScoreStorageException =
                    new FailedScoreStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedScoreStorageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidScoreReferenceException =
                    new InvalidScoreReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidScoreReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedScoreException = new LockedScoreException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedScoreException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedScoreStorageException = new FailedScoreStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedScoreStorageException);
            }
            catch (Exception serviceException)
            {
                var failedScoreServiceException = new FailedScoreServiceException(serviceException);

                throw CreateAndLogServiceException(failedScoreServiceException);
            }
        }

        private IQueryable<Score> TryCatch(ReturningScoresFunction returningScoresFunction)
        {
            try
            {
                return returningScoresFunction();
            }
            catch (SqlException sqlException)
            {
                var failedScoreStorageException = new FailedScoreStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedScoreStorageException);
            }
            catch (Exception serviceException)
            {
                var failedScoreServiceException = new FailedScoreServiceException(serviceException);

                throw CreateAndLogServiceException(failedScoreServiceException);
            }
        }

        private ScoreValidationException CreateAndLogValidationException(Xeption exception)
        {
            var scoreValidationException =
                new ScoreValidationException(exception);

            this.loggingBroker.LogError(scoreValidationException);

            return scoreValidationException;
        }

        private ScoreDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var scoreDependencyException = new ScoreDependencyException(exeption);
            this.loggingBroker.LogCritical(scoreDependencyException);

            return scoreDependencyException;
        }

        private ScoreDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var scoreDependencyValidationException = new ScoreDependencyValidationException(exception);
            this.loggingBroker.LogError(scoreDependencyValidationException);

            return scoreDependencyValidationException;
        }

        private ScoreDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var scoreDependencyException = new ScoreDependencyException(exception);
            this.loggingBroker.LogError(scoreDependencyException);

            return scoreDependencyException;
        }

        private ScoreServiceException CreateAndLogServiceException(Xeption exception)
        {
            var scoreServiceException = new ScoreServiceException(exception);
            this.loggingBroker.LogError(scoreServiceException);

            return scoreServiceException;
        }
    }
}