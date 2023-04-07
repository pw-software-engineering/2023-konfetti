FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ./src/Core/*.Api/*.csproj /restore/TicketManager.Core.Api/
COPY ./src/Core/*.Contracts/*.csproj /restore/TicketManager.Core.Contracts/
COPY ./src/Core/*.Domain/*.csproj /restore/TicketManager.Core.Domain/
COPY ./src/Core/*.Services/*.csproj /restore/TicketManager.Core.Services/
RUN dotnet restore "/restore/TicketManager.Core.Api/TicketManager.Core.Api.csproj"
WORKDIR /src
COPY ./src/. .
RUN cp -r /restore/* Core/
RUN dotnet publish "Core/TicketManager.Core.Api/TicketManager.Core.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TicketManager.Core.Api.dll"]