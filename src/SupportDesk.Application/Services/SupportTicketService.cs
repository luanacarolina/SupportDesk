using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Repositories;
using SupportDesk.Application.Interfaces.Services;
using SupportDesk.Domain.Entities;
using SupportDesk.Domain.Enums;

namespace SupportDesk.Application.Services;

public class SupportTicketService(ISupportTicketRepository repository) : ISupportTicketService
{
    private readonly ISupportTicketRepository _repository = repository;

    public async Task<SupportTicketResponse> CreateAsync(CreateSupportTicketRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var entity = new SupportTicket
        {
            Id = Guid.NewGuid(),
            ClientName = request.ClientName,
            ProblemDescription = request.ProblemDescription,
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(entity, cancellationToken);

        return Map(entity);
    }

    public async Task<List<SupportTicketResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tickets = await _repository.GetAllAsync(cancellationToken);
        return [.. tickets.Select(Map)];
    }

    public async Task<SupportTicketResponse?> UpdateStatusAsync(Guid id, UpdateTicketStatusRequest request, CancellationToken cancellationToken = default)
    {
        ValidateStatusRequest(request);
        var ticket = await _repository.GetByIdAsync(id, cancellationToken);

        if (ticket is null)
            return null;

        ticket.Status = request.Status;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(ticket, cancellationToken);

        return Map(ticket);
    }
    private static void ValidateStatusRequest(UpdateTicketStatusRequest request)
    {
        if (!Enum.IsDefined(typeof(TicketStatus), request.Status))
            throw new ArgumentException("Status é inválido");
    }
    private static void ValidateCreateRequest(CreateSupportTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ClientName))
            throw new ArgumentException("ClientName é obrigatório");

        if (string.IsNullOrWhiteSpace(request.ProblemDescription))
            throw new ArgumentException("ProblemDescription é obrigatório");
    }

    private static SupportTicketResponse Map(SupportTicket entity)
    {
        return new SupportTicketResponse
        {
            Id = entity.Id,
            ClientName = entity.ClientName,
            ProblemDescription = entity.ProblemDescription,
            Priority = entity.Priority,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}