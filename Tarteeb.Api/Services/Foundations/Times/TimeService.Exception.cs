//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
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
            catch(NullTimeException nullTimeException)
            {
                throw CreateAndLogValidationException(nullTimeException);
            }
        }

        private TimeValidationException CreateAndLogValidationException(Xeption exception)
        {
            TimeValidationException timeValidationException = 
                new TimeValidationException(exception);

            this.loggingBroker.LogError(timeValidationException);

            return timeValidationException;
        }
    }
}
