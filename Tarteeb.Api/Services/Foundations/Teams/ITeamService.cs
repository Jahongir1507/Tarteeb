//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public interface ITeamService
    {
        ValueTask<Team> AddTeamAsync(Team team);
        ValueTask<Team> RetrieveTeamByIdAsync(Guid teamId);
        ValueTask<Team> RemoveTeamByIdAsync(Guid TeamId);
    }
}