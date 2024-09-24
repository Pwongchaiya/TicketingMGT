using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class TicketServiceException : Xeption
    {
        public TicketServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
