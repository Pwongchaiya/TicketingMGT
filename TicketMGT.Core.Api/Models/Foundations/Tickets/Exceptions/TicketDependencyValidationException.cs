using System;
using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class TicketDependencyValidationException : Xeption
    {
        public TicketDependencyValidationException(string message, Exception innerException)
            : base(message, innerException) 
        { }
    }
}
