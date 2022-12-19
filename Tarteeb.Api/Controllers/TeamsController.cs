//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Teams;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : RESTFulController
    {
        [HttpPost]
        public async ValueTask<ActionResult<Team>> PostTeamAsync(Team team)
        {
            throw new NotImplementedException();
        }
    }
}