using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldRemoveTicketAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;

            Ticket randomTicket = CreateRandomTicket();

            Ticket storageTicket = randomTicket;
            Ticket removedTicket = storageTicket;
            Ticket expectedTicket = removedTicket.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(inputId))
                    .ReturnsAsync(storageTicket);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTicketAsync(storageTicket))
                    .ReturnsAsync(removedTicket);

            // when 
            Ticket actualTicket =
                await this.ticketService.RemoveTicketByIdAsync(inputId);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(inputId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTicketAsync(storageTicket),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
