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
using Tarteeb.Api.Models.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Tarteeb.Api.Services.Foundations.Times;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesController : RESTFulController
    {
        private readonly ITimeService timeService;

        public TimesController(ITimeService timeService) =>
            this.timeService = timeService;

        [HttpPost]
        public async ValueTask<ActionResult<Time>> PostTimeAsync(Time time)
        {
            try
            {
                return await this.timeService.AddTimeAsync(time);
            }
            catch (TimeValidationException timeValidationException)
            {
                return BadRequest(timeValidationException.InnerException);
            }
            catch (TimeDependencyValidationException timeDependencyValidationException)
                when (timeDependencyValidationException.InnerException is AlreadyExistsTimeException)
            {
                return Conflict(timeDependencyValidationException.InnerException);
            }
            catch (TimeDependencyValidationException timeDependencyValidationException)
            {
                return BadRequest(timeDependencyValidationException.InnerException);
            }
            catch (TimeDependencyException timeDependencyException)
            {
                return InternalServerError(timeDependencyException.InnerException);
            }
            catch (TimeServiceException timeServiceException)
            {
                return InternalServerError(timeServiceException.InnerException);
            }
        }


        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Time>> GetAllTimes()
        {
            try
            {
                IQueryable<Time> allTimes = this.timeService.RetrieveAllTimes();

                return Ok(allTimes);
            }
            catch (TimeDependencyException timeDependencyException)
            {
                return InternalServerError(timeDependencyException.InnerException);
            }
            catch (TimeServiceException timeServiceException)
            {
                return InternalServerError(timeServiceException.InnerException);
            }
        }

        [HttpDelete("{timeId}")]
        public async ValueTask<ActionResult<Time>> DeleteTimeByIdAsync(Guid timeId)
        {
            try
            {
                Time deletedTime =
                    await this.timeService.RemoveTimeByIdAsync(timeId);

                return Ok(deletedTime);
            }
            catch (TimeValidationException timeValidationException)
                when (timeValidationException.InnerException is NotFoundTimeException)
            {
                return NotFound(timeValidationException.InnerException);
            }
            catch (TimeValidationException timeValidationException)
            {
                return BadRequest(timeValidationException.InnerException);
            }
            catch (TimeDependencyValidationException timeDependencyValidationException)
                when (timeDependencyValidationException.InnerException is LockedTimeException)
            {
                return Locked(timeDependencyValidationException.InnerException);
            }
            catch (TimeDependencyValidationException timeDependencyValidationException)
            {
                return BadRequest(timeDependencyValidationException.InnerException);
            }
            catch (TimeDependencyException timeDependencyException)
            {
                return InternalServerError(timeDependencyException.InnerException);
            }
            catch (TimeServiceException timeServiceException)
            {
                return InternalServerError(timeServiceException.InnerException);
            }
        }
    }
}
