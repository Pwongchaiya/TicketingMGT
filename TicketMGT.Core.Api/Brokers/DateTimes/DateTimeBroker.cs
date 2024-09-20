using System;
using TicketMGT.Core.Api.Brokers.DateTimes;

namespace TicketMGT.Core.Api.Brokers.DateTimeBrokers
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTimeOffset GetCurrentDateTimeOffset() =>
            DateTimeOffset.UtcNow;
    }
}
