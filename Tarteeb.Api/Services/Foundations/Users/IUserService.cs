//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        IQueryable<User> RetrieveAllUsers();
        ValueTask<User> ModifyUserAsync(User user);
    }
}