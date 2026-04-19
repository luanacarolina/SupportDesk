using SupportDesk.Domain.Enums;

namespace SupportDesk.Application.Dtos;

public class SupportTicketsFilter
{
    public string? ClientName { get; set; }
    public PriorityLevel? Priority { get; set; }
    public TicketStatus? Status { get; set; }
}