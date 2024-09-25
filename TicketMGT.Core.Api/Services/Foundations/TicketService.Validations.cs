using System;
using TicketMGT.Core.Api.Models.Foundations.Tickets;
using TicketMGT.Core.Api.Models.Foundations.Tickets.Exceptions;

namespace TicketMGT.Core.Api.Services.Foundations
{
    public partial class TicketService
    {
        public void ValidateTicketOnAdd(Ticket ticket)
        {
            ValidateTicketNotNull(ticket);

            Validate(
                (Rule: IsInvalid(ticket.Id), Parameter: nameof(Ticket.Id)),
                (Rule: IsInvalid(ticket.Title), Parameter: nameof(Ticket.Title)),
                (Rule: IsInvalid(ticket.Description), Parameter: nameof(Ticket.Description)),
                (Rule: IsInvalid(ticket.Status), Parameter: nameof(Ticket.Status)),
                (Rule: IsInvalid(ticket.CreatedDate), Parameter: nameof(Ticket.CreatedDate)),
                (Rule: IsInvalid(ticket.UpdatedDate), Parameter: nameof(Ticket.UpdatedDate)),

                (Rule: IsNotSame(
                        firstDate: ticket.UpdatedDate,
                        secondDate: ticket.CreatedDate,
                        secondDateName: nameof(Ticket.CreatedDate)),

                Parameter: nameof(Ticket.UpdatedDate)),

                (Rule: IsNotRecent(ticket.CreatedDate), Parameter: nameof(ticket.CreatedDate))
            );
        }

        private void ValidateTicketOnModify(Ticket ticket)
        {
            ValidateTicketNotNull(ticket);

            Validate(
                (Rule: IsInvalid(ticket.Id), Parameter: nameof(Ticket.Id)),
                (Rule: IsInvalid(ticket.CreatedDate), Parameter: nameof(Ticket.CreatedDate)),
                (Rule: IsInvalid(ticket.UpdatedDate), Parameter: nameof(Ticket.UpdatedDate)),

                (Rule: IsSame(
                        firstDate: ticket.UpdatedDate,
                        secondDate: ticket.CreatedDate,
                        secondDateName: nameof(Ticket.CreatedDate)),

                Parameter: nameof(Ticket.UpdatedDate)),

                (Rule: IsNotRecent(ticket.UpdatedDate), Parameter: nameof(Ticket.UpdatedDate))
            );
        }

        private static void ValidateStorageTicketOnModify(
            Ticket inputTicket,
            Ticket storageTicket)
        {
            ValidateTicketExists(storageTicket, inputTicket.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTicket.CreatedDate,
                    secondDate: storageTicket.CreatedDate,
                    secondDateName: nameof(Ticket.CreatedDate)),

                Parameter: nameof(Ticket.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputTicket.UpdatedDate,
                    secondDate: storageTicket.UpdatedDate,
                    secondDateName: nameof(Ticket.UpdatedDate)),

                Parameter: nameof(Ticket.UpdatedDate)),

                (Rule: IsUpdateDateEarlierThanStorage(
                    inputDate: inputTicket.UpdatedDate,
                    storageDate: storageTicket.UpdatedDate,
                    secondDateName: nameof(Ticket.UpdatedDate)),

                Parameter: nameof(Ticket.UpdatedDate))
            );
        }

        private static void ValidateTicketExists(Ticket ticket, Guid ticketId)
        {
            if (ticket is null)
            {
                throw new NotFoundTicketException(
                    ticketId: ticketId,
                    message: $"Could not find ticket with id: {ticketId}");
            }
        }

        private void ValidateTicketNotNull(Ticket ticket)
        {
            if (ticket is null)
            {
                throw new NullTicketException(
                    message: "Ticket is null.");
            }
        }

        private void ValidateId(Guid id)
        {
            Validate(
                (Rule: IsInvalid(id), Parameter: nameof(Ticket.Id)));
        }

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsSame(
            DateTimeOffset? firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = IsDateSame(firstDate, secondDate),
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(
            DateTimeOffset date) => new
            {
                Condition = IsDateNotRecent(date),
                Message = "Date is not recent"
            };

        private static dynamic IsUpdateDateEarlierThanStorage(
            DateTimeOffset inputDate,
            DateTimeOffset storageDate,
            string secondDateName) => new
            {
                Condition = IsDateIsBefore(inputDate, storageDate),
                Message = $"Date is earlier than storage {secondDateName}"
            };

        private static bool IsDateSame(
            DateTimeOffset? firstDate,
            DateTimeOffset secondDate)
        {
            return firstDate == secondDate;
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static bool IsDateIsBefore(
            DateTimeOffset inputDate,
            DateTimeOffset storageDate)
        {
            TimeSpan timeDifference = inputDate.Subtract(storageDate);

            return timeDifference.TotalSeconds is < 0;
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(TicketStatus state) => new
        {
            Condition = state != TicketStatus.New,
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTicketException = new InvalidTicketException(
                message: "Invalid Ticket. Please correct the errors and try again");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTicketException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTicketException.ThrowIfContainsErrors();
        }
    }
}