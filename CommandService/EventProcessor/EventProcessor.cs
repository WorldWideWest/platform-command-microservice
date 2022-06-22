using System.Text.Json;
using AutoMapper;
using CommandService.Models.DTOs;
using CommandService.Models.Entities;
using CommandService.Models.Interfaces;

namespace CommandService.EventProcessor{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }
        public void ProcessEvent(string message)
        {
            try
            {
                var eventType = DetermineEvent(message);
                switch(eventType){
                    case EventType.PlatformPublished:
                        AddPlatform(message);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ProcessEvent));
                throw;
            }
        }

        private void AddPlatform(string message)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var publishResponse = JsonSerializer.Deserialize<PlatformPublishDTO>(message);
                Console.WriteLine($"-->: {publishResponse.Event} {publishResponse.Id}");
                try
                {
                    var platform = _mapper.Map<Platform>(publishResponse);
                    Console.WriteLine($"_--Y>{platform.Id} {platform.Name} {platform.ExternalId}");
                    if(!repository.ExternalPlatformExists(platform.ExternalId))
                    {
                        repository.CreatePlatform(platform);
                        repository.SaveChanges(); 
                    }
                    else
                    {
                        _logger.LogError("Platform Exists");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(AddPlatform));
                    throw;
                }
            }
        }

        private EventType DetermineEvent(string notification)
        {
            _logger.LogInformation($"Determining Event: { notification }");
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notification);
            switch(eventType.Event){
                case "PlatformPublished":
                    _logger.LogInformation($"Event Selected: PlatformPublished");
                    return EventType.PlatformPublished;
                default:
                    _logger.LogInformation($"Event Selected: Undetermined");
                        return EventType.Undetermined;
            }


        }
    }

    enum EventType{
        PlatformPublished,
        Undetermined
    }
}