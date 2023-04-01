//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class NullScoreException : Xeption
    {
        public NullScoreException() : base(message: "Score is null.")
        { }
    }
}
