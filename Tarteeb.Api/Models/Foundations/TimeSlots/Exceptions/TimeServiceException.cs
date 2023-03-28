//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions
{
    public class TimeServiceException : Xeption
    {
        public TimeServiceException(Xeption innerException)
            : base(message: "Time service error occurred, contact support.", innerException)
        { }
    }
}
