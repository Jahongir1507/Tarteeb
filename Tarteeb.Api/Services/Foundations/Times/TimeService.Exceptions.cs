//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Times
{
    public partial class TimeService
    {
        private delegate IQueryable<Time> ReturningTimesFunction();

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

        private TimeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var timeDependencyException = new TimeDependencyException(exception);
            this.loggingBroker.LogCritical(timeDependencyException);

            return timeDependencyException;
        }

        private TimeServiceException CreateAndLogServiceException(Exception exception)
        {
            var timeServiceException = new TimeServiceException(exception);
            this.loggingBroker.LogError(timeServiceException);

            return timeServiceException;
        }
    }
}
