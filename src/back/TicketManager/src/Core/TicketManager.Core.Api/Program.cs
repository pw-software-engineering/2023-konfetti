using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TicketManager.Core.Contracts.Validation;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.Configuration;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.JsonConverters;
using TicketManager.Core.Services.Services.Mockables;
using TicketManager.Core.Services.Services.PasswordManagers;
using TicketManager.Core.Services.Services.TokenManager;

namespace TicketManager.Core.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseNpgsql("Host=api-database;Username=admin;Password=password;Database=api") // TODO: move to env
        );

        // TODO: move to env
        var signingKey = "UWaNHq1sR+3HEYyrcqO1MLa4zgtR9mYHW/wRYNsBzKRlqBMUD8U3sLUS0+j2RsN2tfNV4rQhhxfcmNmDldk94EOtDiAxg8By6YUod0fXIgWGykeb7VYg5s/NzS1UTTe8Fj7ddB522HwR3iCz97sF3H2oUW0MFYtJr9eF61MG+ZHbaw4FWeqGwqc9W0is/Q4ceLzBR3ndS+gsT/5sdMVpAt+oVa0Z08WG0BCRJrFyJhcxOkC2UGGGQVxcGUHS/ICP5zgWcOp3/iDswC6MBkl3W1T4BFmGyrBhjArGWaCwo2ae0/Z0rvSkeERgF4+AMFNRIjAYEcERFUhG1kgwL1/vAw=="; // TODO: Move to variable
        
        builder.Services.AddSingleton<PasswordManager>();
        builder.Services.AddSingleton<MockableCoreDbResolver>();
        builder.Services.AddSingleton<TokenCreator>();
        builder.Services.AddSingleton<TokenConfiguration>(new TokenConfiguration(signingKey));
            
        builder.Services.AddScoped<Repository<User, Guid>>();
        builder.Services.AddScoped<Repository<Organizer, Guid>>();
        builder.Services.AddScoped<Repository<Account, Guid>>();
        builder.Services.AddScoped<Repository<Event, Guid>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        builder.Services.AddJWTBearerAuth(signingKey);
        
        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints(c =>
        {
            c.Serializer.Options.Converters.Add(new DateOnlyConverter());
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) => new ValidationErrorResponse
            {
                Errors = failures.Select(f => new ValidationError
                {
                    ErrorCode = int.Parse(f.ErrorCode),
                    ErrorMessage = f.ErrorMessage,
                })
                .ToList(),
            };
        });
        app.UseSwaggerGen();
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<CoreDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        app.Run();
    }
}
