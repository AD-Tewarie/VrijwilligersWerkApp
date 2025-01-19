﻿using Domain.Common.Interfaces;
using Infrastructure.Database;
using MySqlConnector;
using System.Data;
using DomainCommand = Domain.Common.Interfaces.IDbCommand;

namespace Infrastructure.Repos_DB;

public class DatabaseService : IDatabaseService
{
    private readonly string connectionString;

    public DatabaseService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    public DomainCommand CreateCommand(IDbConnection connection, string commandText)
    {
        var mysqlCommand = new MySqlCommand(commandText, connection as MySqlConnection);
        return new DbCommandAdapter(mysqlCommand);
    }

    public void OpenConnection(IDbConnection connection)
    {
        if (connection?.State != ConnectionState.Open)
        {
            connection?.Open();
        }
    }

    public void CloseConnection(IDbConnection connection)
    {
        if (connection?.State != ConnectionState.Closed)
        {
            connection?.Close();
        }
    }

    public IDbTransaction BeginTransaction(IDbConnection connection)
    {
        if (connection?.State != ConnectionState.Open)
        {
            throw new InvalidOperationException("Connection must be open before starting a transaction.");
        }
        return connection.BeginTransaction();
    }
}
