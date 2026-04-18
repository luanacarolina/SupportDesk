namespace SupportDesk.Application.Messaging;

public class SupportTicketCompletedMessage
{
    public Guid TicketId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ProblemDescription { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime CompletedAt { get; set; }
}