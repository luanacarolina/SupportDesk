using Microsoft.EntityFrameworkCore;
using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Repositories;
using SupportDesk.Domain.Entities;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repositories;

public class SupportTicketRepository(AppDbContext context) : ISupportTicketRepository
{
    private readonly AppDbContext _context = context;

    public async Task<SupportTicket> AddAsync(SupportTicket supportTicket, CancellationToken cancellationToken = default)
    {
        await _context.SupportTickets.AddAsync(supportTicket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return supportTicket;
    }

    public async Task<List<SupportTicket>> GetAllAsync(SupportTicketsFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.SupportTickets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.ClientName))
        {
            var clientName = filter.ClientName.Trim().ToLower();
            query = query.Where(x => x.ClientName.ToLower().Contains(clientName));
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(x => x.Priority == filter.Priority.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SupportTicket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(SupportTicket supportTicket, CancellationToken cancellationToken = default)
    {
        _context.SupportTickets.Update(supportTicket);
        await _context.SaveChangesAsync(cancellationToken);
    }
}