//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Users;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllUsers()
        {
            // given
            IQueryable<User> randomUsers = CreateRandomUsers();
            IQueryable<User> storageUsers = randomUsers;
            IQueryable<User> expectedUser = storageUsers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers()).Returns(storageUsers);

            // when
            IQueryable<User> actualUser = this.userService.RetrieveAllUsers();

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}