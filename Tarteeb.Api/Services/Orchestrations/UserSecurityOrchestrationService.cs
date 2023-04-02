//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models.Foundations.Users;
using Tarteeb.Api.Models.Orchestrations.UserTokens;
using Tarteeb.Api.Services.Foundations.Emails;
using Tarteeb.Api.Services.Foundations.Securities;
using Tarteeb.Api.Services.Foundations.Users;

namespace Tarteeb.Api.Services.Orchestrations
{
    public partial class UserSecurityOrchestrationService : IUserSecurityOrchestrationService
    {
        private readonly IUserService userService;
        private readonly ISecurityService securityService;
        private readonly IEmailService emailService;
        private readonly ILoggingBroker loggingBroker;

        public UserSecurityOrchestrationService(
            IUserService userService,
            ISecurityService securityService,
            IEmailService emailService,
            ILoggingBroker loggingBroker)
        {
            this.userService = userService;
            this.securityService = securityService;
            this.emailService = emailService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> CreateUserAccountAsync(User user) =>
            throw new System.NotImplementedException();

        public UserToken CreateUserToken(string email, string password) =>
        TryCatch(() =>
        {
            ValidateEmailAndPassword(email, password);
            User maybeUser = RetrieveUserByEmailAndPassword(email, password);
            ValidateUserExists(maybeUser);
            string token = this.securityService.CreateToken(maybeUser);

            return new UserToken
            {
                UserId = maybeUser.Id,
                Token = token
            };
        });

        private User RetrieveUserByEmailAndPassword(string email, string password)
        {
            IQueryable<User> allUser = this.userService.RetrieveAllUsers();

            return allUser.FirstOrDefault(retrievedUser => retrievedUser.Email.Equals(email)
                    && retrievedUser.Password.Equals(password));
        }
    }
}
