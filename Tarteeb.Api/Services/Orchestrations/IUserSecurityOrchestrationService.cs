//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace Tarteeb.Api.Services.Orchestrations
{
    internal interface IUserSecurityOrchestrationService
    {
        object LoginUser(string email, string password);
    }
}