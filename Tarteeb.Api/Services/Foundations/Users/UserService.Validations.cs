//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Users.Exceptions;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private static void ValidateUserNotNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }
    }
}
