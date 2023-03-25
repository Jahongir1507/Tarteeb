//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Time> Times { get; set; }

        public async ValueTask<Time> DeleteTimeAsync(Time time) =>
           await DeleteAsync(time);
    }
}