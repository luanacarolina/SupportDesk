using SupportDesk.Domain.Enums;

namespace SupportDesk.Application.Dtos;

public class CreateSupportTicketRequest
{
    public string ClientName { get; set; } = string.Empty;
    public string ProblemDescription { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
}