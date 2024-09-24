using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketMGT.Core.Api.Models.Foundations.Tickets;

namespace TicketMGT.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        protected void ConfigureTicket(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(ticket => ticket.Id);

            builder.Property(ticket => ticket.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
