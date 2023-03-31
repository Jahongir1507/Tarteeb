//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class FailedTimeServiceException : Xeption
    {
        public FailedTimeServiceException(Exception innerException)
            : base(message: "Failed time service error occurred, please contact support.",
                innerException)
        {}
    }
}
