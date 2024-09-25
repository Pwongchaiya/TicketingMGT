using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldModifyTicketAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            int randomDaysInThePast = GetRandomNegativeNumber();
            int randomMinutes = GetRandomNegativeNumber();

            Ticket randomTicket = CreateRandomTicket(randomDateTime);

            randomTicket.CreatedDate = randomTicket.CreatedDate.AddDays(randomDaysInThePast);

            Ticket inputTicket = randomTicket;

            inputTicket.UpdatedDate = inputTicket.UpdatedDate.AddMinutes(randomMinutes);

            Ticket beforeUpdateTicket = inputTicket.DeepClone();
            inputTicket.UpdatedDate = randomDateTime;
            Ticket afterUpdateStorageTicket = inputTicket;
            Ticket expectedTicket = afterUpdateStorageTicket.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(inputTicket.Id))
                    .ReturnsAsync(beforeUpdateTicket);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTicketAsync(inputTicket))
                    .ReturnsAsync(afterUpdateStorageTicket);

            // when
            Ticket actualTicket = await this.ticketService.ModifyTicketAsync(inputTicket);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(inputTicket.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTicketAsync(inputTicket),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
