//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class MilestoneServiceException : Xeption
    {
        public MilestoneServiceException(Exception innerException)
            : base(message: "Milestone service error occurred, contact support.", innerException)
        { }
    }
}
