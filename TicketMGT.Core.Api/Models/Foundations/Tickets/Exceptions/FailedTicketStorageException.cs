using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class FailedTicketStorageException : Xeption
    {
        public FailedTicketStorageException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
