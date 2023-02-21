using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings
{
    public partial class SecurityProcessingServiceTests
    {
        [Fact]
        public void ShouldCreateTokenUser()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            string randomString = CreateRandomString();
            string createToken = randomString;
            string exceptedToken = createToken;

            this.securityServiceMock.Setup(service =>
            service.CreateToken(inputUser)).Returns(exceptedToken);

            //when
            string actualToken = this.securityProcessingService.CreateToken(inputUser);

            //then
            actualToken.Should().BeEquivalentTo(exceptedToken);

            this.securityServiceMock.Verify(service =>
                service.CreateToken(inputUser), Times.Once());

            this.securityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}