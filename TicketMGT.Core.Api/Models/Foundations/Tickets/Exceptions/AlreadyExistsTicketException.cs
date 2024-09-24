using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class AlreadyExistsTicketException : Xeption
    {
        public AlreadyExistsTicketException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
