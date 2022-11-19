//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Team> InsertTeamAsync(Team team);
        IQueryable<Team> SelectAllTeams();
        ValueTask<Team> UpdateTeamAsync(Team team);
        ValueTask<Team> DeleteTeamAsync(Team team);
    }
}