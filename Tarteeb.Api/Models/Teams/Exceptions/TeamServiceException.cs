//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class TeamServiceException: Xeption
    {
        public TeamServiceException(Xeption innerException)
            : base(message: "Team service error occurred, contact support.", innerException)
        { }
    }
}