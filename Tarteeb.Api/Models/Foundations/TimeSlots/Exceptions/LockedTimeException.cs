//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions
{
    public class LockedTimeException : Xeption
    {
        public LockedTimeException(Exception innerException) 
            : base(message: "Time is locked, please try again.", innerException)
        { }
    }
}
