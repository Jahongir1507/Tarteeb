//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations
{
    public interface IUserSecurityService
    {
        string CreateToken(User user);
    }
}
