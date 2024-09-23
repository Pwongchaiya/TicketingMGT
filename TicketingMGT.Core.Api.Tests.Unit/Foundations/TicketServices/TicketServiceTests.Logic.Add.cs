using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {

        [Fact]
        public async Task ShouldAddTicketAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Ticket randomTicket = CreateRandomTicket(randomDateTime);
            Ticket inputTicket = randomTicket;
            Ticket storageTicket = inputTicket;
            Ticket expectedTicket = storageTicket.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.AddTicketAsync(inputTicket))
                    .ReturnsAsync(storageTicket);

            // when
            Ticket actualTicket =
                await this.ticketService
                    .CreateTicketAsync(inputTicket);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.AddTicketAsync(inputTicket),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
