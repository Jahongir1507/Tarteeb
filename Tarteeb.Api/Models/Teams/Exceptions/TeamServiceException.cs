//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class TeamServiceException : Xeption
    {
        public TeamServiceException(Exception innerException)
            : base(message: "Team service error occurred, contact support.", innerException)
        { }
    }
}