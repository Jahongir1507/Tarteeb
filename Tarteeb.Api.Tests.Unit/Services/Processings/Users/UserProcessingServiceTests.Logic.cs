//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Processings.Users
{
    public partial class UserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddUserIfNotExistAsync()
        {
            // given
            IQueryable<User> randomUsers = CreateRandomUsers();
            IQueryable<User> retrievedUser = randomUsers;
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            User addedUser = inputUser;
            User expectedUser = addedUser.DeepClone();

            this.userServiceMock.Setup(service =>
                service.RetrieveAllUsers()).Returns(retrievedUser);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(inputUser)).ReturnsAsync(addedUser);

            // when
            User actualUser =
                await this.userProcessingService.UpsertUserAsync(inputUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userServiceMock.Verify(service =>
                service.RetrieveAllUsers(), Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(inputUser), Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}