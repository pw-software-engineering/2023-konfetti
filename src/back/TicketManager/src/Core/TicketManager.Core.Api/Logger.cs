namespace TicketManager.Core.Api;

public static class Logger
{
    public static void UseLoggingMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetService<ILogger<Program>>();
            logger!.LogInformation($"Request: {context.Request.Path}");

            await next.Invoke();
        });
    }
}
