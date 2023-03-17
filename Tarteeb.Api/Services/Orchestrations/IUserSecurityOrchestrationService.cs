//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Orchestrations.UserTokens;

namespace Tarteeb.Api.Services.Orchestrations
{
    public interface IUserSecurityOrchestrationService
    {
        UserToken CreateUserToken(string email, string password);
    }
}
