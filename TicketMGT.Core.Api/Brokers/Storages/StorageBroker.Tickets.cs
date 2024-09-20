using System.Linq;
using System.Threading.Tasks;
using System;
using TicketMGT.Core.Api.Models.Tickets;
using Microsoft.EntityFrameworkCore;

namespace TicketMGT.Core.Api.Brokers.Storages
{
    partial class StorageBroker
    {
        public DbSet<Ticket> Tickets { get; set; }

        public async ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
            await InsertAsync(ticket);

        public async ValueTask<Ticket> GetTicketByIdAsync(Guid ticketId) =>
            await SelectAsync<Ticket>(ticketId);

        public IQueryable<Ticket> GetAllTicketsAsync() =>
            SelectAll<Ticket>();

        public async ValueTask<Ticket> UpdateTicketAsync(Ticket ticket) =>
            await UpdateAsync(ticket);

        public async ValueTask<Ticket> DeleteTicketAsync(Ticket ticket) =>
            await DeleteAsync(ticket);
    }
}
