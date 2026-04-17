using SupportDesk.Domain.Enums;

namespace SupportDesk.Application.Dtos;

public class SupportTicketResponse
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ProblemDescription { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}