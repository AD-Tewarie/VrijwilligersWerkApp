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
                        File.AppendAllText("error.log", $"Fout bij ophalen categorieën: {ex.Message}" + Environment.NewLine);
                        throw new Exception($"Kon categorieën niet ophalen", ex);
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
                string query = "SELECT id, name FROM categories WHERE id = @id";

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
                                string naam = reader.GetString("name");

                                categorie = new CategorieDTO(id, naam);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen categorie: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon categorie niet ophalen", ex);
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




        public List<WerkCategorieDTO> GetWerkCategorieënByWerkId(int werkId)
        {
            var werkCategorieLijst = new List<WerkCategorieDTO>();

            if (IsConnect(connString))
            {
                string query = "SELECT work_id, category_id FROM work_categories WHERE work_id = @werkId";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@werkId", werkId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int werkIdDb = reader.GetInt32("work_id");
                                int categorieId = reader.GetInt32("category_id");

                                werkCategorieLijst.Add(new WerkCategorieDTO(werkIdDb, categorieId));
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen categorieën: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon categorieën niet ophalen", ex);
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

            return werkCategorieLijst;
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
                        File.AppendAllText("error.log", $"Fout bij ophalen testvragen: {ex.Message}" + Environment.NewLine);
                        throw new Exception($"Kon testvragen niet ophalen", ex);
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
                    File.AppendAllText("error.log", $"Fout bij ophalen testvraag: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon testvraag niet ophalen", ex);
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
                    File.AppendAllText("error.log", $"Fout bij ophalen testvraag: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon testvraag niet ophalen", ex);
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
