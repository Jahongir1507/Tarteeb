//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions
{
    public class FailedTimeStorageException : Xeption
    {
        public FailedTimeStorageException(Exception innerException)
            : base(message: "Failed time storage error occurred, contact support.", innerException)
        { }
    }
}
