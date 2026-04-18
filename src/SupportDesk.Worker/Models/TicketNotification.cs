namespace SupportDesk.Worker.Models;

public class TicketNotification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TicketId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string ProblemDescription { get; set; } = string.Empty;
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime CompletedAt { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}