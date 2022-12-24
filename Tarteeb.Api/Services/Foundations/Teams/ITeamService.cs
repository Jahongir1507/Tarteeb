//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public interface ITeamService
    {
        ValueTask<Team> AddTeamAsync(Team team);
        IQueryable<Team> RetrieveAllTeams();
        ValueTask<Team> RetrieveTeamByIdAsync(Guid teamId);
        ValueTask<Team> ModifyTeamAsync(Team team);
    }
}