//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class FailedTeamDependencyValidationException : Xeption
    {
        public FailedTeamDependencyValidationException(Exception innerException)
            : base(message: "Failed team dependency validation error occurred, fix the errors and try again.",
                  innerException) { }
    }
}
