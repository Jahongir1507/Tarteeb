//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models;

namespace Tarteeb.Api.Services.Foundations.Teams
{
    public class TeamService :ITeamService
    {
        public readonly IStorageBroker storageBroker;
        public readonly IloggingBroker loggingBroker;

        public TeamService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        ValueTask<Team> ITeamService.AddTeamAsync(Team team) => 
            throw new NotImplementedException();
    }
}
