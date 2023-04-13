using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Contracts.Validation;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.Configuration;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Extensions.JsonConverters;
using TicketManager.Core.Services.Extensions.Parsers;
using TicketManager.Core.Services.Services.HttpClients;
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
                .UseNpgsql(builder.Configuration["DatabaseConnectionString"])
        );

        var signingKey = builder.Configuration["SigningKey"]; 
        
        builder.Services.AddSingleton<PasswordManager>();
        builder.Services.AddSingleton<MockableCoreDbResolver>();
        builder.Services.AddSingleton<TokenCreator>();
        builder.Services.AddSingleton<TokenConfiguration>(new TokenConfiguration(signingKey));
        builder.Services.AddSingleton(new PaymentClientConfiguration(
            builder.Configuration["PaymentClientBaseUrl"],
            builder.Configuration["PaymentClientApiKey"]));

        builder.Services.AddHttpClient<PaymentClient>().ConfigureHttpClient(PaymentClient.Configure);
        
        builder.Services.AddScoped<Repository<User, Guid>>();
        builder.Services.AddScoped<Repository<Organizer, Guid>>();
        builder.Services.AddScoped<Repository<Account, Guid>>();
        builder.Services.AddScoped<Repository<Event, Guid>>();
        builder.Services.AddScoped<Repository<SectorReservation, Guid>>();
        
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
            c.Serializer.Options.Converters.Add(new DateTimeConverter());
            c.Binding.ValueParserFor<List<TaxIdTypeDto>>(DtoListParser<TaxIdTypeDto>.Parse);
            c.Binding.ValueParserFor<List<VerificationStatusDto>>(DtoListParser<VerificationStatusDto>.Parse);
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) => new ValidationErrorResponse
            {
                Errors = failures.Select(f =>
                    {
                        try
                        {
                            return new ValidationError
                            {
                                ErrorCode = int.Parse(f.ErrorCode),
                                ErrorMessage = f.ErrorMessage,
                            };
                        }
                        catch (Exception)
                        {
                            return new ValidationError
                            {
                                ErrorCode = -1,
                                ErrorMessage = "Unexpected error has happened",
                            };
                        }
                        
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
