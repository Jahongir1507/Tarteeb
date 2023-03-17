//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException()
            : base(message: "User with the credentials doesn't exist")
        { }
    }
}
