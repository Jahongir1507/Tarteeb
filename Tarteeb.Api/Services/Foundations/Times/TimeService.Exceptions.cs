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
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService
    {
        private delegate ValueTask<Time> ReturningTimeFunction();
        private delegate IQueryable<Time> ReturningTimesFunction();

        private async ValueTask<Time> TryCatch(ReturningTimeFunction returningTimeFunction)
        {
            try
            {
                return await returningTimeFunction();
            }
            catch (NullTimeException nullTimeException)
            {
                throw CreateAndLogValidationException(nullTimeException);
            }
            catch (InvalidTimeException invalidTimeException)
            {
                throw CreateAndLogValidationException(invalidTimeException);
            }
            catch (NotFoundTimeException notFoundTimeException)
            {
                throw CreateAndLogValidationException(notFoundTimeException);
            }
            catch (SqlException sqlException)
            {
                var failedTimeStorageException =
                    new FailedTimeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTimeStorageException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTimeStorageException = new FailedTimeStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedTimeStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var failedTimeDependencyValidationException =
                     new AlreadyExistsTimeException(duplicateKeyException);

                throw CreateAndDependencyValidationException(failedTimeDependencyValidationException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTimeReferenceException = new InvalidTimeReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidTimeReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTimeException = new LockedTimeException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedTimeException);
            }
            catch (Exception serviceException)
            {
                var failedTimeServiceException = new FailedTimeServiceException(serviceException);

                throw CreateAndLogServiceException(failedTimeServiceException);
            }
        }

        private TimeDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var timeDependencyException = new TimeDependencyException(exception);
            this.loggingBroker.LogError(timeDependencyException);

            return timeDependencyException;
        }

        private TimeDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var timeDependencyValidationException = new TimeDependencyValidationException(exception);
            this.loggingBroker.LogError(timeDependencyValidationException);

            return timeDependencyValidationException;
        }

        private IQueryable<Time> TryCatch(ReturningTimesFunction returningTimesFunction)
        {
            try
            {
                return returningTimesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTimeStorageException = new FailedTimeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTimeStorageException);
            }
            catch (Exception serviceException)
            {
                var failedTimeServiceException = new FailedTimeServiceException(serviceException);

                throw CreateAndLogServiceException(failedTimeServiceException);
            }
        }

        private TimeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var timeValidationException = new TimeValidationException(exception);
            this.loggingBroker.LogError(timeValidationException);

            return timeValidationException;
        }

        private TimeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var timeDependencyException = new TimeDependencyException(exception);
            this.loggingBroker.LogCritical(timeDependencyException);

            return timeDependencyException;
        }

        private TimeDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var timeDependencyValidationException =
                new TimeDependencyValidationException(exception);

            this.loggingBroker.LogError(timeDependencyValidationException);

            return timeDependencyValidationException;
        }

        private TimeServiceException CreateAndLogServiceException(Xeption exception)
        {
            var timeServiceException = new TimeServiceException(exception);
            this.loggingBroker.LogError(timeServiceException);

            return timeServiceException;
        }
    }
}
