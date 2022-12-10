//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public interface ITeamService
    {
        IQueryable<Team> RetrieveAllTeams();
    }
}
