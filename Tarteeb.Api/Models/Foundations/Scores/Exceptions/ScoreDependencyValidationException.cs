//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class ScoreDependencyValidationException : Xeption
    {
        public ScoreDependencyValidationException(Xeption innerException)
            : base(message: "Score dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
