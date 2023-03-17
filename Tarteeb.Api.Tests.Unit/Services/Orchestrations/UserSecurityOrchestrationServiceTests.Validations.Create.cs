//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        [Fact]
        public void ShouldThrowValidationExceptionOnCreateIfEmailOrPasswordAreInvalidAndLogItAsync()
        {
            //given
            string invalidEmail = string.Empty;
            string invalidPassword = string.Empty;
            var invalidUserCreadentialOrchestrationException = new InvalidUserCredentialOrchestrationException();

            invalidUserCreadentialOrchestrationException.AddData(
                key: nameof(User.Email),
                values: "Value is required");

            invalidUserCreadentialOrchestrationException.AddData(
                key: nameof(User.Password),
                values: "Value is required");

            var expectedUserOrchestrationValidationException =
                new UserTokenOrchestrationValidationException(invalidUserCreadentialOrchestrationException);

            //when
            Action createUserTokenAction = () =>
                this.userSecurityOrchestrationService.CreateUserToken(invalidEmail, invalidPassword);

            UserTokenOrchestrationValidationException actualUserTokenOrchestrationValidationException =
                 Assert.Throws<UserTokenOrchestrationValidationException>(createUserTokenAction);

            //then
            actualUserTokenOrchestrationValidationException.Should().BeEquivalentTo(
               expectedUserOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationValidationException))), Times.Once);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Never);

            this.securityServiceMock.Verify(service =>
                service.CreateToken(It.IsAny<User>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
            this.securityServiceMock.VerifyNoOtherCalls();
        }
    }
}
