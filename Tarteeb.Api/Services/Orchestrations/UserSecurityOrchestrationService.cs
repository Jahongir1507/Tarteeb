//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Orchestrations.UserTokens;
using Tarteeb.Api.Services.Foundations.Securities;
using Tarteeb.Api.Services.Foundations.Users;

namespace Tarteeb.Api.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationService : IUserSecurityOrchestrationService
    {
        private readonly IUserService userService;
        private readonly ISecurityService securityService;
        private readonly ILoggingBroker loggingBroker;

        public UserSecurityOrchestrationService(
            IUserService userService,
            ISecurityService securityService,
            ILoggingBroker loggingBroker)
        {
            this.userService = userService;
            this.securityService = securityService;
            this.loggingBroker = loggingBroker;
        }

        public UserToken CreateUserToken(string email, string password) =>
            throw new System.NotImplementedException();
    }
}
