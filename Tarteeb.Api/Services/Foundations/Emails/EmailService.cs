//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
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
            throw new System.NotImplementedException();
    }
}
