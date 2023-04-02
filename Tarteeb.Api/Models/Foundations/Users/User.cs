//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Text.Json.Serialization;
using Tarteeb.Api.Models.Foundations.Teams;

namespace Tarteeb.Api.Models.Foundations.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }
        public string IsVerififed { get; set; }
        public string GitHubUsername { get; set; }
        public string TelegramUsername { get; set; }

        public Guid TeamId { get; set; }
        [JsonIgnore]
        public Team Team { get; set; }
    }
}
