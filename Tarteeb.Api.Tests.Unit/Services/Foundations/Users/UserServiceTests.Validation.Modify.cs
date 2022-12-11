//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Users.Exceptions;
using Tarteeb.Api.Models;
using Xunit;
using Moq;

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
            ValueTask<User>modifyUserTask=
                this.userService.ModifyUserAsync(nullUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
            modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker=>
            broker.LogError(It.Is(SameExceptionAs(
                expectedUserValidationException))),
                Times.Once());

            this.storageBrokerMock.Verify(broker=>
            broker.UpdateUserAsync(It.IsAny<User>()),
            Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
