//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService
    {
        private static void ValidateTeamNotNull(Team team)
        {
            if (team is null)
            {
                throw new NullTeamException();
            }
        }
    }
}
