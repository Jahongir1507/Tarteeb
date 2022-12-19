//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using System.Linq;
using Tarteeb.Api.Models;
using System;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> AddUserAsync(User user);
        IQueryable<User> RetrieveAllUsers();
        ValueTask<User> RetrieveUserByIdAsync(Guid userId);
        ValueTask<User> RemoveUserByIdAsync(Guid userId);
    }
}