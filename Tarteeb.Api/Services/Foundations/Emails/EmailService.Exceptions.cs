//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Models.Foundations.Emails.Exceptions;
using Xeptions;

namespace Tarteeb.Api.Services.Foundations.Emails
{
    public partial class EmailService
    {
        private delegate ValueTask<Email> ReturningEmailFunction();

        private async ValueTask<Email> TryCatch(ReturningEmailFunction returningEmailFunction)
        {
            try
            {
                return await returningEmailFunction();
            }
            catch (NullEmailException nullEmailException)
            {
                throw CreateAndLogValidationException(nullEmailException);
            }
        }

        private EmailValidationException CreateAndLogValidationException(Xeption exception)
        {
            var emailValidationException = new EmailValidationException(exception);
            this.loggingBroker.LogError(emailValidationException);

            return emailValidationException;
        }
    }
}
