//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class InvalidTeamException : Xeption
    {
        public InvalidTeamException()
            : base(message: "Team is invalid.")
        { }
    }
}