//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Teams.Exceptions
{
    public class TeamValidationException : Xeption
    {
        public TeamValidationException(Xeption innerException)
            : base(message: "Team validation error occured, fix the errors and try again.", innerException)
        { }
    }
}