﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();
            SqlException sqlException = CreateSqlException();
            var failedUserStorageException = new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
              broker.GetCurrentDateTime()).
                Throws(sqlException);

            //when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);

            //then
            actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTime(), Times.Once);
           
            this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedUserDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurrsandLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();
            string someMessage = GetRandomString();
            var duplicatekeyException = new DuplicateKeyException(someMessage);

            var failedUserDependencyValidationException =
                new FailedUserDependencyValidationException(duplicatekeyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(failedUserDependencyValidationException);

            this.storageBrokerMock.Setup(broker => broker.InsertUserAsync(It.IsAny<User>()))
                .ThrowsAsync(duplicatekeyException);

            //when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(someUser);

            UserDependencyValidationException actualUserDependencyValidationException =
            await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);

            //then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.storageBrokerMock.Verify(broker => broker.InsertUserAsync(
                It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                  expectedUserDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedUserException = new LockedUserException(dbUpdateConcurrencyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(lockedUserException);

            this.storageBrokerMock.Setup(broker => broker.InsertUserAsync(It.IsAny<User>()))
                .ThrowsAsync(dbUpdateConcurrencyException);

            //when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(someUser);

            UserDependencyValidationException actualUserDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);

            //then
            actualUserDependencyValidationException.Should().BeEquivalentTo(expectedUserDependencyValidationException);

            this.storageBrokerMock.Verify(broker => broker.InsertUserAsync(It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedUserDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddifServiceErrorOccursAndLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();
            var serviceException = new Exception();

            var failedUserServiceException =
                new FailedUserServiceException(serviceException);

            var expectedUserServiceException =
                new UserServiceException(failedUserServiceException);

            this.storageBrokerMock.Setup(broker => broker.InsertUserAsync(It.IsAny<User>()))
                .ThrowsAsync(serviceException);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser);

            UserServiceException actualUserServiceException=
                await Assert.ThrowsAsync<UserServiceException>(addUserTask.AsTask);

            //then
            actualUserServiceException.Should().BeEquivalentTo(
                expectedUserServiceException);
           
            this.storageBrokerMock.Verify(broker =>
               broker.InsertUserAsync(It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(broker=>
               broker.LogError(It.Is(SameExceptionAs(
                  expectedUserServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}