//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models;
using Tarteeb.Api.Models.Tickets;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<User> Users { get; set; }

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>();

        public async ValueTask<User> SelectUserByIdAsync(Guid id) =>
            await SelectAsync<User>(id);

        public async ValueTask<User> UpdateUserAsync(User user) =>
            await UpdateAsync(user);

        public async ValueTask<User> GetUserAsync(User user) =>
            await DeleteAsync(user);
    }
}
