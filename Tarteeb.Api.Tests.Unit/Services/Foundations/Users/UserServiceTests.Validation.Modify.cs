//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsNullAndLogItAsync()
        {
            //given
            User nullUser = null;
            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(nullUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()), Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsInvalidAndLogItAsync(string invalidText)
        {
            //given
            var invalidUser = new User
            {
                FirstName = invalidText
            };

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required");

            invalidUserException.AddData(
                key: nameof(User.FirstName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.LastName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.Email),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.BirthDate),
                values: "Value is required");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: "Value is required");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Value is required");

            var expectedUserValidationException = new UserValidationException(
                invalidUserException);

            //when
            ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(invalidUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
               modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdateDateIsNotSameAsCreateDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDatetime = GetRandomDateTime();
            User randomUser = CreateRandomUser(randomDatetime);
            User invalidUser = randomUser;

            var invalidUserException =
                new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: $"Date is same as {nameof(User.CreatedDate)}");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                  .Returns(randomDatetime);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
              modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedUserValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Never);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectUserByIdAsync(invalidUser.Id), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
