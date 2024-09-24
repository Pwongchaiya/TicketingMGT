﻿using FluentAssertions;
using Force.DeepCloner;
using Moq;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveTicketByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;

            Ticket randomTicket = CreateRandomTicket();

            Ticket storageTicket = randomTicket;
            Ticket expectedTicket = storageTicket.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.GetTicketByIdAsync(inputId))
                    .ReturnsAsync(storageTicket);

            // when
            Ticket actualTicket = await this.ticketService
                .GetTicketByIdAsync(inputId);

            // then
            actualTicket.Should().BeEquivalentTo(expectedTicket);

            this.storageBrokerMock.Verify(broker =>
                broker.GetTicketByIdAsync(inputId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
