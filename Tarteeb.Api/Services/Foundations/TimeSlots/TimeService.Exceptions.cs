//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
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
            catch (Exception serviceException)
            {
                var failedServiceProfileException = new FailedTicketServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceProfileException);
            }
        }

        private TimeServiceException CreateAndLogServiceException(Xeption exception)
        {
            var timeServiceException = new TimeServiceException(exception);
            this.loggingBroker.LogError(timeServiceException);

            return timeServiceException;
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
    }
}
