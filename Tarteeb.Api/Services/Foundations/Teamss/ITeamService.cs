using System.Linq;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Services.Foundations.Teamss
{
    public interface ITeamService
    {
        IQueryable<Team>RetrieveAllTeams(); 
    }
}
