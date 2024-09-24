using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Brokers.DateTimes;
using TicketMGT.Core.Api.Brokers.Loggings;
using TicketMGT.Core.Api.Brokers.Storages;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public partial class TicketService : ITicketService
    {
        private readonly IStorageBroker storageBroker;        
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TicketService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.storageBroker = storageBroker;
        }

        public ValueTask<Ticket> CreateTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicketOnAdd(ticket);

            return await storageBroker.AddTicketAsync(ticket);
        });

        public IQueryable<Ticket> GetAllTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Ticket> GetTicketByIdAsync(Guid ticketId)
        {
            return await storageBroker.GetTicketByIdAsync(ticketId);
        }

        public ValueTask<Ticket> DeleteTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Ticket> UpdateTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
