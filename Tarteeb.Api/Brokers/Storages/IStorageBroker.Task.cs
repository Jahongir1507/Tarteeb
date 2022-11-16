//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Team> UpdateTeamAsync(Team team);
    }
}
