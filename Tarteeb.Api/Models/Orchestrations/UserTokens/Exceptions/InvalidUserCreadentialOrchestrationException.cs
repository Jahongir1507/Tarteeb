//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class InvalidUserCreadentialOrchestrationException : Xeption
    {
        public InvalidUserCreadentialOrchestrationException()
            : base(message: "Credential missing. Fix the error and try again.")
        { }
    }
}
