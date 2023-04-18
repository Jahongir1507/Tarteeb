//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Milestones.Exceptions
{
    public class MilestoneDependencyValidationException : Xeption
    {
        public MilestoneDependencyValidationException(Xeption innerException)
           : base(message: "Milestone dependency validation error occurred, fix the errors and try again.",
               innerException)
        { }
    }
}
