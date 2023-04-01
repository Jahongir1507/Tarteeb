//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.TimeSlots
{
    public partial class TimeService
    {
        private delegate ValueTask<Time> ReturningTimeFunction();

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
                var failedTimeStorageException = new FailedTimeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedTimeStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTimeException = new LockedTimeException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedTimeException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedTimeStorageException = new FailedTimeStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedTimeStorageException);
            }
            catch (Exception serviceException)
            {
                var failedServiceTimeException = new FailedTimeServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceTimeException);
            }
        }

        private TimeServiceException CreateAndLogServiceException(Xeption exception)
        {
            var timeServiceException = new TimeServiceException(exception);
            this.loggingBroker.LogError(timeServiceException);

            return timeServiceException;
        }

        private TimeDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var timeDependencyException = new TimeDependencyException(exception);
            this.loggingBroker.LogError(timeDependencyException);

            return timeDependencyException;
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

        private TimeDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var timeDependencyValidationException = new TimeDependencyValidationException(exception);
            this.loggingBroker.LogError(timeDependencyValidationException);

            return timeDependencyValidationException;
        }
    }
}
