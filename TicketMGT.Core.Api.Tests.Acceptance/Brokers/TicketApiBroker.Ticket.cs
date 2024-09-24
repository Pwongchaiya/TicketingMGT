using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Acceptance.Brokers
{
    public partial class TicketApiBroker
    {
        private const string ticketRelativeUrl = "api/tickets";

        public async ValueTask<Ticket> PostTicketAsync(Ticket ticket) =>
           await this.apiFactoryClient.PostContentAsync(ticketRelativeUrl, ticket);
    }
}
