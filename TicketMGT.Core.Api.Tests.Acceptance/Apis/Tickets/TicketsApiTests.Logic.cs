using System.Threading.Tasks;
using FluentAssertions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Acceptance.Apis.Tickets
{
    public partial class TicketsApiTests
    {
        [Fact]
        private async Task ShouldPostTicketAsync()
        {
            // given
            Ticket randomTicket = CreateRandomTicket();
            Ticket inputTicket = randomTicket;
            Ticket expectedTicket = inputTicket;

            // when
            await this.ticketsApiBroker
                .PostTicketAsync(inputTicket);

            Ticket actualTicket =
                await this.ticketsApiBroker.GetTicketByIdAsync(
                    inputTicket.Id);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            await this.ticketsApiBroker.DeleteTicketByIdAsync(actualTicket.Id);
        }
    }
}
