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
        IQueryable<Milestone> SelectAllMilestones();
        ValueTask<Milestone> UpdateMilestoneAsync(Milestone milestone);
    }
}
