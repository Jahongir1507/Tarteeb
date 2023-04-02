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
using Tarteeb.Api.Models.Foundations.Tickets;
using Tarteeb.Api.Models.Foundations.Tickets.Exceptions;
using Tarteeb.Api.Services.Foundations.Tickets;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : RESTFulController
    {
        private readonly ITicketService ticketService;

        public TicketsController(ITicketService ticketService) =>
            this.ticketService = ticketService;

        [HttpPost]
        public async ValueTask<ActionResult<Ticket>> PostTicketAsync(Ticket ticket)
        {
            try
            {
                return await this.ticketService.AddTicketAsync(ticket);
            }
            catch (TicketValidationException ticketValidationException)
            {
                return BadRequest(ticketValidationException.InnerException);
            }
            catch (TicketDependencyValidationException ticketDependencyValidationException)
                when (ticketDependencyValidationException.InnerException is AlreadyExistsTicketException)
            {
                return Conflict(ticketDependencyValidationException.InnerException);
            }
            catch (TicketDependencyValidationException ticketDependencyValidationException)
            {
                return BadRequest(ticketDependencyValidationException.InnerException);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return InternalServerError(ticketDependencyException.InnerException);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return InternalServerError(ticketServiceException.InnerException);
            }
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Ticket>> GetAllTickets()
        {
            try
            {
                IQueryable<Ticket> allTickets = this.ticketService.RetrieveAllTickets();

                return Ok(allTickets);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return InternalServerError(ticketDependencyException.InnerException);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return InternalServerError(ticketServiceException.InnerException);
            }
        }

        [HttpGet("{ticketId}")]
        public async ValueTask<ActionResult<Ticket>> GetTicketByIdAsync(Guid ticketId)
        {
            try
            {
                return await this.ticketService.RetrieveTicketByIdAsync(id);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return InternalServerError(ticketDependencyException.InnerException);
            }
            catch (TicketValidationException ticketValidationException)
                when (ticketValidationException.InnerException is InvalidTicketException)
            {
                return BadRequest(ticketValidationException.InnerException);
            }
            catch (TicketValidationException ticketValidationException)
                when (ticketValidationException.InnerException is NotFoundTicketException)
            {
                return NotFound(ticketValidationException.InnerException);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return InternalServerError(ticketServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Ticket>> PutTicketAsync(Ticket ticket)
        {
            try
            {
                Ticket modifiedTicket =
                    await this.ticketService.ModifyTicketAsync(ticket);

                return Ok(modifiedTicket);
            }
            catch (TicketValidationException ticketValidationException)
                when (ticketValidationException.InnerException is NotFoundTicketException)
            {
                return NotFound(ticketValidationException.InnerException);
            }
            catch (TicketValidationException ticketValidationException)
            {
                return BadRequest(ticketValidationException.InnerException);
            }
            catch (TicketDependencyValidationException ticketDependencyValidationException)
            {
                return BadRequest(ticketDependencyValidationException.InnerException);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return InternalServerError(ticketDependencyException.InnerException);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return InternalServerError(ticketServiceException.InnerException);
            }
        }

        [HttpDelete("{ticketId}")]
        public async ValueTask<ActionResult<Ticket>> DeleteTicketByIdAsync(Guid ticketId)
        {
            try
            {
                Ticket deletedTicket =
                    await this.ticketService.RemoveTicketByIdAsync(ticketId);

                return Ok(deletedTicket);
            }
            catch (TicketValidationException ticketValidationException)
                when (ticketValidationException.InnerException is NotFoundTicketException)
            {
                return NotFound(ticketValidationException.InnerException);
            }
            catch (TicketValidationException ticketValidationException)
            {
                return BadRequest(ticketValidationException.InnerException);
            }
            catch (TicketDependencyValidationException ticketDependencyValidationException)
                when (ticketDependencyValidationException.InnerException is LockedTicketException)
            {
                return Locked(ticketDependencyValidationException.InnerException);
            }
            catch (TicketDependencyValidationException ticketDependencyValidationException)
            {
                return BadRequest(ticketDependencyValidationException.InnerException);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return InternalServerError(ticketDependencyException.InnerException);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return InternalServerError(ticketServiceException.InnerException);
            }
        }
    }
}
