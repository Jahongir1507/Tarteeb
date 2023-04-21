//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class NotFoundMilestoneException : Xeption
    {
        public NotFoundMilestoneException(Guid milestoneId)
           : base(message: $"Couldn't find milestone with id: {milestoneId}.")
        { }
    }
}
