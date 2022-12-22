//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
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
