//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Moq;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Services.Foundations.Securities;
using Tarteeb.Api.Services.Foundations.Users;
using Tarteeb.Api.Services.Orchestrations;

namespace Tarteeb.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationServiceTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<ISecurityService> securityServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserSecurityOrchestrationService userSecurityOrchestrationService;

        public UserSecurityOrchestrationServiceTests()
        {
            userServiceMock = new Mock<IUserService>();
            securityServiceMock = new Mock<ISecurityService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userSecurityOrchestrationService = new UserSecurityOrchestrationService(
                userService: userServiceMock.Object,
                securityService: securityServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }
    }
}
