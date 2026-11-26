using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ModelContextProtocol.Server;

public sealed class SqlServerDatabase
{
    public SqlServerDatabase()
    {
        ConnectionString = ResolveConnectionString();
    }

    public string ConnectionString { get; }

    public async Task TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT DB_NAME(), @@SERVERNAME, @@VERSION";
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
    }

    private static string ResolveConnectionString()
    {
        var fromEnvironment = Environment.GetEnvironmentVariable("MCP_SQL_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(fromEnvironment))
            return fromEnvironment;

        var configuredAppSettings = Environment.GetEnvironmentVariable("MCP_APPSETTINGS_PATH");
        var candidates = new List<string>();

        if (!string.IsNullOrWhiteSpace(configuredAppSettings))
            candidates.Add(configuredAppSettings);

        candidates.Add(Path.Combine(AppContext.BaseDirectory, "appsettings.mcp.json"));

        for (var directory = new DirectoryInfo(Directory.GetCurrentDirectory()); directory != null; directory = directory.Parent)
        {
            candidates.Add(Path.Combine(directory.FullName, "appsettings.mcp.json"));
            candidates.Add(Path.Combine(directory.FullName, "WebAPI", "appsettings.json"));
            candidates.Add(Path.Combine(directory.FullName, "hobby-shop-backend", "WebAPI", "appsettings.json"));
        }

        foreach (var candidate in candidates)
        {
            if (!File.Exists(candidate))
                continue;

            using var document = JsonDocument.Parse(File.ReadAllText(candidate));
            if (document.RootElement.TryGetProperty("ConnectionStrings", out var connections) &&
                connections.TryGetProperty("DefaultConnection", out var connection))
            {
                var value = connection.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
        }

        throw new InvalidOperationException(
            "No SQL Server connection configured. Set MCP_SQL_CONNECTION_STRING or provide WebAPI/appsettings.json.");
    }
}
