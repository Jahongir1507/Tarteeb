//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class InvalidMilestoneException : Xeption
    {
        public InvalidMilestoneException()
            : base(message: "Milestone is invalid.")
        { }
    }
}
