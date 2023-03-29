//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Scores.Exceptionis
{
    public class FailedScoreStorageException : Xeption
    {
        public FailedScoreStorageException(Exception innerException)
            : base(message: "Failed score storage error occurred, contact support.", innerException)
        { }
    }
}
