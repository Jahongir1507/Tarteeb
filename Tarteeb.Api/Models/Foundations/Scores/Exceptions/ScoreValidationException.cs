//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class ScoreValidationException:Xeption
    {
        public ScoreValidationException(Xeption innerException)
            :base(message: "Score validation error occured, fix the errors and try again.")
        { }
    }
}
