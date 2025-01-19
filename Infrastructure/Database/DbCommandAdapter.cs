using MySqlConnector;
using System.Data;
using DomainCommand = Domain.Common.Interfaces.IDbCommand;

namespace Infrastructure.Database;

internal class DbCommandAdapter : DomainCommand
{
    private readonly MySqlCommand command;

    public DbCommandAdapter(MySqlCommand command)
    {
        this.command = command ?? throw new ArgumentNullException(nameof(command));
    }

    public void AddParameter(string name, object value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }

    public int ExecuteNonQuery()
    {
        return command.ExecuteNonQuery();
    }

    public IDataReader ExecuteReader()
    {
        return command.ExecuteReader();
    }

    public void Dispose()
    {
        command.Dispose();
    }

    public IDbTransaction Transaction
    {
        get => command.Transaction;
        set => command.Transaction = value as MySqlTransaction;
    }
}