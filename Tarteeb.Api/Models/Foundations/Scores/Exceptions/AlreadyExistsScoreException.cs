//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class AlreadyExistsScoreException : Xeption
    {
        public AlreadyExistsScoreException(Exception innerException)
            : base(message: "Score already exists.", innerException)
        { }
    }
}
