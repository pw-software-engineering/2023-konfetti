namespace TicketManager.Core.Services.Configuration;

public record BlobStorageConfiguration(string ConnectionString, string ContainerName);
