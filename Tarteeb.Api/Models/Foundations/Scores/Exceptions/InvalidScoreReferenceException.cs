//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptions
{
    public class InvalidScoreReferenceException : Xeption
    {
        public InvalidScoreReferenceException(Exception innerException)
            : base(message: "Invalid score reference error occurred.", innerException)
        { } 
    }
}