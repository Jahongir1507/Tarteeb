//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using PostmarkDotNet;
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
            catch (FailedEmailServerException failedEmailServerException)
            {
                throw CreateAndLogCriticalDependencyException(failedEmailServerException);
            }
            catch (InvalidEmailException invalidEmailException)
            {
                throw CreateAndLogDependencyValidationException(invalidEmailException);
            }
        }

        private EmailValidationException CreateAndLogValidationException(Xeption exception)
        {
            var emailValidationException = new EmailValidationException(exception);
            this.loggingBroker.LogError(emailValidationException);

            return emailValidationException;
        }

        private EmailDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var emailDependencyException = new EmailDependencyException(exception);
            this.loggingBroker.LogCritical(emailDependencyException);

            return emailDependencyException;
        }

        private EmailDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var emailDependencyValidationException = new EmailDependencyValidationException(exception);
            this.loggingBroker.LogError(emailDependencyValidationException);

            return emailDependencyValidationException;
        }


        private Email ConvertToMeaningfulError(PostmarkResponse postmarkResponse)
        {
            var innerException = new Exception(postmarkResponse.Message);

            return postmarkResponse.Status switch
            {
                PostmarkStatus.ServerError => throw new FailedEmailServerException(innerException),
                PostmarkStatus.UserError => throw new InvalidEmailException(innerException),
                _ => throw new FailedEmailServerException(innerException)
            };
        }
    }
}
