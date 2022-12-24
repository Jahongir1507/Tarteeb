//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace Tarteeb.Api.Models.Teams.Exceptions
{
    public class NotFoundTeamException : Xeption
    {
        public NotFoundTeamException(Guid teamId)
            : base(message: $"Couldn't find team with id: {teamId}.")
        { }
    }
}