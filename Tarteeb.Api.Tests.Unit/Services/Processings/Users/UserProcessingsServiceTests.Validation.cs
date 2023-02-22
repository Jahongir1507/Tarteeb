using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Processings.Users;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingsServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnUpsertIfEmailAndPasswordAreInvalidAndLogItAsync()
        {
            //given
            string invalidEmail= string.Empty;
            string invalidPassword= string.Empty;
            var invalidUserProcessingException = new InvalidUserProcessingException();

            invalidUserProcessingException.AddData(
                key: nameof(User.Email),
                values: "Text is required");

            invalidUserProcessingException.AddData(
                key: nameof(User.Password),
                values: "Text is required");

            var expectedUserProcessingValidationException = 
                new UserProcessingValidationException(invalidUserProcessingException);

            //when
            ValueTask<User> retrieveUserByTask= 
                this.userProcessingsService.RetrieveUserByCredentails(invalidEmail, invalidPassword);

            UserProcessingValidationException actualUserProcessingValidationException = 
                await Assert.ThrowsAsync<UserProcessingValidationException>(retrieveUserByTask.AsTask);

            //then
            actualUserProcessingValidationException.Should().BeEquivalentTo(
               expectedUserProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserProcessingValidationException))), Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>()), Times.Never);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userServiceMock.VerifyNoOtherCalls();
        }
    }
}
