//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Services.Foundations.Times;
using Tarteeb.Api.Models.Foundations.Times.Exceptions;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesController : RESTFulController
    {
        private readonly ITimeService timeService;

        public TimesController(ITimeService timeService) =>
            this.timeService = timeService;

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
    }
}
