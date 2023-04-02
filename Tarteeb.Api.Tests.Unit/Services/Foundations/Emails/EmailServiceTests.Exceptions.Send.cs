//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PostmarkDotNet;
using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Models.Foundations.Emails.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalExceptionOnSendIfServerErrorOccurrsAndLogItAsync()
        {
            // given
            Email someEmail = CreateRandomEmail();

            PostmarkResponse serverErrorResponse =
                CreatePostmarkResponse(PostmarkStatus.ServerError);

            var exception = new Exception(serverErrorResponse.Message);

            var failedEmailServerException =
                new FailedEmailServerException(exception);

            var expectedEmailDependencyException =
                new EmailDependencyException(failedEmailServerException);

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmail(someEmail))
                    .ReturnsAsync(serverErrorResponse);

            // when
            ValueTask<Email> sendEmailTask = this.emailService.SendEmailAsync(someEmail);

            EmailDependencyException actualEmailDependencyException =
                await Assert.ThrowsAsync<EmailDependencyException>(sendEmailTask.AsTask);

            // then
            actualEmailDependencyException.Should().BeEquivalentTo(
                expectedEmailDependencyException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmail(It.IsAny<Email>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedEmailDependencyException))),
                        Times.Once);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnSendIfUserErrorOccurrsAndLogItAsync()
        {
            // given
            Email someEmail = CreateRandomEmail();

            PostmarkResponse userErrorResponse =
                CreatePostmarkResponse(PostmarkStatus.UserError);

            var exception = new Exception(userErrorResponse.Message);

            var invalidEmailException =
                new InvalidEmailException(exception);

            var expectedEmailDependencyValidationException =
                new EmailDependencyValidationException(invalidEmailException);

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmail(someEmail))
                    .ReturnsAsync(userErrorResponse);

            // when
            ValueTask<Email> sendEmailTask = this.emailService.SendEmailAsync(someEmail);

            EmailDependencyValidationException actualEmailDependencyValidationException =
                await Assert.ThrowsAsync<EmailDependencyValidationException>(sendEmailTask.AsTask);

            // then
            actualEmailDependencyValidationException.Should().BeEquivalentTo(
                expectedEmailDependencyValidationException);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmail(It.IsAny<Email>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedEmailDependencyValidationException))),
                        Times.Once);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
