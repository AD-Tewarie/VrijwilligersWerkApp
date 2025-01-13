using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos_DB
{
    public class DatabaseService
    {
        private readonly string connString;
        private MySqlConnection connection;

        public DatabaseService(DBSettings settings)
        {
            connString = settings.DefaultConnection;
        }

        public MySqlConnection GetConnection()
        {
            if (connection == null || connection.State != ConnectionState.Open)
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            return connection;
        }

        public void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }
    }
}
