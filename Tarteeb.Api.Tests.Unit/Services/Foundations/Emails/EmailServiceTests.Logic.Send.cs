//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using PostmarkDotNet;
using Tarteeb.Api.Models.Foundations.Emails;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Emails
{
    public partial class EmailServiceTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            Email randomEmail = CreateRandomEmail();
            Email inputEmail = randomEmail;
            Email expectedEmail = inputEmail.DeepClone();

            PostmarkResponse randomPostmarkResponse =
                CreatePostmarkResponse(PostmarkStatus.Success);

            PostmarkResponse receivedPostmarkResponse =
                randomPostmarkResponse;

            this.emailBrokerMock.Setup(broker =>
                broker.SendEmail(inputEmail))
                    .ReturnsAsync(receivedPostmarkResponse);

            // when
            Email actualEmail = await this.emailService
                .SendEmailAsync(inputEmail);

            // then
            actualEmail.Should().BeEquivalentTo(expectedEmail);

            this.emailBrokerMock.Verify(broker =>
                broker.SendEmail(inputEmail), Times.Once);

            this.emailBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
