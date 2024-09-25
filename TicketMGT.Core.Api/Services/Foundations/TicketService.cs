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

        public ValueTask<Ticket> AddTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicketOnAdd(ticket);

            return await storageBroker.InsertTicketAsync(ticket);
        });

        public IQueryable<Ticket> RetrieveAllTicketsAsync() =>
        TryCatch(() => storageBroker.SelectAllTicketsAsync());

        public ValueTask<Ticket> RetrieveTicketByIdAsync(Guid ticketId) =>
        TryCatch(async () =>
        {
            ValidateId(ticketId);

            Ticket maybeTicket = await storageBroker.SelectTicketByIdAsync(ticketId);

            ValidateTicketExists(maybeTicket, ticketId);

            return maybeTicket;
        });

        public ValueTask<Ticket> ModifyTicketAsync(Ticket ticket) =>
        TryCatch(async () =>
        {
            ValidateTicketOnModify(ticket);

            Ticket maybeTicket =
            await storageBroker.SelectTicketByIdAsync(ticket.Id);

            ValidateStorageTicketOnModify(
                inputTicket: ticket,
                storageTicket: maybeTicket);

            return await storageBroker.UpdateTicketAsync(ticket);
        });

        public ValueTask<Ticket> RemoveTicketByIdAsync(Guid id) =>
            TryCatch(async () =>
            {
                ValidateId(id);

                Ticket maybeTicket = await this.storageBroker
                    .SelectTicketByIdAsync(id);

                ValidateTicketExists(maybeTicket, id);

                return await this.storageBroker.DeleteTicketAsync(maybeTicket);
            });
    }
}
