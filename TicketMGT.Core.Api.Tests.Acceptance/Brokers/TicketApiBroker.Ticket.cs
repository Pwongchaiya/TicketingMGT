using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Acceptance.Brokers
{
    public partial class TicketApiBroker
    {
        private const string ticketRelativeUrl = "api/ticket";

        public async ValueTask<Ticket> PostTicketAsync(Ticket ticket) =>
           await this.apiFactoryClient.PostContentAsync(ticketRelativeUrl, ticket);
        public async ValueTask<List<Ticket>> GetAllTickets() =>
            await this.apiFactoryClient.GetContentAsync<List<Ticket>>($"{ticketRelativeUrl}/");

        public async ValueTask<Ticket> GetTicketByIdAsync(Guid id) =>
            await this.apiFactoryClient.GetContentAsync<Ticket>($"{ticketRelativeUrl}/{id}");

        public async ValueTask<Ticket> PutTicketAsync(Ticket ticket) =>
            await this.apiFactoryClient.PutContentAsync<Ticket>(ticketRelativeUrl, ticket);

        public async ValueTask<Ticket> DeleteTicketByIdAsync(Guid id) =>
            await this.apiFactoryClient.DeleteContentAsync<Ticket>($"{ticketRelativeUrl}/{id}");
    }
}
