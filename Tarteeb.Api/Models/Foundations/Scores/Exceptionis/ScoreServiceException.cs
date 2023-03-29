//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class ScoreServiceException : Xeption
    {
        public ScoreServiceException(Exception innerException) 
            :base(message: "Score service error occurred, contact support.", innerException)
        { }
    }
}
