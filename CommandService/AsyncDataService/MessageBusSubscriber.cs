using System.Text;
using CommandService.Models.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataService{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _processor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor processor, ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _processor = processor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
            
            _logger.LogInformation("Listening on the Message Bus");

            _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Connection terminated");
        }

        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                stoppingToken.ThrowIfCancellationRequested();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (ModuleHandle, ea) => {
                    _logger.LogInformation("Event Recieved");
                    var body = ea.Body;
                    var notification = Encoding.UTF8.GetString(body.ToArray());
                    
                    _processor.ProcessEvent(notification);
                };

                _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ExecuteAsync));
                throw;
            }
        }
    }
}