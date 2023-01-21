using Tarteeb.Api.Services.Orchestrations.Model;

namespace Tarteeb.Api.Services.Orchestrations;

public interface IUserSecurityOrchestrationService
{
    public UserToken LoginUser(string email, string password);
}