//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Users;
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

        [Fact]
        public void ShouldAddPasswordAndEmailIfNotExistAsync()
        {
            //given
            string randomEmail = GetrandomString();
            string randomPassword = GetrandomString();
            var randomUser = new User { Email = randomEmail, Password = randomPassword };
            var inputUser = randomUser;
            var addUser = inputUser;
            var expectedUser = new List<User> { randomUser };

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers()).Returns(expectedUser.AsQueryable());

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(inputUser))
                    .ReturnsAsync(addUser);

            //when
            User actualUser = userProcessingsService.
                RetrieveUserByCredentails(randomEmail, randomPassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Once);

            this.userServiceMock.Verify(service=>
                service.AddUserAsync(inputUser),Times.Once) ;

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
