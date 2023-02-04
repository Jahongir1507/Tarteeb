using FluentAssertions;
using Moq;
using Tarteeb.Api.Models;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations
{
    public partial class SecurityServiceTests
    {
        [Fact]
        public void ShouldCreateToken()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            string randomString = "asadsdsfdfdfdfdfd";
            string createdToken = randomString;
            string expectedToken = createdToken;

            this.tokenBrokerMock.Setup(broker =>
                broker.GenerateJWT(inputUser)).Returns(expectedToken);

            //when
            string actualToken = securityService.CreateToken(inputUser);

            //then
            actualToken.Should().BeEquivalentTo(expectedToken);

            this.tokenBrokerMock.Verify(broker =>
                broker.GenerateJWT(inputUser), Times.Once);

            this.tokenBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
