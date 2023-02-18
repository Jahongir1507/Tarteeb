using System.Threading.Tasks;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Processings.Users
{
    public interface IUserProcessingsService
    {
        public ValueTask<User> RetrieveUserByCredentails(string email, string password);
    }
}