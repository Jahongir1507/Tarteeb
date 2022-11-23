//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Users.Exceptions
{
    public class UserValidationException :Xeption
    {
        public UserValidationException(Xeption innerExeption)
            :base(message:"User validation error occured,fix the errors and try again.",
                 innerExeption)
        {}
    }
}
