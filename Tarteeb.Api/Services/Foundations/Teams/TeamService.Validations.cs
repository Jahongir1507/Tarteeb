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
        private void ValidateTeam(Team team)
        {
            ValidateTeamNotNull(team);

            Validate(
                (Rule: IsInvalid(team.Id), Parameter: nameof(Team.Id)),
                (Rule: IsInvalid(team.TeamName), Parameter: nameof(Team.TeamName)),
                (Rule: IsInvalid(team.CreatedDate), Parameter: nameof(Team.CreatedDate)),
                (Rule: IsInvalid(team.UpdatedDate), Parameter: nameof(Team.UpdatedDate)),
                (Rule: IsNotRecent(team.CreatedDate), Parameter: nameof(Team.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: team.CreatedDate,
                    secondDate: team.UpdatedDate,
                    secondDateName: nameof(Team.UpdatedDate)),
                Parameter: nameof(Team.CreatedDate)));
        }

        private void ValidateTeamOnModify(Team team)
        {
            ValidateTeamNotNull(team);
            Validate(
                (Rule: IsInvalid(team.Id), Parameter: nameof(Team.Id)),
                (Rule: IsInvalid(team.TeamName), Parameter: nameof(Team.TeamName)),
                (Rule: IsInvalid(team.CreatedDate), Parameter: nameof(Team.CreatedDate)),
                (Rule: IsInvalid(team.UpdatedDate), Parameter: nameof(Team.UpdatedDate)),
                (Rule: IsNotRecent(team.UpdatedDate), Parameter: nameof(Team.UpdatedDate)),

                (Rule: IsSame(
                        firstDate: team.UpdatedDate,
                        secondDate: team.CreatedDate,
                        secondDateName: nameof(team.CreatedDate)),
                     Parameter: nameof(team.UpdatedDate)));
        }

        private static void ValidateStorageTeam(Team maybeTeam, Guid teamId)
        {
            if (maybeTeam is null)
            {
                throw new NotFoundTeamException(teamId);
            }
        }

        private static void ValidateAginstStorageTeamOnModify(Team inputTeam, Team storageTeam)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTeam.CreatedDate,
                    secondDate: storageTeam.CreatedDate,
                    secondDateName: nameof(Team.CreatedDate)),
                Parameter: nameof(Team.CreatedDate)),

                (Rule: IsSame(
                        firstDate: inputTeam.UpdatedDate,
                        secondDate: storageTeam.UpdatedDate,
                        secondDateName: nameof(Team.UpdatedDate)),
                Parameter: nameof(Team.UpdatedDate)));
        }

        private void ValidateTeamId(Guid teamId) =>
            Validate((Rule: IsInvalid(teamId), Parameter: nameof(Team.Id)));

        private void ValidateStorageTeam(Team maybeTeam, Guid teamId)
        {
            if (maybeTeam is null)
            {
                throw new NotFoundTeamException(teamId);
            }
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

        private static dynamic IsNotSame
            (DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}."
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent."
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

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
