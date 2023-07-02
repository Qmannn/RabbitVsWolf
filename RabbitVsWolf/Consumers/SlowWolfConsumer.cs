using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitVsWolf.Events;

namespace RabbitVsWolf.Consumers
{
    internal class SlowWolfConsumer : IConsumer<BonesEvent>
    {
        private readonly ILogger<SlowWolfConsumer> _logger;

        private static int _countOfProcessedMessages;

        public SlowWolfConsumer(ILogger<SlowWolfConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BonesEvent> context)
        {
            Interlocked.Increment(ref _countOfProcessedMessages);

            double waitForSecounds = 60;

            _logger.LogInformation($"[{_countOfProcessedMessages}] Slow wolf eats rabbit bones {context.Message.Id}. Time to eating {waitForSecounds} s.");

            await Task.Delay(TimeSpan.FromSeconds(waitForSecounds));

            _logger.LogInformation($"Slow wolf finish eats rabbit bones");
        }
    }
}
