//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class MilestoneValidationException : Xeption
    {
        public MilestoneValidationException(Xeption innerException)
            : base(message: "Milestone validation error occurred, fix the errors and try again.", innerException)
        {}
    }
}
