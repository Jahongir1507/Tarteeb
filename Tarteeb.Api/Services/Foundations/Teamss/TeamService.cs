using System.Linq;
using Tarteeb.Api.Brokers.DateTimes;
using Tarteeb.Api.Brokers.Loggings;
using Tarteeb.Api.Brokers.Storages;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teamss
{
    public class TeamService : ITeamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TeamService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }
        public IQueryable<Team> RetrieveAllTeams()
        {
            throw new System.NotImplementedException();
        }
    }
}
