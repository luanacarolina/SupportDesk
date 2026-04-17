using Microsoft.EntityFrameworkCore;
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

    public async Task<List<SupportTicket>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
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