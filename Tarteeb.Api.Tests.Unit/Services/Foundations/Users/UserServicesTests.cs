//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//===============================

using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Services.Foundations.Users;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServicesTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;

        public UserServicesTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.userService =
                new UserService(this.storageBrokerMock.Object, this.loggingBrokerMock.Object);
        }
    }
}