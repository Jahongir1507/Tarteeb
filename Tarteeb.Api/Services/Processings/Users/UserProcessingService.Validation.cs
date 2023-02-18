using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Processings.Exceptions;

namespace Tarteeb.Api.Services.Processings.Users
{
    public partial class UserProcessingService
    {
        private static void ValidateUser(string email,string password)
        {
            if ((email is null)&&(password is null))
            {
                throw new NullUserProcessingException();
            }
        }
    }
}
