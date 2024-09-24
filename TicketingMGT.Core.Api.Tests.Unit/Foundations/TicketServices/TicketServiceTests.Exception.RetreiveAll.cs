using FluentAssertions;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllTicketsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = CreateSqlException();

            var failedTicketStorageException =
                new FailedTicketStorageException(
                    message: "Failed ticket storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedTicketDependencyException =
                new TicketDependencyException(
                    message: "Ticket dependency error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTicketsAsync())
                    .Throws(sqlException);

            // when
            Action retrieveAllTicketsAction = () =>
                this.ticketService.RetrieveAllTicketsAsync();

            TicketDependencyException actualTicketDependencyException =
                Assert.Throws<TicketDependencyException>(
                    retrieveAllTicketsAction);

            // then
            actualTicketDependencyException.Should()
                .BeEquivalentTo(expectedTicketDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTicketDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTicketsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllTicketsIfServiceErrorOccursAndLogItAsync()
        {
            var exception = new Exception();

            var failedTicketStorageException =
                new FailedTicketServiceException(
                    message: "Failed ticket service error occurred, contact support.",
                    innerException: exception);

            var expectedTicketServiceException =
                new TicketServiceException(
                    message: "Ticket service error occurred, contact support.",
                    innerException: failedTicketStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTicketsAsync())
                    .Throws(exception);

            // when
            Action retrieveAllTicketsAction = () =>
                this.ticketService.RetrieveAllTicketsAsync();

            TicketServiceException actualTicketServiceException =
                Assert.Throws<TicketServiceException>(
                    retrieveAllTicketsAction);

            // then
            actualTicketServiceException.Should()
                .BeEquivalentTo(expectedTicketServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTicketServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTicketsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
