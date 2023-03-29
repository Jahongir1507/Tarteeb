//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class InvalidScoreException : Xeption
    {
        public InvalidScoreException()
            : base(message: "Score is invalid.")
        { }
    }
}
