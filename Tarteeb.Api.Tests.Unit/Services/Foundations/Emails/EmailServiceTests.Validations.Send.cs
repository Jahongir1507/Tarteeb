//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Models.Foundations.Emails.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendIfEmailIsNullAndLogItAsync()
        {
            // given
            Email noEmail = null;
            var nullEmailException = new NullEmailException();

            var expectedEmailValidationException =
                new EmailValidationException(nullEmailException);

            // when
            ValueTask<Email> sendEmailTask = this.emailService.SendEmailAsync(noEmail);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(sendEmailTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(expectedEmailValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedEmailValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnSendIfEmailIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            Email invalidEmail = new Email
            {
                SenderAddress = invalidText
            };

            var invalidEmailException = new InvalidEmailException();

            invalidEmailException.AddData(
                key: nameof(Email.SenderAddress),
                values: "Value is required.");

            invalidEmailException.AddData(
                key: nameof(Email.ReferenceEquals),
                values: "Value is required.");

            invalidEmailException.AddData(
                key: nameof(Email.Subject),
                values: "Value is required.");

            invalidEmailException.AddData(
                key: nameof(Email.HtmlBody),
                values: "Value is required.");

            var expectedEmailValidationException =
                new EmailValidationException(invalidEmailException);

            // when
            ValueTask<Email> sendEmailTask = this.emailService.SendEmailAsync(invalidEmail);

            EmailValidationException actualEmailValidationException =
                await Assert.ThrowsAsync<EmailValidationException>(sendEmailTask.AsTask);

            // then
            actualEmailValidationException.Should().BeEquivalentTo(expectedEmailValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedEmailValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emailBrokerMock.VerifyNoOtherCalls();
        }
    }
}
