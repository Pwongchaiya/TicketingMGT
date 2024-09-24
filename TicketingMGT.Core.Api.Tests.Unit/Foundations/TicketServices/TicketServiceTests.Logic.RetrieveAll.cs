using FluentAssertions;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTickets()
        {
            // given
            DateTimeOffset dates = GetRandomDateTimeOffset();

            IQueryable<Ticket> randomTickets =
                CreateRandomTickets(dates);

            IQueryable<Ticket> storageTickets =
                randomTickets;

            IQueryable<Ticket> expectedTickets = storageTickets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTicketsAsync())
                    .Returns(storageTickets);

            // when
            IQueryable<Ticket> actualTickets =
                this.ticketService.RetrieveAllTicketsAsync();

            // then
            actualTickets.Should().BeEquivalentTo(expectedTickets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTicketsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
