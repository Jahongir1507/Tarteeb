//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;

namespace Tarteeb.Api.Models.Foundations.Scores
{
    public class Score
    {
        public Guid Id { get; set; }
        public int Grade { get; set; }
        public float Weight { get; set; }
        public string EffortLink { get; set; }
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
