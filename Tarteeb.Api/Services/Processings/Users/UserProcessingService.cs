using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Models;
using Tarteeb.Api.Services.Foundations.Users;

namespace Tarteeb.Api.Services.Processings.Users
{
    public partial class UserProcessingService : IUserProcessingsService
    {
        private readonly IUserService userService;
        private readonly ILoggingBroker loggingBroker;

        public UserProcessingService(IUserService userService, ILoggingBroker loggingBroker)
        {
            this.userService = userService;
            this.loggingBroker = loggingBroker;
        }

        public User RetrieveUserByCredentails(string email, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
