using System.Data;
using DomainCommand = Domain.Common.Interfaces.IDbCommand;

namespace Domain.Common.Interfaces;

public interface IDatabaseService
{
    IDbConnection GetConnection();
    void OpenConnection(IDbConnection connection);
    void CloseConnection(IDbConnection connection);
    DomainCommand CreateCommand(IDbConnection connection, string commandText);
    IDbTransaction BeginTransaction(IDbConnection connection);
}