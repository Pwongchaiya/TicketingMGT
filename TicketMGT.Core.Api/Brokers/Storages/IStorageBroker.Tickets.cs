using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Ticket> InsertTicketAsync(Ticket ticket);

        ValueTask<Ticket> SelectTicketByIdAsync(Guid ticketId);
        IQueryable<Ticket> SelectAllTicketsAsync();

        ValueTask<Ticket> UpdateTicketAsync(Ticket ticket);

        ValueTask<Ticket> DeleteTicketAsync(Ticket ticket);
    }
}
