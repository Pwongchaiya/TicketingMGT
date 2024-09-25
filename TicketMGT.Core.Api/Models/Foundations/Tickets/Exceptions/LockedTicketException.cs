using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class LockedTicketException : Xeption
    {
        public LockedTicketException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
