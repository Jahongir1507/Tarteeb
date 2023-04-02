//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class FailedUserOrchestrationException : Xeption
    {
        public FailedUserOrchestrationException(Exception innerException)
            : base(message: "Failed user token service error occurred, contact support.", innerException)
        { }
    }
}
