using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory,
            IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = this.GetEventType(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    this.AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private async Task AddPlatform(string message)
        {
            using (var scope = this.serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                try
                {
                    var platform = this.mapper.Map<Platform>(platformPublishedDto);
                    if (!await repo.ExternalPlatformExist(platform.ExternalID))
                    {
                        repo.CreatePlatform(platform);
                        await repo.SaveChangesAsync();
                        Console.WriteLine("--> Platform added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB: {ex.Message}");
                }
            }
        }

        private EventType GetEventType(string notificationMessage)
        {
            var eventData = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            Console.WriteLine($"--> Determining Event Type for: {eventData.Event}");
            return eventData.Event switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }
    }
    public enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
