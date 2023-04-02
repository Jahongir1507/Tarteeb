//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq.Expressions;
using Moq;
using PostmarkDotNet;
using Tarteeb.Api.Brokers.Emails;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Services.Foundations.Emails;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        private readonly Mock<IEmailBroker> emailBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IEmailService emailService;

        public EmailServiceTests()
        {
            this.emailBrokerMock = new Mock<IEmailBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.emailService = new EmailService(
                emailBroker: this.emailBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTime GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
         actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString()
        {
            return new MnemonicString(
                wordMinLength: 4,
                wordMaxLength: 9,
                wordCount: 1)
                    .GetValue();
        }

        private static PostmarkResponse CreatePostmarkResponse(PostmarkStatus postmarkStatus)
        {
            return new PostmarkResponse
            {
                ErrorCode = 200,
                Message = string.Empty,
                MessageID = Guid.NewGuid(),
                Status = postmarkStatus,
                SubmittedAt = GetRandomDateTime(),
                To = GetRandomString()
            };
        }

        private static Email CreateRandomEmail() =>
            CreateEmailFiller().Create();

        private static Filler<Email> CreateEmailFiller() =>
            new Filler<Email>();
    }
}
