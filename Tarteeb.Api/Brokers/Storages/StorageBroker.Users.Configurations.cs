//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarteeb.Api.Models.Foundations.Users;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public void ConfigureUserEmail(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.GitHubUsername).IsUnique().HasFilter("[GitHubUsername] NULL");
            builder.HasIndex(user => user.TelegramUsername).IsUnique().HasFilter("[TelegramUsername] NULL");
        }
    }
}
