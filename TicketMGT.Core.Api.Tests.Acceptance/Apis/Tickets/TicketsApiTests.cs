using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;

namespace TicketMGT.Core.Api.Tests.Acceptance.Apis.Tickets
{
    public partial class TicketsApiTests
    {
        private readonly TicketApiBroker ticketApiBroker;

        public TicketsApiTests(TicketApiBroker ticketApiBroker) =>
            this.ticketApiBroker = ticketApiBroker;

        private async Task<List<Ticket>> CreateRandomTickets()
        {
            int randomNumber = GetRandomNumber();
            var randomTickets = new List<Ticket>();

            for (int i = 0; i < randomNumber; i++)
            {
                Ticket randomPostedTicket =
                    await PostRandomTicketAsync();

                randomTickets.Add(randomPostedTicket);
            }

            return randomTickets;
        }

        private async ValueTask<Ticket> PostRandomTicketAsync()
        {
            Ticket randomTicket = CreateRandomTicket();
            await this.ticketApiBroker.PostTicketAsync(randomTicket);

            return randomTicket;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Ticket CreateRandomTicket() =>
            CreateTicketFiller().Create();

        private static Filler<Ticket> CreateTicketFiller()
        {
            var filler = new Filler<Ticket>();
            int randomDays = GetRandomNumber();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset dueDate = now.AddDays(randomDays);

            filler.Setup()
                .OnProperty(ticket => ticket.DueDate).Use(dueDate)
                .OnProperty(ticket => ticket.CompletedAt).Use(dueDate)
                .OnProperty(ticket => ticket.ReminderDate).Use(dueDate)
                .OnProperty(ticket => ticket.Status).Use(TicketStatus.New)
                .OnType<DateTimeOffset>().Use(now);

            return filler;
        }
    }
}
