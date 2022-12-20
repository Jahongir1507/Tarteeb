//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Microsoft.Data.SqlClient;
using Moq;
using Tarteeb.Api.Models.Users.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                   .Throws(sqlException);

            //when
            Action retrieveAllUsersAction = () =>
                this.userService.RetrieveAllUsers();

            //then
            Assert.Throws<UserDependencyException>(retrieveAllUsersAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedUserServiceException =
                new FailedUserServiceException(serviceException);

            var expecteduserServiceException =
                new UserServiceException(failedUserServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers()).Throws(serviceException);

            //when
            Action retrieveAllUserAction = () =>
                this.userService.RetrieveAllUsers();

            //then
            Assert.Throws<UserServiceException>(retrieveAllUserAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expecteduserServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}