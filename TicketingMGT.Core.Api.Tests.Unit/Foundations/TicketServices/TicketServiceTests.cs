using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Moq;
using TicketMGT.Core.Api.Brokers.DateTimes;
using TicketMGT.Core.Api.Brokers.Loggings;
using TicketMGT.Core.Api.Brokers.Storages;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Services.Foundations;
using Tynamix.ObjectFiller;
using Xeptions;

namespace TicketMGT.Core.Api.Tests.Unit.Foundations.TicketServices
{
    public partial class TicketServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly TicketService ticketService;
        public TicketServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            ticketService = new TicketService(
                storageBrokerMock.Object,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object);
        }

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static Ticket CreateRandomTicket(DateTimeOffset dates) =>
            CreateTicketFiller(dates).Create();

        private static Ticket CreateRandomTicket()
        {
            int randomDays = GetRandomNumber();
            DateTimeOffset dateTimeOffset = GetRandomDateTimeOffset();

            return CreateTicketFiller(dateTimeOffset).Create();
        }

        private static IQueryable<Ticket> CreateRandomTickets(DateTimeOffset dates) =>
            CreateTicketFiller(dates: dates)
                .Create(GetRandomNumber())
                    .AsQueryable();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private SqlException CreateSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 9).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateOnly GetRandomDateOnly(DateTimeOffset dates) =>
            new(dates.Year, dates.Month, dates.Day);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static long CalculateDateDifference(DateTimeOffset dateTimeOffset1, DateTimeOffset dateTimeOffset2)
        {
            return (dateTimeOffset1 - dateTimeOffset2).Ticks;
        }

        private static Filler<Ticket> CreateTicketFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Ticket>();
            int randomDays = GetRandomNumber();
            DateTimeOffset dueDate = dates.AddDays(randomDays);

            filler.Setup()
                .OnProperty(ticket => ticket.DueDate).Use(dueDate)
                .OnProperty(ticket => ticket.CompletedAt).Use(dueDate)
                .OnProperty(ticket => ticket.ReminderDate).Use(dueDate)
                .OnProperty(ticket => ticket.Status).Use(TicketStatus.New)
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
