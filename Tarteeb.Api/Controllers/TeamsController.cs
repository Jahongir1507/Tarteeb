//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Teams;
using Tarteeb.Api.Models.Teams.Exceptions;
using Tarteeb.Api.Services.Foundations.Teams;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : RESTFulController
    {
        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService) =>
            this.teamService = teamService;

        [HttpPost]
        public async ValueTask<ActionResult<Team>> PostTeamAsync(Team team)
        {
            try
            {
                return await this.teamService.AddTeamAsync(team);
            }
            catch (TeamValidationException teamValidationExpection)
            {
                return BadRequest(teamValidationExpection.InnerException);
            }
            catch (TeamDependencyValidationException teamDependencyValidationException)
                when (teamDependencyValidationException.InnerException is AlreadyExistsTeamException)
            {
                return Conflict(teamDependencyValidationException.InnerException);
            }
            catch (TeamDependencyValidationException teamDependencyValidationException)
            {
                return BadRequest(teamDependencyValidationException.InnerException);
            }
            catch (TeamDependencyException teamDependencyException)
            {
                return InternalServerError(teamDependencyException.InnerException);
            }
            catch (TeamServiceException teamServiceException)
            {
                return InternalServerError(teamServiceException.InnerException);
            }
        }
    }
}