using System.Threading.Tasks;
using System;

namespace TicketMGT.Core.Api.Models.Foundations.Tickets
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrencePattern? RecurrencePattern { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public DateTimeOffset? ReminderDate { get; set; }
        public long? EstimatedTimeToCompleteInHours { get; set; }
        public long? ActualTimeToCompleteInHours { get; set; }
    }
}
