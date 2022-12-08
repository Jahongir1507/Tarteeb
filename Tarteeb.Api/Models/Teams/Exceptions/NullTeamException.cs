//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class NullTeamException : Xeption
    {
        public NullTeamException() : base(message: "Team is null.")
        {}
    }
}