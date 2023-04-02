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
    }
}
