using FluentAssertions;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTicketIsNullAndLogItAsync()
        {
            // given
            Ticket nullTicket = null;

            var nullTicketException =
                new NullTicketException(
                    message: "Ticket is null.");

            var expectedTicketValidationException = new TicketValidationException(
                message: "Ticket validation error occurred. Fix the errors and try again.",
                innerException: nullTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.AddTicketAsync(
                    It.IsAny<Ticket>()))
                        .ReturnsAsync(nullTicket);

            // when
            ValueTask<Ticket> ticketTask =
                this.ticketService.CreateTicketAsync(nullTicket);

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
                broker.AddTicketAsync(
                    It.IsAny<Ticket>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnAddIfTicketIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidTicket = new Ticket()
            {
                Title = invalidText,
                Description = invalidText,
                Status = TicketStatus.InProgress
            };


            var invalidTicketException = new InvalidTicketException(
                message: "Invalid Ticket. Please correct the errors and try again");

            invalidTicketException.AddData(
                key: nameof(Ticket.Title),
                values: "Text is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Description),
                values: "Text is required");

            invalidTicketException.AddData(
                key: nameof(Ticket.Status),
                values: "Value is required");

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
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.AddTicketAsync(
                    It.IsAny<Ticket>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
