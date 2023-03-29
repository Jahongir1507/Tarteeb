//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Times
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
            catch (InvalidTimeException invalidTimeException)
            {
                throw CreateAndLogValidationException(invalidTimeException);
            }
            catch (NotFoundTimeException  notFoundTimeException) 
            {
                throw CreateAndLogValidationException(notFoundTimeException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTimeException = new LockedTimeException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedTimeException);
            }
        }

        private TimeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var timeValidationException = new TimeValidationException(exception);
            this.loggingBroker.LogError(timeValidationException);

            return timeValidationException;
        }

        private TimeDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var timeDependencyValidationException = new TimeDependencyValidationException(exception);
            this.loggingBroker.LogError(timeDependencyValidationException);

            return timeDependencyValidationException;
        }
    }
}
