//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class LockedTeamException : Xeption
    {
        public LockedTeamException(Exception innerException)
            : base(message: "Team is locked, please try again.", innerException)
        { }
    }
}