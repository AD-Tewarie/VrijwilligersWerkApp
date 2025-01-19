using System.Data;

namespace Domain.Common.Interfaces;

public interface IDbCommand : IDisposable
{
    void AddParameter(string name, object value);
    int ExecuteNonQuery();
    IDataReader ExecuteReader();
    IDbTransaction Transaction { get; set; }
}