using Confluent.Kafka;
namespace KafkaImplementation.kafkaservice;

public class ConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
    {
        _logger = logger;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = "PrincesGroup",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("princesAdded");

        while (!stoppingToken.IsCancellationRequested)
        {
            // ProcessKafkaMessage might involve I/O and thus should be awaited
            await Task.Run(() => ProcessKafkaMessage(stoppingToken), stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }

        _consumer.Close();
    }

    private void ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            if (consumeResult != null)
            {
                var message = consumeResult.Message.Value;
                _logger.LogInformation($"Received Princes update: {message}");
            }
        }
        catch (OperationCanceledException)
        {
            // Expected during shutdown
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
        }
    }
}
