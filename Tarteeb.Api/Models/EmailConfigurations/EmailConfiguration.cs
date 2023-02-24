//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.ComponentModel.DataAnnotations;

namespace Tarteeb.Api.Models.EmailConfigurations
{
    public class EmailConfiguration
    {
        public string Host { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
