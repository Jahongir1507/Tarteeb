//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;

namespace Tarteeb.Api.Models.Foundations.Milestones
{
    public class Milestone
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
