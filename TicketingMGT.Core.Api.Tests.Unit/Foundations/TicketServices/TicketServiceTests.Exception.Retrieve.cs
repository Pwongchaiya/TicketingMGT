using Microsoft.Data.SqlClient;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using FluentAssertions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdAsyncIfSqlErrorOccursAndLogItAsync()
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
            ValueTask<Ticket> retrieveTicketByIdTask =
                this.ticketService.RetrieveTicketByIdAsync(someId);

            TicketDependencyException actualTicketDependencyException =
                await Assert.ThrowsAsync<TicketDependencyException>(
                    retrieveTicketByIdTask.AsTask);

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
        private async Task ShouldThrowServiceExceptionOnRetrieveByIdIfExceptionOccursAndLogItAsync()
        {
            Guid someId = Guid.NewGuid();
            Exception serviceException = new Exception();

            var failedTicketStorageException =
                new FailedTicketServiceException(
                    message: "Failed ticket service error occurred, contact support.",
                    innerException: serviceException);

            var expectedTicketServiceException =
                new TicketServiceException(
                    message: "Ticket service error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTicketByIdAsync(
                    It.IsAny<Guid>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask<Ticket> retrieveTicketByIdTask =
                this.ticketService.RetrieveTicketByIdAsync(someId);

            TicketServiceException actualTicketServiceException =
                await Assert.ThrowsAsync<TicketServiceException>(
                    retrieveTicketByIdTask.AsTask);

            // then
            actualTicketServiceException.Should().BeEquivalentTo(
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
