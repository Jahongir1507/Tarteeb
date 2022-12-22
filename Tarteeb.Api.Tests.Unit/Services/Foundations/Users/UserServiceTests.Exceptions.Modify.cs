//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Tickets.Exceptions;
using Tarteeb.Api.Models.Tickets;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;
using FluentAssertions;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Throws(sqlException);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(randomUser);

            //then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
               modifyUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                       Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectUserByIdAsync(randomUser.Id),
                  Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(randomUser),
                   Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsnyc()
        {
            //given
            User foreignKeyConflictedUser = CreateRandomUser();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidUserReferenceException =
                new InvalidUserReferenceException(foreignKeyConstraintConflictException);

            var userDependencyValidationException =
                new UserDependencyValidationException(invalidUserReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(foreignKeyConflictedUser);

            //then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
               modifyUserTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    userDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectUserByIdAsync(foreignKeyConflictedUser.Id),
                   Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(foreignKeyConflictedUser),
                   Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomUser(randomDateTime);
            User someUser = randomUser;
            Guid userId = someUser.Id;
            someUser.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedUserException =
                new FailedUserStorageException(databaseUpdateException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
              await Assert.ThrowsAsync<UserDependencyException>(
                  modifyUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
