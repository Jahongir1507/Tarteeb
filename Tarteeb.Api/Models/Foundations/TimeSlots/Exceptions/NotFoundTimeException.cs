//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.TimeSlots.Exceptions
{
    public class NotFoundTimeException : Xeption
    {
        public NotFoundTimeException(Guid timeId)
            : base(message: $"Couldn't find time with id: {timeId}.")
        { }
    }
}
