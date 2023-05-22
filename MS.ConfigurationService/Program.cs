using MassTransit;
using System.Reflection;

namespace MS.ConfigurationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddMassTransit(x =>
                    {
                        var entryAssembly = Assembly.GetExecutingAssembly();
                        x.AddConsumers(entryAssembly);

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("rabbitmq", "/", h => {
                                h.Username("rabbitmq");
                                h.Password("rabbitmq");
                            });
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    services.AddHostedService<Worker>();

                })
                .Build();

            host.Run();
        }
    }
}