using Microsoft.EntityFrameworkCore;
using SupportDesk.Domain.Entities;

namespace SupportDesk.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.ToTable("support_tickets");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ClientName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.ProblemDescription)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Priority)
                .IsRequired();

            entity.Property(x => x.Status)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .IsRequired();
        });
    }
}