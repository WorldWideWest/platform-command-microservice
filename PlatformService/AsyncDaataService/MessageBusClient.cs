using System.Text;
using System.Text.Json;
using PlatformService.Models.DTOs;
using PlatformService.Models.Interfaces;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataService{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbtiMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _logger.LogInformation("Connected to messagebus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(MessageBusClient));
                throw;
            }

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"Connection shutdown: { e }");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            _logger.LogInformation("Message Sent!");
        }


        public void Dispose()
        {
            _logger.LogInformation("Message Bus Disposed!");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public void PublishNewPlatform(PlatformPublishDTO platform)
        {
            try
            {
                var message = JsonSerializer.Serialize(platform);
                if(_connection.IsOpen)
                {
                    _logger.LogInformation("Sending message");
                    SendMessage(message);
                }
                else
                {
                    _logger.LogInformation("Connection closed");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(PublishNewPlatform));
                throw;
            }
        }
    }
}