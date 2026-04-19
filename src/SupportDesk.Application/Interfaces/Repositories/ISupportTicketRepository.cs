using SupportDesk.Application.Dtos;
using SupportDesk.Domain.Entities;

namespace SupportDesk.Application.Interfaces.Repositories;

public interface ISupportTicketRepository
{
    Task<SupportTicket> AddAsync(SupportTicket supportTicket, CancellationToken cancellationToken = default);
    Task<List<SupportTicket>> GetAllAsync(SupportTicketsFilter filter, CancellationToken cancellationToken = default);
    Task<SupportTicket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(SupportTicket supportTicket, CancellationToken cancellationToken = default);
}