using SupportDesk.Application.Dtos;

namespace SupportDesk.Application.Interfaces.Services;

public interface ISupportTicketService
{
    Task<SupportTicketResponse> CreateAsync(CreateSupportTicketRequest request, CancellationToken cancellationToken = default);
    Task<List<SupportTicketResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SupportTicketResponse?> UpdateStatusAsync(Guid id, UpdateTicketStatusRequest request, CancellationToken cancellationToken = default);
}