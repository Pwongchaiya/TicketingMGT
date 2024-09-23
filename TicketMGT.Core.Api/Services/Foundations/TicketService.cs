using System;
using System.Linq;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Brokers.DateTimes;
using TicketMGT.Core.Api.Brokers.Loggings;
using TicketMGT.Core.Api.Brokers.Storages;
using TicketMGT.Core.Api.Models.Tickets;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public class TicketService : ITicketService
    {
        private readonly IStorageBroker StorageBroker;        
        private readonly ILoggingBroker LoggingBroker;
        private readonly IDateTimeBroker DateTimeBroker;

        public TicketService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            DateTimeBroker = dateTimeBroker;
            LoggingBroker = loggingBroker;
            StorageBroker = storageBroker;
        }

        public ValueTask<Ticket> CreateTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Ticket> DeleteTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Ticket> GetAllTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<Ticket> GetTicketAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Ticket> UpdateTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
