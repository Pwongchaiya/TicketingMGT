using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Ticket> AddTicketAsync(Ticket ticket);

        ValueTask<Ticket> GetTicketByIdAsync(Guid ticketId);
        IQueryable<Ticket> GetAllTicketsAsync();

        ValueTask<Ticket> UpdateTicketAsync(Ticket ticket);

        ValueTask<Ticket> DeleteTicketAsync(Ticket ticket);
    }
}
