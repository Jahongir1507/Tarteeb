//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Tarteeb.Api.Models;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingsServiceTests
    {
        [Fact]
        public void ShouldAddCountryIfNotExistAsync()
        {
            // given
            string inputEmail = GetrandomString();
            string inputpassword = GetrandomString();
            User expectedUser = new User { Email = inputEmail, Password = inputpassword };

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(new List<User> { expectedUser }.AsQueryable());

            // when
            var actualUser = userProcessingsService.RetrieveUserByCredentails(inputEmail, inputpassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Once);

            userServiceMock.VerifyNoOtherCalls();
        }
    }
}
