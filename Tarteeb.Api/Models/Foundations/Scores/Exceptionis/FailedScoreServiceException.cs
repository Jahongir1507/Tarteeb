//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class FailedScoreServiceException : Xeption
    {
        public FailedScoreServiceException(Exception innerException) 
            :base(message: "Failed score service error occurred, please contact support.", innerException)
        { }
    }
}
