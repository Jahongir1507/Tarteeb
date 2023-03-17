//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingsServiceTests
    {
        [Fact]
        public void ShoulRetrieveUserByCredentails()
        {
            // given
            string inputEmail = GetrandomString();
            string inputPassword = GetrandomString();
            var expectedUser = new User { Email = inputEmail, Password = inputPassword };
            var users = new List<User> { expectedUser };

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(users.AsQueryable());

            // when
            User actualUser = userProcessingsService.
                RetrieveUserByCredentails(inputEmail, inputPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
