using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string inputpassword = GetrandomString();

            User expectedUser = new User { Email = inputEmail, Password = inputpassword };

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers())
                    .Returns(new List<User> { expectedUser }.AsQueryable());

            // when
            var actualUser = userProcessingsService.RetrieveUserByCredentails(inputEmail, inputpassword);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
