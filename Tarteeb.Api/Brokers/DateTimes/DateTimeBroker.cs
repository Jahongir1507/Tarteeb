//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;

namespace Tarteeb.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTime() =>
            DateTimeOffset.UtcNow;
    }
}
