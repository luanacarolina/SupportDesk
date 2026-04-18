using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SupportDesk.Application.Interfaces.Messaging;
using SupportDesk.Application.Messaging;

namespace SupportDesk.Infrastructure.Messaging;

public class RabbitMqTicketCompletedPublisher : ITicketCompletedPublisher
{
    private const string QueueName = "support-ticket-completed";

    public async Task PublishAsync(SupportTicketCompletedMessage message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);

        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: QueueName,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}