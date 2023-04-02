//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using PostmarkDotNet;
using Tarteeb.Api.Brokers.Emails;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Foundations.Emails;

namespace Tarteeb.Api.Services.Foundations.Emails
{
    public partial class EmailService : IEmailService
    {
        private readonly IEmailBroker emailBroker;
        private readonly ILoggingBroker loggingBroker;

        public EmailService(
            IEmailBroker emailBroker,
            ILoggingBroker loggingBroker)
        {
            this.emailBroker = emailBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Email> SendEmailAsync(Email email) =>
        TryCatch(async () =>
        {
            ValidateEmailNotNull(email);
            PostmarkResponse postmarkResponse = await this.emailBroker.SendEmail(email);

            return postmarkResponse.Status switch
            {
                PostmarkStatus.ServerError => ConvertToMeaningfulServerError(postmarkResponse),
                PostmarkStatus.UserError => throw new System.NotImplementedException(),
                PostmarkStatus.Success => email,
                _ => throw new System.NotImplementedException(),
            };
        });
    }
}
