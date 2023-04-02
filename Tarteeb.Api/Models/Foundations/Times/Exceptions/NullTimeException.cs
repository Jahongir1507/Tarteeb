//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class NullTimeException : Xeption
    {
        public NullTimeException() : base("Time is null.")
        { }
    }
}
