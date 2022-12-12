//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class FailedTeamStorageException: Xeption
    {
        public FailedTeamStorageException(Exception innerException)
            : base(message: "Failed ticket storage error occurred, contact support.", innerException)
        { }
    }
}
