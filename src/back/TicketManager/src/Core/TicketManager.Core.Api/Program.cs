using Microsoft.EntityFrameworkCore;
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
        
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
