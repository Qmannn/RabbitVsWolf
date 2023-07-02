using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitVsWolf.Events;

namespace RabbitVsWolf.MeatGeneration
{
    internal class RabbitMeatFactoryService : BackgroundService
    {
        private readonly ILogger<RabbitMeatFactoryService> _logger;
        private readonly IBus _bus;

        public RabbitMeatFactoryService(ILogger<RabbitMeatFactoryService> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int countOfMeatToGenerate = 1000;
            Random weightGen = new Random(42);

            _logger.LogInformation("Start of spliting rabbit Alex for meat parts");

            int totalRabbitWeight = 0;

            for (int i = 0; i < countOfMeatToGenerate; i++)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                int weight = i * weightGen.Next();
                totalRabbitWeight += weight;

                await _bus.Publish(new MeatEvent
                {
                    Identity = i,
                    Name = $"Alex. Part#{i}",
                    Weight = weight
                });

                // TODO: seems like need to rename factory!
                await _bus.Publish(new BonesEvent
                {
                    Id = i.ToString(),
                });
            }

            // TODO: if cancellation will be requested log may contains invalid count of Alex parts because here is used constant, not actual parts count
            _logger.LogInformation($"Alex was splitted for {countOfMeatToGenerate} parts. Total Alex weight {totalRabbitWeight} g.");
        }
    }
}
