using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Services.Foundations;

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
                Ticket createdTicket = await ticketService.CreateTicketAsync(ticket);
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
    }
}
