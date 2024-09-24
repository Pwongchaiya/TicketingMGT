using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Services.Foundations;
using System;
using System.Linq;

namespace TicketMGT.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : RESTFulController
    {
        private readonly ITicketService ticketService;

        public TicketController(ITicketService ticketService) =>
            this.ticketService = ticketService;

        [HttpPost]
        public async ValueTask<ActionResult<Ticket>> PostTicketAsync(Ticket ticket)
        {
            try
            {
                Ticket createdTicket = await ticketService.AddTicketAsync(ticket);
                return Created(createdTicket);
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
        public ActionResult<IQueryable<Ticket>> GetAllTickets()
        {
            try
            {
                IQueryable<Ticket> tickets = ticketService.RetrieveAllTicketsAsync();
                return Ok(tickets);
            }
            catch (TicketDependencyException ticketDependencyException)
            {
                return Problem(ticketDependencyException.Message);
            }
            catch (TicketServiceException ticketServiceException)
            {
                return Problem(ticketServiceException.Message);
            }
        }

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<Ticket>> GetTicketByIdAsync(Guid id)
        {
            try
            {
                Ticket ticket = await ticketService.RetrieveTicketByIdAsync(id);
                return Ok(ticket);
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
