using Infrastructure.DTO;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos_DB
{
    public class UserRepositoryDB : IUserRepository
    {

        private readonly string connString;
        private MySqlConnection connection = null;



        public UserRepositoryDB(DBSettings settings)
        {
            connString = settings.DefaultConnection;
        }


        public bool IsConnect(string connString)
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            return true;
        }


        // ophalen


        public List<UserDTO> GetUsers()
        {
            List<UserDTO> userLijst = new List<UserDTO>();

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
                                int id = reader.GetInt32("id");
                                string voornaam = reader.GetString("first_name");
                                string achternaam = reader.GetString("last_name");
                                string email = reader.GetString("email");
                                string passwordHash = reader.GetString("password");
                                string salt = reader.GetString("salt");



                                UserDTO user = new UserDTO(id, voornaam, achternaam, email, passwordHash, salt);
                                userLijst.Add(user);
                            }
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen gebruikers: {ex.Message}" + Environment.NewLine);
                        throw new Exception($"Kon gebruikers niet ophalen", ex);
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
            return userLijst;

        }



        public UserDTO GetUserByEmail(string email)
        {

            UserDTO userDTO = null;

            if (IsConnect(connString))
            {
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

                                int id = reader.GetInt32("id");
                                string voorNaam = reader.GetString("first_name");
                                string achterNaam = reader.GetString("last_name");
                                string passwordHash = reader.GetString("password");
                                string salt = reader.GetString("salt");



                                userDTO = new UserDTO(id, voorNaam, achterNaam, email, passwordHash, salt);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen gebruiker: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon gebruiker niet ophalen", ex);
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

            return userDTO;
        }






        public UserDTO GetUserOnId(int id)
        {

            UserDTO userDTO = null;

            if (IsConnect(connString))
            {
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

                                string naam = reader.GetString("first_name");
                                string achterNaam = reader.GetString("last_name");
                                string email = reader.GetString("email");
                                string passwordHash = reader.GetString("password");
                                string salt = reader.GetString("salt");



                                userDTO = new UserDTO(id, naam, achterNaam, email, passwordHash, salt);
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
            }

            return userDTO;


        }


        // Voeg user toe


        public void AddUser(UserDTO userDTO)
        {

            UserDTO dTO = null;

            if (IsConnect(connString))
            {
                string query = @"INSERT INTO user(first_name, last_name, email, salt, password)
                                VALUES (@naam, @achternaam,@email, @salt, @passwordHash)";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@naam", userDTO.Naam);
                        cmd.Parameters.AddWithValue("@achternaam", userDTO.AchterNaam);
                        cmd.Parameters.AddWithValue("email", userDTO.Email);
                        cmd.Parameters.AddWithValue("@salt", userDTO.Salt);
                        cmd.Parameters.AddWithValue("@passwordHash", userDTO.PasswordHash);


                        cmd.ExecuteNonQuery();

                    }


                }

                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij toevoegen gebruiker: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon gebruiker niet toevoegen", ex);

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

        // Verwijder een user
        public bool VerwijderUser(int userId)
        {
            if (!IsConnect(connString))
                return false;

            const string query = "DELETE FROM user WHERE id = @id";

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
