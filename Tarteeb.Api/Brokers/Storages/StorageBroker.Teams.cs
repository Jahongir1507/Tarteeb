//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Team> Teams { get; set; }

        public async ValueTask<Team> InsertTeamAsync(Team team) =>
            await InsertAsync(team);

        public IQueryable<Team> SelectAllTeams() =>
            SelectAll<Team>();

        public async ValueTask<Team> SelectTeamById(Guid id) =>
            await SelectTeamById<Team>(id);

        public async ValueTask<Team> UpdateTeamAsync(Team team) =>
            await UpdateAsync(team);


    }
}
