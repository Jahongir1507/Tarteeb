//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class InvalidScoreExpetion : Xeption
    {
        public InvalidScoreExpetion()
            : base (message: "Score is invalid.")
        { }
    }
}
