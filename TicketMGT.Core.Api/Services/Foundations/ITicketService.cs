using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public interface ITicketService
    {
        ValueTask<Ticket> CreateTicketAsync(Ticket ticket);
        ValueTask<Ticket> GetTicketByIdAsync(Guid ticketId);
        IQueryable<Ticket> GetAllTicketsAsync();
        ValueTask<Ticket> UpdateTicketAsync(Ticket ticket);
        ValueTask<Ticket> DeleteTicketAsync(Guid id);
    }
}
