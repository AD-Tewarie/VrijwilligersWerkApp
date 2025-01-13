using Infrastructure.DTO;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos_DB
{
    public class WerkRegistratieRepositoryDB : IWerkRegistratieRepository
    {

        private IUserRepository userDB;
        private IVrijwilligersWerkRepository werkDB;
        private string connString;
        private MySqlConnection connection = null;



        public WerkRegistratieRepositoryDB(DBSettings settings, IVrijwilligersWerkRepository werkDb, IUserRepository userDb)
        {
            userDB = userDb;
            werkDB = werkDb;
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

        // Tuple method

        public (UserDTO, VrijwilligersWerkDTO) HelperMethod(int userId, int werkId)
        {
            UserDTO user = userDB.GetUserOnId(userId);
            VrijwilligersWerkDTO werk = werkDB.GetWerkOnId(werkId);

            return (user, werk);
        }


        //Ophalen

        public List<WerkRegistratieDTO> GetWerkRegistraties()
        {
            List<WerkRegistratieDTO> registratieLijst = new List<WerkRegistratieDTO>();

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM volenteer_work_user";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                int userId = reader.GetInt32("user_id");
                                int werkId = reader.GetInt32("volenteer_work_id");

                                var (user, werk) = HelperMethod(userId, werkId);


                                WerkRegistratieDTO registratie = new WerkRegistratieDTO(werk, user, id);
                                registratieLijst.Add(registratie);
                            }
                            reader.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen van registraties: {ex.Message}" + Environment.NewLine);
                        throw new Exception($"Kon werkregistraties niet ophalen", ex);
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
            return registratieLijst;
        }



        public WerkRegistratieDTO GetRegistratieOnId(int id)
        {

            WerkRegistratieDTO werkRegistratie = null;

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM volenteer_work_user WHERE id = @id";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int userId = reader.GetInt32("user_id");
                                int werkId = reader.GetInt32("volenteer_work_id");


                                var (user, werk) = HelperMethod(userId, werkId);


                                werkRegistratie = new WerkRegistratieDTO(werk, user, id);

                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen registratie {id}: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon werkregistratie {id} niet ophalen", ex);
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

            return werkRegistratie;


        }


        // Haal registratie op werkID

        public WerkRegistratieDTO GetRegistratieOnWerkId(int checkId)
        {
            List<WerkRegistratieDTO> registratieLijst = new List<WerkRegistratieDTO>();
            WerkRegistratieDTO werkRegistratie = null;

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM volenteer_work_user WHERE volenteer_work_id = @id";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@id", checkId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                int userId = reader.GetInt32("user_id");
                                int werkId = reader.GetInt32("volenteer_work_id");

                                var (user, werk) = HelperMethod(userId, werkId);


                                WerkRegistratieDTO registratie = new WerkRegistratieDTO(werk, user, id);
                                registratieLijst.Add(registratie);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij ophalen registratie: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon werkregistratie niet ophalen", ex);
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

            return werkRegistratie;


        }

        // Toevoegen

        public void AddWerkRegistratie(WerkRegistratieDTO registratieDTO)
        {

            WerkRegistratieDTO dto = null;

            if (IsConnect(connString))
            {
                string query = @"INSERT INTO volenteer_work_user(user_id, volenteer_work_id)
                                VALUES (@user_id, @werk_id)";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {




                        cmd.Parameters.AddWithValue("@user_id", registratieDTO.User.UserId);
                        cmd.Parameters.AddWithValue("@werk_id", registratieDTO.VrijwilligersWerk.WerkId);



                        cmd.ExecuteNonQuery();

                    }


                }

                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij toevoegen van registratie: {ex.Message}" + Environment.NewLine);
                    throw new Exception($"Kon werkregistratie niet toevoegen", ex);

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




        //Verwijderen

        public bool VerwijderWerkRegistratie(int registratieId)
        {
            if (!IsConnect(connString))
                return false;

            const string query = "DELETE FROM volenteer_work_user WHERE id = @id";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@id", registratieId);


                    int rowsAffected = cmd.ExecuteNonQuery();


                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij verwijderen van registratie: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon werkregistratie niet verwijderen", ex);

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




        // ophalen aantal registraties

        public int GetRegistratieCountForWerk(int werkId)
        {
            if (!IsConnect(connString))
                return 0;

            string query = "SELECT COUNT(*) FROM volenteer_work_user WHERE volenteer_work_id = @werkId";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@werkId", werkId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen registratie telling voor werk {werkId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon registratie telling niet ophalen voor werk {werkId}", ex);
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
