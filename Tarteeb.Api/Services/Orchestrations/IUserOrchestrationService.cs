using Tarteeb.Api.Services.Orchestrations.Model;

namespace Tarteeb.Api.Services.Orchestrations;

public interface IUserOrchestrationService
{
    public UserToken LoginUser(string email, string password);
}