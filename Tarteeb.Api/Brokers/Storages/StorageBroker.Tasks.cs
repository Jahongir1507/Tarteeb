//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using Local = Tarteeb.Api.Models.Tasks;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Local.Task> Tasks { get; set; }

        public async ValueTask<Local.Task> InsertShelterAsync(Local.Task task)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Local.Task> taskEntityEntry =
                await broker.AddAsync(task);

            await broker.SaveChangesAsync();

            return taskEntityEntry.Entity;
        }
    }
}
