using PlatformService.Models.DTOs;
using PlatformService.Models.Interfaces;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataService{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusClient> _logger;

        public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

        }
        public void PublishNewPlatform(PlatformPublishDTO platform)
        {
            try
            {
                
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(PublishNewPlatform));
                throw;
            }
        }
    }
}