using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

namespace ModelContextProtocol.Server;

[McpServerToolType]
public sealed class SqlServerDatabaseTools
{
    private readonly SqlServerDatabase _database;

    public SqlServerDatabaseTools(SqlServerDatabase database)
    {
        _database = database;
    }

    [McpServerTool(Name = "sql_test_connection")]
    [Description("Test connection to the SQL Server database configured by the existing backend connection settings.")]
    public async Task<string> TestConnection(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_database.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT
                DB_NAME() AS DatabaseName,
                CAST(SERVERPROPERTY('ServerName') AS nvarchar(128)) AS ServerName,
                CAST(SERVERPROPERTY('ProductVersion') AS nvarchar(128)) AS SqlVersion";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);

        return JsonSerializer.Serialize(new
        {
            connected = true,
            database = reader[0]?.ToString(),
            server = reader[1]?.ToString(),
            sqlVersion = reader[2]?.ToString()
        });
    }

    [McpServerTool(Name = "sql_list_tables")]
    [Description("List tables and views in the connected SQL Server database.")]
    public async Task<string> ListTables(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_database.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_TYPE IN ('BASE TABLE', 'VIEW')
            ORDER BY TABLE_SCHEMA, TABLE_NAME";

        var rows = await ReadRowsAsync(command, cancellationToken);
        return JsonSerializer.Serialize(rows);
    }

    [McpServerTool(Name = "sql_list_columns")]
    [Description("List column metadata for a SQL Server table or view.")]
    public async Task<string> ListColumns(
        [Description("Schema name, for example dbo.")] string schema = "dbo",
        [Description("Table or view name.")] string table = "")
    {
        if (string.IsNullOrWhiteSpace(table))
            throw new ArgumentException("Table name is required.", nameof(table));

        await using var connection = new SqlConnection(_database.ConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION,
                   DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table
            ORDER BY ORDINAL_POSITION";
        command.Parameters.AddWithValue("@schema", schema);
        command.Parameters.AddWithValue("@table", table);

        var rows = await ReadRowsAsync(command, CancellationToken.None);
        return JsonSerializer.Serialize(rows);
    }

    [McpServerTool(Name = "sql_read_query")]
    [Description("Execute a read-only SELECT query. INSERT, UPDATE, DELETE, DDL and multiple statements are rejected.")]
    public async Task<string> ReadQuery(
        [Description("Read-only SQL query, starting with SELECT or WITH.")] string sql,
        [Description("Maximum rows to return.")] int maxRows = 100)
    {
        ValidateReadOnlyQuery(sql);
        maxRows = Math.Clamp(maxRows, 1, 500);

        await using var connection = new SqlConnection(_database.ConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        var rows = await ReadRowsAsync(command, CancellationToken.None, maxRows);
        return JsonSerializer.Serialize(rows);
    }

    private static void ValidateReadOnlyQuery(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
            throw new ArgumentException("SQL query is required.", nameof(sql));

        var normalized = sql.TrimStart().ToUpperInvariant();
        if (!normalized.StartsWith("SELECT", StringComparison.Ordinal) &&
            !normalized.StartsWith("WITH", StringComparison.Ordinal))
            throw new InvalidOperationException("Only SELECT or WITH read queries are allowed.");

        if (sql.Contains(';'))
            throw new InvalidOperationException("Multiple SQL statements are not allowed.");

        string[] forbidden =
        {
            " INSERT ", " UPDATE ", " DELETE ", " MERGE ", " DROP ", " ALTER ", " CREATE ",
            " TRUNCATE ", " EXEC ", " EXECUTE ", " SP_", " XP_", " INTO ", " OPENROWSET ",
            " OPENDATASOURCE ", " BULK ", " DBCC ", " GRANT ", " REVOKE "
        };

        var padded = " " + normalized + " ";
        if (forbidden.Any(token => padded.Contains(token, StringComparison.Ordinal)))
            throw new InvalidOperationException("Write or administrative SQL operations are not allowed.");
    }

    private static async Task<List<Dictionary<string, object?>>> ReadRowsAsync(
        SqlCommand command,
        CancellationToken cancellationToken,
        int? maxRows = null)
    {
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var rows = new List<Dictionary<string, object?>>();

        while (await reader.ReadAsync(cancellationToken))
        {
            if (maxRows.HasValue && rows.Count >= maxRows.Value)
                break;

            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            for (var index = 0; index < reader.FieldCount; index++)
            {
                var value = reader.GetValue(index);
                row[reader.GetName(index)] = value is DBNull ? null : value;
            }
            rows.Add(row);
        }

        return rows;
    }
}
