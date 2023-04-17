//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Foundations.Milestones;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Milestone> InsertMilestoneAsync(Milestone milestone);
        IQueryable<Milestone> SelectAllMilestones();
        ValueTask<Milestone> UpdateMilestoneAsync(Milestone milestone);
        ValueTask<Milestone> DeleteMilestoneAsync(Milestone milestone);
    }
}
