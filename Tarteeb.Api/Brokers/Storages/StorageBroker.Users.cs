//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<User> Users { get; set; }
    }
}
