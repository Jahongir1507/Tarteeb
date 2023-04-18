//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class AlreadyExistsMilestoneException : Xeption
    {
        public AlreadyExistsMilestoneException(Exception innerException)
           : base(message: "Milestone already exists.", innerException)
        { }
    }
}
