using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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

        [Fact]
        private async Task ShouldGetAllTicketsAsync()
        {
            // given
            List<Ticket> randomTickets =
                await CreateRandomTickets();

            List<Ticket> expectedTickets =
                randomTickets;

            // when
            List<Ticket> actualTickets =
                await this.ticketsApiBroker.GetAllTickets();

            // then
            foreach (Ticket expectedTicket in expectedTickets)
            {
                Ticket actualTicket =
                    actualTickets.Single(
                        ticket => ticket.Id == expectedTicket.Id);

                actualTicket.Should().BeEquivalentTo(expectedTicket);

                await this.ticketsApiBroker.DeleteTicketByIdAsync(actualTicket.Id);
            }
        }

        [Fact]
        private async Task ShouldGetTicketByIdAsync()
        {
            // given
            Ticket randomTicket = await PostRandomTicketAsync();
            Ticket retrievedTicket = randomTicket;
            Ticket expectedTicket = retrievedTicket.DeepClone();

            // when
            Ticket actualTicket =
                await this.ticketsApiBroker.GetTicketByIdAsync(randomTicket.Id);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            await this.ticketsApiBroker.DeleteTicketByIdAsync(actualTicket.Id);
        }
    }
}
