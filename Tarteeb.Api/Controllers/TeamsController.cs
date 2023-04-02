//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Teams;
using Tarteeb.Api.Models.Foundations.Teams.Exceptions;
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

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Team>> GetAllTeams()
        {
            try
            {
                IQueryable<Team> allTeams = this.teamService.RetrieveAllTeams();

                return Ok(allTeams);
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
        public async ValueTask<ActionResult<Team>> GetTeamByIdAsync(Guid teamId)
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

        [HttpPut]
        public async ValueTask<ActionResult<Team>> PutTeamAsync(Team team)
        {
            try
            {
                Team modifiedTeam =
                    await this.teamService.ModifyTeamAsync(team);

                return Ok(modifiedTeam);
            }
            catch (TeamValidationException teamValidationException)
                when (teamValidationException.InnerException is NotFoundTeamException)
            {
                return NotFound(teamValidationException.InnerException);
            }
            catch (TeamValidationException teamValidationException)
            {
                return BadRequest(teamValidationException.InnerException);
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

        [HttpDelete("{teamId}")]
        public async ValueTask<ActionResult<Team>> DeleteTeamByIdAsync(Guid teamId)
        {
            try
            {
                Team deletedTeam =
                    await this.teamService.RemoveTeamByIdAsync(teamId);

                return Ok(deletedTeam);
            }
            catch (TeamValidationException teamValidationException)
                when (teamValidationException.InnerException is NotFoundTeamException)
            {
                return NotFound(teamValidationException.InnerException);
            }
            catch (TeamValidationException teamValidationException)
            {
                return BadRequest(teamValidationException.InnerException);
            }
            catch (TeamDependencyValidationException teamDependencyValidationException)
                when (teamDependencyValidationException.InnerException is LockedTeamException)
            {
                return Locked(teamDependencyValidationException.InnerException);
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