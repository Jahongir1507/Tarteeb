//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Emails;
using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions;
using Xeptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(UserEmailDependencyExceptions))]
        public async Task ShoudThrowDependencyExceptionOnCreateAccountIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            User someUser = CreateRandomUser();

            var expectedUserOrchestrationDependencyException =
                new UserOrchestrationDependencyException(dependencyException);

            this.userServiceMock.Setup(service => service.AddUserAsync(
                It.IsAny<User>())).ThrowsAsync(dependencyException);

            // when
            ValueTask<User> createUserAccountTask = this.userSecurityOrchestrationService
                .CreateUserAccountAsync(someUser);

            UserOrchestrationDependencyException actualUserOrchestrationDependencyException =
                await Assert.ThrowsAsync<UserOrchestrationDependencyException>(createUserAccountTask.AsTask);

            // then
            actualUserOrchestrationDependencyException.Should().BeEquivalentTo(
                expectedUserOrchestrationDependencyException);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(
                broker => broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationDependencyException))), Times.Once);

            this.emailServiceMock.Verify(service =>
                service.SendEmailAsync(It.IsAny<Email>()), Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emailServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(UserEmailDependencyValidationExceptions))]
        public async Task ShoudThrowDependencyValidationExceptionOnCreateAccountIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            User someUser = CreateRandomUser();

            var expectedUserOrchestrationDependencyValidationException =
                new UserOrchestrationDependencyValidationException(dependencyException);

            this.userServiceMock.Setup(service => service.AddUserAsync(
                It.IsAny<User>())).ThrowsAsync(dependencyException);

            // when
            ValueTask<User> createUserAccountTask = this.userSecurityOrchestrationService
                .CreateUserAccountAsync(someUser);

            UserOrchestrationDependencyValidationException actualUserOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<UserOrchestrationDependencyValidationException>(createUserAccountTask.AsTask);

            // then
            actualUserOrchestrationDependencyValidationException.Should().BeEquivalentTo(
                expectedUserOrchestrationDependencyValidationException);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(
                broker => broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationDependencyValidationException))), Times.Once);

            this.emailServiceMock.Verify(service =>
                service.SendEmailAsync(It.IsAny<Email>()), Times.Never);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.emailServiceMock.VerifyNoOtherCalls();
        }
    }
}
