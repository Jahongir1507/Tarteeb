//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Users.Exceptions;
using Tarteeb.Api.Models;
using Xunit;
using FluentAssertions;

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

            this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(It.IsAny<User>())).ThrowsAsync(sqlException);

            //when
            ValueTask<User> addUserTask = this.userService.AddUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);

            //then
            actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);

            this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedUserDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
