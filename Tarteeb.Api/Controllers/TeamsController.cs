//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

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

        [HttpGet("{teamId}")]
        public async ValueTask<ActionResult<Team>> GetTeamByIdAsync(Guid Id)
        {
            try
            {
                return await this.teamService.RetrieveTeamByIdAsync(Id);
            }
            catch (TeamDependencyException teamDependencyException)
            {
                return InternalServerError(teamDependencyException.InnerException);
            }
            catch (TeamValidationException teamValidationException)
                when (teamValidationException.InnerException is InvalidTeamException)
            {
                return BadRequest(teamValidationException.InnerException);
            }
            catch (TeamValidationException teamValidationException)
                when (teamValidationException.InnerException is NotFoundTeamException)
            {
                return NotFound(teamValidationException.InnerException);
            }
            catch (TeamServiceException teamServiceException)
            {
                return InternalServerError(teamServiceException.InnerException);
            }
        }
    }
}