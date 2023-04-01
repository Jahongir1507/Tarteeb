﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Times.Exceptions
{
    public class AlreadyExistsTimeException : Xeption
    {
        public AlreadyExistsTimeException(Exception innerException)
            : base(message: "Time already exists.", innerException)
        { }
    }
}
