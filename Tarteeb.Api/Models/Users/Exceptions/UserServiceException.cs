//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Users.Exceptions
{
    public class UserServiceException : Xeption
    {
        public UserServiceException(Exception innerException)
            : base(message: "User service error occured,contact support.", innerException)
        { }
    }
}
