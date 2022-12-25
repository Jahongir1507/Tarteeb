//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public partial class TeamService : ITeamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TeamService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Team> AddTeamAsync(Team team) =>
        TryCatch(async () =>
        {
            ValidateTeam(team);

            return await this.storageBroker.InsertTeamAsync(team);
        });

        public IQueryable<Team> RetrieveAllTeams() =>
        TryCatch(() => this.storageBroker.SelectAllTeams());

        public ValueTask<Team> RetrieveTeamByIdAsync(Guid teamId) =>
        TryCatch(async () =>
        {
            ValidateTeamId(teamId);

            Team maybeTeam =
                await storageBroker.SelectTeamByIdAsync(teamId);

            ValidateStorageTeam(maybeTeam, teamId);

            return maybeTeam;
        });

        public ValueTask<Team> ModifyTeamAsync(Team team) =>
        TryCatch(async () =>
        {
            ValidateTeamOnModify(team);

            var maybeTeam = 
                await this.storageBroker.SelectTeamByIdAsync(team.Id);

            ValidateAgainstStorageTeamOnModify(inputTeam: team, storageTeam: maybeTeam);

            return await this.storageBroker.UpdateTeamAsync(team);
        });

        public ValueTask<Team> RemoveTeamByIdAsync(Guid teamId) =>
        TryCatch(async () =>
        {
            ValidateTeamId(teamId);

            Team maybeTeam =
                await this.storageBroker.SelectTeamByIdAsync(teamId);

            ValidateStorageTeam(maybeTeam, teamId);

            return await this.storageBroker.DeleteTeamAsync(maybeTeam);
        });
    }
}