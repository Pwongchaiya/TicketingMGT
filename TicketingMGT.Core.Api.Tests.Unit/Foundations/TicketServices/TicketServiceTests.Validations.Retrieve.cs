using FluentAssertions;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidTicketException = new InvalidTicketException(
                message: "Invalid Ticket. Please correct the errors and try again");

            invalidTicketException.AddData(
                key: nameof(Ticket.Id),
                values: "Id is required");

            var expectedTicketValidationException = new TicketValidationException(
                message: "Ticket validation error occurred. Fix the errors and try again.",
                innerException: invalidTicketException);

            // when
            ValueTask<Ticket> ticketTask =
                this.ticketService.RetrieveTicketByIdAsync(invalidId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    ticketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnRetrieveByIdIfTicketNotFoundAndLogItAsync()
        {
            Guid someId = Guid.NewGuid();
            Ticket nonExistentTicket = null;

            var notFoundTicketValidationException =
                new NotFoundTicketException(
                    ticketId: someId,
                    message: $"Could not find ticket with id: {someId}");

            var expectedTicketValidationException = new TicketValidationException(
               message: "Ticket validation error occurred. Fix the errors and try again.",
               innerException: notFoundTicketValidationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(nonExistentTicket);

            // when
            ValueTask<Ticket> ticketTask =
                this.ticketService.RetrieveTicketByIdAsync(someId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    ticketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
