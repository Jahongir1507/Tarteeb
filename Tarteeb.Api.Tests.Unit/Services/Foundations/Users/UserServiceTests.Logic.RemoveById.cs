using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tarteeb.Api.Models;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldRemoveUserByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserId = randomId;
            User randomUser = CreateRandomUser();
            User storageUser = randomUser;
            User expectedInputUser = storageUser;
            User deletedUser = expectedInputUser;
            User expectedUser = deletedUser.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(storageUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(deletedUser);

            // when
            User actualUser = await this.userService
                .RemoveUserByIdAsync(inputUserId);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(expectedInputUser),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}