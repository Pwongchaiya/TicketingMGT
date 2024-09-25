using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using FluentAssertions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnRemoveIfTicketInvalidAndLogItAsync()
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
            ValueTask<Ticket> removeTicketTask =
               this.ticketService.RemoveTicketByIdAsync(invalidId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    removeTicketTask.AsTask);

            // then
            actualTicketValidationException.Should().BeEquivalentTo(
                expectedTicketValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTicketAsync(
                    It.IsAny<Ticket>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnRemoveIfTicketDoesNotExistAndLogItAsync()
        {
            // given
            Guid nonExistentId = Guid.NewGuid();
            Ticket nonExistentTicket = null;

            var notFoundTicketException =
                new NotFoundTicketException(
                    ticketId: nonExistentId,
                    message: $"Could not find ticket with id: {nonExistentId}");

            var expectedTicketValidationException = new TicketValidationException(
                message: "Ticket validation error occurred. Fix the errors and try again.",
                innerException: notFoundTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(nonExistentTicket);

            // when
            ValueTask<Ticket> removeTicketTask =
               this.ticketService.RemoveTicketByIdAsync(nonExistentId);

            TicketValidationException actualTicketValidationException =
                await Assert.ThrowsAsync<TicketValidationException>(
                    removeTicketTask.AsTask);

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

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTicketAsync(
                    It.IsAny<Ticket>()),
                        Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
