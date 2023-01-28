using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Processings.Users
{
    public interface IUserProcessingsService
    {
        public User RetrieveUserByCredentails(string email, string password);
    }
}