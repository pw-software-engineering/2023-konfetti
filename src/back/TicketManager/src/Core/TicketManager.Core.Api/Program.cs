using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.JsonConverters;
using TicketManager.Core.Services.Services.PasswordManagers;

namespace TicketManager.Core.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseInMemoryDatabase("Database")
        );

        builder.Services.AddSingleton<PasswordManager>();

        builder.Services.AddScoped<Repository<User, Guid>>();
        builder.Services.AddScoped<Repository<Account, Guid>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc();
        
        var app = builder.Build();

        app.UseAuthorization();
        app.UseFastEndpoints(c =>
        {
            c.Serializer.Options.Converters.Add(new DateOnlyConverter());
        });
        app.UseSwaggerGen();

        app.Run();
    }
}
