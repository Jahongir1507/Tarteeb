//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
        
        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Milestone>> GetAllMilestones()
        {
            try
            {
                IQueryable<Milestone> allMilesones = this.milestoneServices.RetrieveAllMilestones();

                return Ok(allMilesones);
            }
            catch (MilestoneDependencyException milestoneDependencyException)
            {
                return InternalServerError(milestoneDependencyException.InnerException);
            }
            catch (MilestoneServiceException milestoneServiceException)
            {
                return InternalServerError(milestoneServiceException.InnerException);
            } 
            
        [HttpPut]
        public async ValueTask<ActionResult<Milestone>> PutMilestoneAsync(Milestone milestone)
        {
            try
            {
                Milestone modifiedMilestone =
                    await this.milestoneServices.ModifyMilestoneAsync(milestone);

                return Ok(modifiedMilestone);
            }
            catch (MilestoneValidationException milestoneValidationException)
                when (milestoneValidationException.InnerException is NotFoundMilestoneException)
            {
                return NotFound(milestoneValidationException.InnerException);
            }
            catch (MilestoneValidationException milestoneValidationException)
            {
                return BadRequest(milestoneValidationException.InnerException);
            }
            catch (MilestoneDependencyValidationException milestoneDependencyValidationException)
            {
                return BadRequest(milestoneDependencyValidationException.InnerException);
            }
        }
    }
}
