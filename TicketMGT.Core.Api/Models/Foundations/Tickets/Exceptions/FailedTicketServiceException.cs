using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class FailedTicketServiceException : Xeption
    {
        public FailedTicketServiceException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}
