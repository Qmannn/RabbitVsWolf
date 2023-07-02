using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitVsWolf.Events;
using System.Threading;

namespace RabbitVsWolf.Consumers
{
    internal class WolfConsumer : IConsumer<MeatEvent>
    {
        private readonly ILogger<WolfConsumer> _logger;

        private static int _countOfProcessedMessages;

        public WolfConsumer(ILogger<WolfConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MeatEvent> context)
        {
            Interlocked.Increment(ref _countOfProcessedMessages);

            double waitForSecounds = 5;

            _logger.LogInformation($"[{_countOfProcessedMessages}] Wolf eats rabbit meat {context.Message.Identity}. Time to eating {waitForSecounds} s.");

            // This approach make blocking of thread. New thread per each event. Too slow!
            // Thread.Sleep(TimeSpan.FromSeconds(waitForSecounds));

            // This way is fast. Thread returns to the ThreadPool and reused by another consumers
            await Task.Delay(TimeSpan.FromSeconds(waitForSecounds));

            _logger.LogInformation($"Wolf finish eats rabbit meat {context.Message.Identity}, {context.Message.Name}");
        }
    }
}
