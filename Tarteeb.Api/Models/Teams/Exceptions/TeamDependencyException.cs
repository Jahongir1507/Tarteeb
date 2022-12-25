//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class TeamDependencyException : Xeption
    {
        public TeamDependencyException(Xeption innerException)
            : base(message: "Team dependency error occurred, contact support.", innerException)
        { }
    }
}