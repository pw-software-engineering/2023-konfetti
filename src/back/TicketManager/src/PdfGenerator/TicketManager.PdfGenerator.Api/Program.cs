using MassTransit;

namespace TicketManager.PdfGenerator.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(builder.Configuration["RabbitMqHost"], builder.Configuration["RabbitMqVirtualHost"], cfg =>
                {
                    cfg.Username(builder.Configuration["RabbitMqUsername"]);
                    cfg.Password(builder.Configuration["RabbitMqPassword"]);
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        
        var app = builder.Build();
        app.Run();
    }
}