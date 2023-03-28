//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class FailedUserTokenOrchestrationException : Xeption
    {
        public FailedUserTokenOrchestrationException(Exception innerException)
            : base(message: "Failed user token service error occurred, contact support.", innerException)
        { }
    }
}
