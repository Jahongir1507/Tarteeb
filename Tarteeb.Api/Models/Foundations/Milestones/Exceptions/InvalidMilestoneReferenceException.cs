//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class InvalidMilestoneReferenceException : Xeption
    {
        public InvalidMilestoneReferenceException(Exception innerException)
            : base(message: "Invalid milestone reference error occurred.", innerException)
        { }
    }
}
