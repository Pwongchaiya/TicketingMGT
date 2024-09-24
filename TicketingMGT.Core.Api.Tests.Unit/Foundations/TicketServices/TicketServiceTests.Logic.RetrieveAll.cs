using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                broker.SelectAllTickets())
                    .Returns(storageTickets);

            // when
            IQueryable<Ticket> actualTickets =
                this.ticketService.RetrieveAllTickets();

            // then
            actualTickets.Should().BeEquivalentTo(expectedTickets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTickets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
