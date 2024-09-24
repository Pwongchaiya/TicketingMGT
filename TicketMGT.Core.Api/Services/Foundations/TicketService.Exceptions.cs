﻿using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (SqlException sqlException)
            {
                var failedTicketCodeStorageException =
                    new FailedTicketStorageException(
                        message: "Failed ticket storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTicketCodeStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var duplicateTicketException =
                    new AlreadyExistsTicketException(
                        message: "Ticket with the same id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(duplicateTicketException);
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

        private TicketDependencyException CreateAndLogCriticalDependencyException(
        Xeption exception)
        {
            var ticketCodeDependencyException = new TicketDependencyException(
                message: "Ticket dependency error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogCritical(ticketCodeDependencyException);

            return ticketCodeDependencyException;
        }

        private TicketDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var ticketDependencyValidationException =
                new TicketDependencyValidationException(
                    message: "Ticket dependency validation error occurred, fix errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ticketDependencyValidationException);

            return ticketDependencyValidationException;
        }
    }
}
