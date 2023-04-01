//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions
{
    public class TimeDependencyValidationException : Xeption
    {
        public TimeDependencyValidationException(Xeption innerException)
             : base(message: "Time dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}
