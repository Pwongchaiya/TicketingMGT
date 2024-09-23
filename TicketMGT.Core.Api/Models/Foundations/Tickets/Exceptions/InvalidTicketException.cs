using Xeptions;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions
{
    public class InvalidTicketException : Xeption
    {
        public InvalidTicketException(string message)
            : base(message) 
        { }
    }
}
