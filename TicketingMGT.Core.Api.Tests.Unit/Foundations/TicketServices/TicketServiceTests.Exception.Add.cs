using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfTicketAlreadyExistsAndLogItAsync()
        {
            // given
            Ticket randomTicket = CreateRandomTicket();
            string randomMessage = GetRandomMessage();
            var duplicateKeyException = new DuplicateKeyException(randomMessage);

            var alreadyExistsTicketException =
                new AlreadyExistsTicketException(
                    message: "Ticket with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedTicketDependencyValidationException =
                new TicketDependencyValidationException(
                    message: "Ticket dependency validation error occurred, fix errors and try again.",
                    innerException: alreadyExistsTicketException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Ticket> addTask =
                this.ticketService.CreateTicketAsync(randomTicket);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                await Assert.ThrowsAsync<TicketDependencyValidationException>(
                    addTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should().BeEquivalentTo(
                expectedTicketDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowTicketDependencyExceptionOnAddIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Ticket randomTicket = CreateRandomTicket();
            var dbUpdateException = new DbUpdateException();

            var failedTicketStorageException = new FailedTicketStorageException(
                message: "Failed ticket storage error occurred, contact support.",
                innerException: dbUpdateException);

            var expectedTicketDependencyException = new TicketDependencyException(
                message: "Ticket dependency error occurred, contact support.",
                innerException: failedTicketStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<Ticket> addTask =
                this.ticketService.CreateTicketAsync(randomTicket);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    addTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
    

}
