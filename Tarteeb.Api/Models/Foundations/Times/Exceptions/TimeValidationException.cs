//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class TimeValidationException : Xeption
    {
        public TimeValidationException(Xeption innerException)
            : base(message: "Time validation error occurred, fix the errors and try again.", innerException)
        { }
    }
}
