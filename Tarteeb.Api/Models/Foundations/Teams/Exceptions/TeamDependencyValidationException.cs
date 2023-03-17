//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Teams.Exceptions
{
    public class TeamDependencyValidationException : Xeption
    {
        public TeamDependencyValidationException(Xeption innerException)
            : base(message: "Team dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}