using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class TicketDependencyException : Xeption
    {
        public TicketDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
