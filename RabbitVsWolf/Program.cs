using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitVsWolf.Consumers;
using RabbitVsWolf.MeatGeneration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitMeatFactoryService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<WolfConsumer>();
            x.AddConsumer<SlowWolfConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("rmuser");
                    h.Password("rmpassword");
                });

                cfg.ReceiveEndpoint("wolf_queue", x =>
                {
                    x.PrefetchCount = 200000;
                    x.ConcurrentMessageLimit = 1000000;

                    x.ConfigureConsumers(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();