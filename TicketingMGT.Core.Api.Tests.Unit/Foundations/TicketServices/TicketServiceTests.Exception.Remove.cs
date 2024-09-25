using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using FluentAssertions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRemoveAsyncIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(
                    message: "Failed ticket storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(
                    message: "Ticket dependency error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Ticket> removeTicketTask =
                this.ticketService.RemoveTicketByIdAsync(someId);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    removeTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);


            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnRemoveIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedTicketException =
                new LockedTicketException(
                    message: "Locked ticket record exception, please try again later.",
                    innerException: databaseUpdateConcurrencyException);

            var expectedTicketDependencyValidationException =
                new TicketDependencyValidationException(
                    message: "Ticket dependency validation error occurred, fix errors and try again.",
                    innerException: lockedTicketException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Ticket> removeTicketTask =
                this.ticketService.RemoveTicketByIdAsync(someId);

            TicketDependencyValidationException actualTicketDependencyValidationException =
                await Assert.ThrowsAsync<TicketDependencyValidationException>(
                    removeTicketTask.AsTask);

            // then
            actualTicketDependencyValidationException.Should().BeEquivalentTo(
                expectedTicketDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnRemoveIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedTicketStorageException =
                new FailedTicketStorageException(
                    message: "Failed ticket storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedTicketDependencyException =
                new TicketDependencyException(
                    message: "Ticket dependency error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Ticket> removeTicketTask =
                this.ticketService.RemoveTicketByIdAsync(someId);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    removeTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTicketStorageException =
                new FailedTicketServiceException(
                    message: "Failed ticket service error occurred, contact support.",
                    innerException: serviceException);

            var expectedTicketServiceException =
                new TicketServiceException(
                    message: "Ticket service error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Ticket> removeTicketTask =
                this.ticketService.RemoveTicketByIdAsync(someId);

            TicketServiceException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketServiceException>(
                    removeTicketTask.AsTask);

            // then
            actualTicketDependencyException.Should().BeEquivalentTo(
                expectedTicketServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTicketByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
