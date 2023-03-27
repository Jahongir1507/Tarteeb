﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class InvalidUserCredentialOrchestrationException : Xeption
    {
        public InvalidUserCredentialOrchestrationException()
            : base(message: "Credential missing. Fix the error and try again.")
        { }
    }
}
