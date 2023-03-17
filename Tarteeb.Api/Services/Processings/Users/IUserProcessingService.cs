//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Foundations.Users;

namespace Tarteeb.Api.Services.Processings.Users
{
    public interface IUserProcessingService
    {
        User RetrieveUserByCredentails(string email, string password);
    }
}