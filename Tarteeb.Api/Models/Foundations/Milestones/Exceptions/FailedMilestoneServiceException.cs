//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using PostmarkDotNet.Model;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class FailedMilestoneServiceException : Xeption
    {
        public FailedMilestoneServiceException(Exception innerException)
            : base(message: "Milestone service error occurred, contact support.", innerException)
        {
            
        }
    }
}
