//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using PostmarkDotNet.Model;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class NullMilestoneException : Xeption
    {
        public NullMilestoneException()
            : base(message: "Milestone is null.")
        {}
    }
}
