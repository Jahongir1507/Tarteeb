using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class TimeServiceException : Xeption
    {
        public TimeServiceException(Exception innerException)
            : base(message: "Time service error occurred, contact support.", innerException)
        { }
    }
}
