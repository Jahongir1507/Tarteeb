//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class ScoreDependencyException : Xeption
    {
        public ScoreDependencyException(Exception innerException)
            : base(message: "Team dependency error occurred, contact support.", innerException)
        { }
    }
}
