//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Processings.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingsServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnUpsertIfEmailAndPasswordIsNullAndLogItAsync()
        {
            //given
            string nullEmail = null;
            string nullPassword = null;
            var nullUserProcessingException = new NullUserProcessingException();

            var expectedUserProcessingValidationException =
                new UserProcessingValidationException(nullUserProcessingException);

            //when
            ValueTask<User>retrieveUserByTask =
               this.userProcessingsService.RetrieveUserByCredentails(nullEmail,password:nullPassword);

            UserProcessingValidationException actualUserProcessingValidationException =
                await Assert.ThrowsAsync<UserProcessingValidationException>(retrieveUserByTask.AsTask);

            //then
            actualUserProcessingValidationException.Should().BeEquivalentTo(
                expectedUserProcessingValidationException);

            this.loggingBrokerMock.Verify(broker=>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingValidationException))),Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>()), Times.Never);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
