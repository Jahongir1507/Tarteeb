//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class TimeDependencyException : Xeption
    {
        public TimeDependencyException(Xeption innerException)
           : base(message: "Time dependency error occurred, contact support.", innerException)
        { }
    }
}
