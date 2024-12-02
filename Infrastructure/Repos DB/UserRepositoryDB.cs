using Infrastructure.DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos_DB
{
    public class UserRepositoryDB
    {

        private string conString = "server=localhost;user=root;database=seniorconnect;port=3306;password=Devesh97!";
        private MySqlConnection connection = null;



        private bool IsConnect()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(conString);
                connection.Open();
            }
            return true;
        }



        // ophalen


        public List<UserDTO> GetUsers()
        {
            List<UserDTO> userLijst = new List<UserDTO>();

            if (IsConnect())
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
                                


                                UserDTO user = new UserDTO(id, voornaam, achternaam);
                                userLijst.Add(user);
                            }
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Query execution failed: {ex.Message}");
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


        public UserDTO GetUserOnId(int id)
        {

            UserDTO userDTO = null;

            if (IsConnect())
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
                                

                                userDTO = new UserDTO(id ,naam, achterNaam);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Query execution failed: {ex.Message}");
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

            if (IsConnect())
            {
                string query = @"INSERT INTO user(id, first_name, last_name )
                                VALUES (@id, @naam, @achternaam)";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {



                        cmd.Parameters.AddWithValue("@id", userDTO.UserId);
                        cmd.Parameters.AddWithValue("@title", userDTO.Naam);
                        cmd.Parameters.AddWithValue("@achternaam", userDTO.AchterNaam);
                  


                        cmd.ExecuteNonQuery();

                    }


                }

                catch (MySqlException ex)
                {
                    Console.WriteLine($"Query execution failed: {ex.Message}");

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
            if (!IsConnect())
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
                Console.WriteLine($"Query execution failed: {ex.Message}");
                return false;
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
