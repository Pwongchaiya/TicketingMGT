using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnAddIfTicketIsInvalidAndLogItAsync()
        {
            // given
            var invalidTicket = new Ticket();

            var invalidTicketException = new InvalidTicketException(
                message: "Invalid Ticket. Please correct the errors and try again");

            invalidTicketException.AddData(
                key: nameof(Ticket.Title),
                values: "Title is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Description),
                values: "Description is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Status),
                values: "Status is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.CreatedDate),
                values: "Date is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.UpdatedDate),
                values: "Date is required");

            var expectedTicketValidationException = new TicketValidationException(
                message: "Ticket validation error occurred. Fix the errors and try again.",
                innerException: invalidTicketException);

            // when
            ValueTask<Ticket> ticketTask =
                this.ticketService.CreateTicketAsync(invalidTicket);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    ticketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.(
                    It.IsAny<Ticket>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
