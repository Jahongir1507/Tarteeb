//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class InvalidTimeReferenceException : Xeption
    {
        public InvalidTimeReferenceException(Exception innerException)
            : base(message: "Invalid time reference error occurred.", innerException)
        { }
    }
}
