using Domain.Common.Data;
using Domain.Common.Interfaces.Repository;
using Domain.Gebruikers.Models;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repos_DB
{
    public class UserRepositoryDB : IUserRepository
    {
        private readonly string connString;
        private MySqlConnection connection = null;
        private readonly IWachtwoordStrategy wachtwoordStrategy;

        public UserRepositoryDB(
            DBSettings settings,
            IWachtwoordStrategy wachtwoordStrategy)
        {
            connString = settings.DefaultConnection;
            this.wachtwoordStrategy = wachtwoordStrategy;
        }

        private bool IsConnect(string connString)
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            return true;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM user";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var userData = new UserData(
                                    reader.GetInt32("id"),
                                    reader.GetString("first_name"),
                                    reader.GetString("last_name"),
                                    reader.GetString("email"),
                                    reader.GetString("password"),
                                    reader.GetString("salt")
                                );
                                users.Add(User.LaadVanuitData(userData, wachtwoordStrategy));
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen gebruikers: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kon gebruikers niet ophalen", ex);
                    }
                    finally
                    {
                        if (connection != null)
                        {
                            connection.Close();
                            connection = null;
                        }
                    }
                }
            }
            return users;
        }

        public User GetUserByEmail(string email)
        {
            if (!IsConnect(connString))
                return null;

            string query = "SELECT * FROM user WHERE email = @email";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var userData = new UserData(
                                reader.GetInt32("id"),
                                reader.GetString("first_name"),
                                reader.GetString("last_name"),
                                reader.GetString("email"),
                                reader.GetString("password"),
                                reader.GetString("salt")
                            );
                            return User.LaadVanuitData(userData, wachtwoordStrategy);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen gebruiker: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon gebruiker niet ophalen", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return null;
        }

        public void AddUser(User user)
        {
            if (!IsConnect(connString))
                return;

            string query = @"INSERT INTO user(first_name, last_name, email, salt, password)
                           VALUES (@naam, @achternaam, @email, @salt, @passwordHash)";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    var userData = user.NaarData();
                    cmd.Parameters.AddWithValue("@naam", userData.Naam);
                    cmd.Parameters.AddWithValue("@achternaam", userData.AchterNaam);
                    cmd.Parameters.AddWithValue("@email", userData.Email);
                    cmd.Parameters.AddWithValue("@salt", userData.Salt);
                    cmd.Parameters.AddWithValue("@passwordHash", userData.PasswordHash);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij toevoegen gebruiker: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon gebruiker niet toevoegen", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public User GetUserOnId(int id)
        {
            if (!IsConnect(connString))
                return null;

            string query = "SELECT * FROM user WHERE id = @id";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var userData = new UserData(
                                reader.GetInt32("id"),
                                reader.GetString("first_name"),
                                reader.GetString("last_name"),
                                reader.GetString("email"),
                                reader.GetString("password"),
                                reader.GetString("salt")
                            );
                            return User.LaadVanuitData(userData, wachtwoordStrategy);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen gebruiker {id}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon gebruiker {id} niet ophalen", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return null;
        }

        public bool VerwijderUser(int userId)
        {
            if (!IsConnect(connString))
                return false;

            string query = "DELETE FROM user WHERE id = @id";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij verwijderen gebruiker {userId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon gebruiker {userId} niet verwijderen", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }
    }
}

