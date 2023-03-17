//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Orchestrations.UserTokens;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        [Fact]
        public void ShoudCreateUserToken()
        {
            // given
            User randomUser = CreateRandomUser();
            User existingUser = randomUser;
            string anotherRandomString = GetRandomString();
            IQueryable<User> randomUsers = CreateRandomUsersIncluding(existingUser);
            IQueryable<User> retrievedUsers = randomUsers;

            string token = anotherRandomString;

            UserToken expectedUserToken = new UserToken
            {
                UserId = existingUser.Id,
                Token = token
            };

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers()).Returns(retrievedUsers);

            this.securityServiceMock.Setup(service =>
                service.CreateToken(existingUser)).Returns(token);

            // when
            UserToken actualUserToken = this.userSecurityOrchestrationService
                .CreateUserToken(existingUser.Email, existingUser.Password);

            // then
            actualUserToken.Should().BeEquivalentTo(expectedUserToken);

            this.userServiceMock.Verify(service => service.RetrieveAllUsers(),
                Times.Once);

            this.securityServiceMock.Verify(service => service.CreateToken(
                existingUser), Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.securityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
