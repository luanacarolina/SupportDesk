using SupportDesk.Domain.Enums;

namespace SupportDesk.Application.Dtos;

public class UpdateTicketStatusRequest
{
    public TicketStatus Status { get; set; }
}