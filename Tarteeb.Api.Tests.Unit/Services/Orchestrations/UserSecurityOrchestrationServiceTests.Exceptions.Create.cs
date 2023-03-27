﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(UserDependencyExceptions))]
        public void ShoudThrowDependencyExceptionOnCreateIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someString = GetRandomString();

            var expectedUserTokenOrchestrationDependencyException =
                new UserTokenOrchestrationDependencyException(dependencyException);

            this.userServiceMock.Setup(service => service.RetrieveAllUsers())
                .Throws(dependencyException);

            // when
            Action createTokenAction = () =>
                this.userSecurityOrchestrationService.CreateUserToken(email: someString, password: someString);

            UserTokenOrchestrationDependencyException actualUserTokenOrchestrationDependencyException =
                Assert.Throws<UserTokenOrchestrationDependencyException>(createTokenAction);

            // then
            actualUserTokenOrchestrationDependencyException.Should().BeEquivalentTo(
                expectedUserTokenOrchestrationDependencyException);

            this.userServiceMock.Verify(service => service.RetrieveAllUsers(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserTokenOrchestrationDependencyException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.securityServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShoudThrowServiceExceptionOnCreateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someString = GetRandomString();
            var serviceException = new Exception();

            var failedUserTokenOrchestrationException = new 
                FailedUserTokenOrchestrationException(serviceException);

            var expectedUserTokenOrchestrationServiceException =
                new UserTokenOrchestrationServiceException(failedUserTokenOrchestrationException);

            this.userServiceMock.Setup(service => service.RetrieveAllUsers())
                .Throws(serviceException);

            // when
            Action createTokenAction = () =>
                this.userSecurityOrchestrationService.CreateUserToken(email: someString, password: someString);

            UserTokenOrchestrationServiceException actualUserTokenOrchestrationServiceException =
                Assert.Throws<UserTokenOrchestrationServiceException>(createTokenAction);

            // then
            actualUserTokenOrchestrationServiceException.Should().BeEquivalentTo(
                expectedUserTokenOrchestrationServiceException);

            this.userServiceMock.Verify(service => service.RetrieveAllUsers(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserTokenOrchestrationServiceException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.securityServiceMock.VerifyNoOtherCalls();
        }
    }
}
