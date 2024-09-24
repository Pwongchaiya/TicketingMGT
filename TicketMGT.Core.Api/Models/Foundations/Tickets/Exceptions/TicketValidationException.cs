using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class TicketValidationException : Xeption
    {
        public TicketValidationException(string message, Xeption innerException)
            : base(message, innerException) { }
    }
}
