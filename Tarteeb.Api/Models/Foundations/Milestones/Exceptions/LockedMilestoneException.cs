//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class LockedMilestoneException : Xeption
    {
        public LockedMilestoneException(Exception innerException)
           : base(message: "Milestone is locked, please try again.", innerException)
        { }
    }
}
