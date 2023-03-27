//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Foundations.Teams.Exceptions
{
    public class AlreadyExistsTeamException : Xeption
    {
        public AlreadyExistsTeamException(Exception innerException)
            : base(message: "Team already exists.", innerException)
        { }
    }
}