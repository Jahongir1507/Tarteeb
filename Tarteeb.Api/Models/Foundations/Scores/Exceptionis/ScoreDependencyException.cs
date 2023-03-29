//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class ScoreDependencyException : Xeption
    {
        public ScoreDependencyException(Xeption innerException)
            : base(message: "Score dependency error occurred, contact support.", innerException)
        { }
    }
}
