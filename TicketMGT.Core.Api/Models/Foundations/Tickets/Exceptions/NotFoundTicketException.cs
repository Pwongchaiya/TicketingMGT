using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class NotFoundTicketException : Xeption
    {
        public NotFoundTicketException(Guid ticketId, string message)
            : base(message)
        { }
    }
}
