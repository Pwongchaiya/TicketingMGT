using System;

namespace TicketMGT.Core.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
