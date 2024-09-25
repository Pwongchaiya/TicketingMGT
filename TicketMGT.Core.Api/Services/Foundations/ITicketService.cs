using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public interface ITicketService
    {
        ValueTask<Ticket> AddTicketAsync(Ticket ticket);
        ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId);
        IQueryable<Ticket> RetrieveAllTicketsAsync();
        ValueTask<Ticket> ModifyTicketAsync(Ticket ticket);
        ValueTask<Ticket> RemoveTicketByIdAsync(Guid id);
    }
}
