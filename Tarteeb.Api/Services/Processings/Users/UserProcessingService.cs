using System.Linq;
using System.Threading.Tasks;
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

        public ValueTask<User> RetrieveUserByCredentails(string email, string password) =>
            TryCatch(async () =>
            {
                ValidateUser(email, password);

                IQueryable<User> allUser = this.userService.RetrieveAllUsers();

                return allUser.FirstOrDefault(retrievedUser => retrievedUser.Email.Equals(email)
                        && retrievedUser.Password.Equals(password));
            });
    }
}
