//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService
    {
        private static void ValidateTeam(Team team)
        {
            ValidateTeamNotNull(team);

            Validate(
                (Rule: IsInvalid(team.Id), Parameter: nameof(Team.Id)),
                (Rule: IsInvalid(team.TeamName), Parameter: nameof(Team.TeamName)),
                (Rule: IsInvalid(team.CreatedDate), Parameter: nameof(Team.CreatedDate)),
                (Rule: IsInvalid(team.UpdatedDate), Parameter: nameof(Team.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static void ValidateTeamNotNull(Team team)
        {
            if (team is null)
            {
                throw new NullTeamException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTeamException = new InvalidTeamException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTeamException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTeamException.ThrowIfContainsErrors();
        }
    }
}
