//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Milestones;
using Tarteeb.Api.Models.Foundations.Milestones.Exceptions;
using Tarteeb.Api.Services.Foundations.Milestones;

namespace Tarteeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilestonesController : RESTFulController
    {
        private readonly IMilestoneService milestoneServices;

        public MilestonesController(IMilestoneService milestoneService) =>
            this.milestoneServices = milestoneService;

        [HttpPost]
        public async ValueTask<ActionResult<Milestone>> PostMilestoneAsync(Milestone milestone)
        {
            try
            {
                return await this.milestoneServices.AddMilestoneAsync(milestone);
            }
            catch (MilestoneValidationException milestoneValidationException)
            {
                return BadRequest(milestoneValidationException.InnerException);
            }
            catch (MilestoneDependencyValidationException milestoneDependencyValidationException)
                when (milestoneDependencyValidationException.InnerException is AlreadyExistsMilestoneException)
            {
                return Conflict(milestoneDependencyValidationException.InnerException);
            }
            catch (MilestoneDependencyValidationException milestoneDependencyValidationException)
            {
                return BadRequest(milestoneDependencyValidationException.InnerException);
            }
            catch (MilestoneDependencyException milestoneDependencyException)
            {
                return InternalServerError(milestoneDependencyException.InnerException);
            }
            catch (MilestoneServiceException milestoneServiceException)
            {
                return InternalServerError(milestoneServiceException.InnerException);
            }
        }
    }
}
