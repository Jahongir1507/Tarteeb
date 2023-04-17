//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Foundations.Milestones;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Milestone> Milestones { get; set; }

        public IQueryable<Milestone> SelectAllMilestones() =>
            SelectAll<Milestone>();

        public async ValueTask<Milestone> UpdateMilestoneAsync(Milestone milestone) =>
            await UpdateAsync<Milestone>(milestone);
    }
}
