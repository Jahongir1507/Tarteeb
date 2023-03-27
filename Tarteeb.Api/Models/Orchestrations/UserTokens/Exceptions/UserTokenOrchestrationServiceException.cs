﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Orchestrations.UserTokens.Exceptions
{
    public class UserTokenOrchestrationServiceException : Xeption
    {
        public UserTokenOrchestrationServiceException(Xeption innerException)
            : base(message: "User token service error occurred, contact support.", innerException)
        { }
    }
}
