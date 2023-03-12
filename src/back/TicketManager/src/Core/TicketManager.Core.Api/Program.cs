using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using NSwag;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts.UseInMemoryDatabase("Database")
        );

        builder.Services.AddScoped<Repository<User, Guid>>();
        builder.Services.AddScoped<Repository<Account, Guid>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc();
        
        var app = builder.Build();

        app.UseAuthorization();
        app.UseFastEndpoints();
        app.UseSwaggerGen();

        app.Run();
    }
}
