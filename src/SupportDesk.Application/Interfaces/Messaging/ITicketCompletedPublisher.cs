using SupportDesk.Application.Messaging;

namespace SupportDesk.Application.Interfaces.Messaging;

public interface ITicketCompletedPublisher
{
    Task PublishAsync(SupportTicketCompletedMessage message, CancellationToken cancellationToken = default);
}