//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class UserTokenOrchestrationDependencyException : Xeption
    {
        public UserTokenOrchestrationDependencyException(Xeption innerException)
            : base(message: "User token dependency error occurred, contact support.", innerException)
        { }
    }
}
