//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;

namespace Tarteeb.Api.Models.Teams
{
    public class Team
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid UpdatedUserId { get; set; }
    }
}