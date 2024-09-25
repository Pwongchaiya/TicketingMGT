using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Ticket> Tickets { get; set; }

        public async ValueTask<Ticket> InsertTicketAsync(Ticket ticket) =>
            await InsertAsync(ticket);

        public async ValueTask<Ticket> SelectTicketByIdAsync(Guid ticketId) =>
            await SelectAsync<Ticket>(ticketId);

        public IQueryable<Ticket> SelectAllTicketsAsync() =>
            SelectAll<Ticket>();

        public async ValueTask<Ticket> UpdateTicketAsync(Ticket ticket) =>
            await UpdateAsync(ticket);

        public async ValueTask<Ticket> DeleteTicketAsync(Ticket ticket) =>
            await DeleteAsync(ticket);
    }
}
