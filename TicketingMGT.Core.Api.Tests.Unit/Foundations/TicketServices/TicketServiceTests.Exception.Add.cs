using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowTicketDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Ticket randomTicket = CreateRandomTicket();
            SqlException sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(
                    message: "Failed ticket storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(
                    message: "Ticket dependency error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Ticket> createTicket =
                this.ticketService.CreateTicketAsync(randomTicket);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    createTicket.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))),
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
