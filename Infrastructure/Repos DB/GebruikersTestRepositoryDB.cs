using Infrastructure.DTO;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Infrastructure.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos_DB
{
    public class GebruikersTestRepositoryDB : IGebruikersTestRepository
    {
        private readonly string connString;
        private MySqlConnection connection;
       

        public GebruikersTestRepositoryDB(DBSettings settings)
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

        // ophalen categorie//

        public List<CategorieDTO> HaalAlleCategorieënOp()
        {
            List<CategorieDTO> categorieLijst = new List<CategorieDTO>();

            if (IsConnect(connString))
            {
                string query = "SELECT id, name FROM categories";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string naam = reader.GetString("name");

                                CategorieDTO categorie = new CategorieDTO(id, naam);
                                categorieLijst.Add(categorie);
                            }
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Query uitvoering mislukt: {ex.Message}");
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

            return categorieLijst;
        }



        public CategorieDTO GetCategorieOnId(int id)
        {

            CategorieDTO categorie = null;

            if (IsConnect(connString))
            {
                string query = "SELECT id, naam FROM categories WHERE id = @id";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int categorieId = reader.GetInt32("id");
                                string naam = reader.GetString("naam");

                                categorie = new CategorieDTO(id, naam);
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

            return categorie;


        }



        // Ophalen Testvraag//


        public List<TestVraagDTO> HaalAlleTestVraagOp()
        {
            List<TestVraagDTO> vraagLijst = new List<TestVraagDTO>();

            if (IsConnect(connString))
            {
                string query = "SELECT id, text, category_id FROM questions";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int vraagId = reader.GetInt32("id");
                                string tekst = reader.GetString("text");
                                int categorieId = reader.GetInt32("category_id");

                                vraagLijst.Add(new TestVraagDTO(vraagId, tekst, categorieId));
                            }
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Query uitvoering mislukt: {ex.Message}");
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

            return vraagLijst;
        }




        public TestVraagDTO GetTestVraagOnId(int id)
        {

            TestVraagDTO testVraag = null;

            if (IsConnect(connString))
            {
                string query = "SELECT id, text, category_id FROM questions WHERE id = @id";


                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int vraagId = reader.GetInt32("id");
                                string tekst = reader.GetString("text");
                                int categorieId = reader.GetInt32("category_id");

                                return new TestVraagDTO (vraagId, tekst, categorieId);

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

            return testVraag;


        }





        
        public List<TestVraagDTO> GetTestVraagOpCategorieId(int categorieId)
        {

            var vragenLijst = new List<TestVraagDTO>();

            if (IsConnect(connString))
            {
                string query = "SELECT id, text, category_id FROM questions WHERE category_id = @categorieId";



                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@categorieId", categorieId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int vraagId = reader.GetInt32("id");
                                string tekst = reader.GetString("text");
                                int catId = reader.GetInt32("category_id");

                                vragenLijst.Add(new TestVraagDTO(vraagId, tekst, catId));

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

            return vragenLijst;


        }



    }
}
