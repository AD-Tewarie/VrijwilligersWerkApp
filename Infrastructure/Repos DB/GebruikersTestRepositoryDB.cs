using Domain.Common.Interfaces.Repository;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repos_DB
{
    public class GebruikersTestRepositoryDB : IGebruikersTestRepository
    {
        private readonly string connString;
        private MySqlConnection connection = null;

        public GebruikersTestRepositoryDB(DBSettings settings)
        {
            connString = settings.DefaultConnection;
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

        public List<Categorie> HaalAlleCategorieënOp()
        {
            var categorieën = new List<Categorie>();

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
                                categorieën.Add(Categorie.Maak(
                                    reader.GetInt32("id"),
                                    reader.GetString("name")
                                ));
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen categorieën: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kon categorieën niet ophalen", ex);
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
            return categorieën;
        }

        public Categorie GetCategorieOnId(int id)
        {
            if (!IsConnect(connString))
                return null;

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
                            return Categorie.Maak(
                                reader.GetInt32("id"),
                                reader.GetString("name")
                            );
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen categorie: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon categorie niet ophalen", ex);
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

        public List<TestVraag> HaalAlleTestVragenOp()
        {
            var vragen = new List<TestVraag>();

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
                                vragen.Add(TestVraag.Maak(
                                    reader.GetInt32("id"),
                                    reader.GetString("text"),
                                    reader.GetInt32("category_id")
                                ));
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen testvragen: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kon testvragen niet ophalen", ex);
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
            return vragen;
        }

        public TestVraag GetTestVraagOnId(int id)
        {
            if (!IsConnect(connString))
                return null;

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
                            return TestVraag.Maak(
                                reader.GetInt32("id"),
                                reader.GetString("text"),
                                reader.GetInt32("category_id")
                            );
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen testvraag: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon testvraag niet ophalen", ex);
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

        public List<TestVraag> GetTestVraagOpCategorieId(int categorieId)
        {
            var vragen = new List<TestVraag>();

            if (IsConnect(connString))
            {
                string query = "SELECT id, text, category_id FROM questions WHERE category_id = @categorieId";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@categorieId", categorieId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vragen.Add(TestVraag.Maak(
                                    reader.GetInt32("id"),
                                    reader.GetString("text"),
                                    reader.GetInt32("category_id")
                                ));
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen testvragen voor categorie: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kon testvragen niet ophalen", ex);
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

            return vragen;
        }

        public List<WerkCategorie> GetWerkCategorieënByWerkId(int werkId)
        {
            var werkCategorieën = new List<WerkCategorie>();

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
                                werkCategorieën.Add(WerkCategorie.Maak(
                                    reader.GetInt32("work_id"),
                                    reader.GetInt32("category_id")
                                ));
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen werkcategorieën: {ex.Message}" + Environment.NewLine);
                    throw new Exception("Kon werkcategorieën niet ophalen", ex);
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

            return werkCategorieën;
        }
    }
}

