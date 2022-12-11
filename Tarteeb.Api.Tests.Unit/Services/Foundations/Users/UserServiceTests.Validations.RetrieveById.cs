//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowVlidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invlidUserId = Guid.Empty;
            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required");

            var expectedUserValidationException = new UserValidationException(invalidUserException);

            // when
            ValueTask<User> retrieveUserByIdTask =
                this.userService.RetrieveUserAsync(invlidUserId);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(retrieveUserByIdTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}