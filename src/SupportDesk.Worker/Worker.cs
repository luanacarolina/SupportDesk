using System.Text;
using System.Text.Json;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SupportDesk.Application.Messaging;
using SupportDesk.Worker.Models;

namespace SupportDesk.Worker;


public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private const string QueueName = "support-ticket-completed";
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);
        _logger.LogInformation("Fila {QueueName} declarada com sucesso.", QueueName);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<SupportTicketCompletedMessage>(json);

                if (message is null)
                {
                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    return;
                }

                var mongoClient = new MongoClient("mongodb://localhost:27017");
                var database = mongoClient.GetDatabase("supportdesk_logs");
                var collection = database.GetCollection<TicketNotification>("ticket_notifications");

                var notification = new TicketNotification
                {
                    TicketId = message.TicketId.ToString(),
                    ClientName = message.ClientName,
                    ProblemDescription = message.ProblemDescription,
                    Priority = message.Priority,
                    Status = message.Status,
                    CompletedAt = message.CompletedAt,
                    ProcessedAt = DateTime.UtcNow,
                    Message = $"Chamado {message.TicketId} do cliente {message.ClientName} foi finalizado."
                };

                await collection.InsertOneAsync(notification, cancellationToken: stoppingToken);
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);

                _logger.LogInformation("Mensagem processada para o chamado {TicketId}", message.TicketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem");
            }
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
        _logger.LogInformation("Consumer iniciado para a fila {QueueName}.", QueueName);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}