using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class NullTicketException : Xeption
    {
        public NullTicketException(string message)
            : base(message)
        { }
    }
}
