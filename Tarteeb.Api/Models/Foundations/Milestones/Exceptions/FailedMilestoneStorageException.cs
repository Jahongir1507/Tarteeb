//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class FailedMilestoneStorageException : Xeption
    {
        public FailedMilestoneStorageException(Exception innerException)
            : base(message: "Failed milestone storage error occurred, contact support.", innerException)
        { }
    }
}
