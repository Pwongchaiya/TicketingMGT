using System.Threading.Tasks;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;
using Xeptions;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public partial class TicketService
    {
        private delegate ValueTask<Ticket> ReturningTicketFunction();

        private async ValueTask<Ticket> TryCatch(
            ReturningTicketFunction returningTicketFunction)
        {
            try
            {
                return await returningTicketFunction();
            }
            catch (NullTicketException nullTicketException)
            {
                throw CreateAndLogValidationException(nullTicketException);
            }
            catch (InvalidTicketException invalidTicketException)
            {
                throw CreateAndLogValidationException(invalidTicketException);
            }
        }

        private TicketValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var ticketValidationException =
                new TicketValidationException(
                    message: "Ticket validation error occurred. Fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ticketValidationException);

            return ticketValidationException;
        }
    }
}
