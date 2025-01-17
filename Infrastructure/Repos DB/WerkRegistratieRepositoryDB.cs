using Domain.Common.Data;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Models;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repos_DB
{
    public class WerkRegistratieRepositoryDB : IWerkRegistratieRepository
    {
        private readonly string connString;
        private MySqlConnection connection = null;
        private readonly IVrijwilligersWerkRepository werkRepository;
        private readonly IUserRepository userRepository;

        public WerkRegistratieRepositoryDB(
            DBSettings settings,
            IVrijwilligersWerkRepository werkRepository,
            IUserRepository userRepository)
        {
            connString = settings.DefaultConnection;
            this.werkRepository = werkRepository;
            this.userRepository = userRepository;
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

        public List<WerkRegistratie> GetWerkRegistraties()
        {
            var registraties = new List<WerkRegistratie>();

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
                                var werk = werkRepository.GetWerkOnId(reader.GetInt32("volenteer_work_id"));
                                var user = userRepository.GetUserOnId(reader.GetInt32("user_id"));

                                if (werk != null && user != null)
                                {
                                    registraties.Add(WerkRegistratie.LaadVanuitDatabase(
                                        reader.GetInt32("id"),
                                        werk,
                                        user
                                    ));
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen registraties: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kon werkregistraties niet ophalen", ex);
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

            return registraties;
        }

        public WerkRegistratie GetRegistratieOnId(int id)
        {
            if (!IsConnect(connString))
                return null;

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
                            var werk = werkRepository.GetWerkOnId(reader.GetInt32("volenteer_work_id"));
                            var user = userRepository.GetUserOnId(reader.GetInt32("user_id"));

                            if (werk != null && user != null)
                            {
                                return WerkRegistratie.LaadVanuitDatabase(
                                    id,
                                    werk,
                                    user
                                );
                            }
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

            return null;
        }

        public WerkRegistratie GetRegistratieOnWerkId(int werkId)
        {
            if (!IsConnect(connString))
                return null;

            string query = "SELECT * FROM volenteer_work_user WHERE volenteer_work_id = @werkId";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@werkId", werkId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var werk = werkRepository.GetWerkOnId(werkId);
                            var user = userRepository.GetUserOnId(reader.GetInt32("user_id"));

                            if (werk != null && user != null)
                            {
                                return WerkRegistratie.LaadVanuitDatabase(
                                    reader.GetInt32("id"),
                                    werk,
                                    user
                                );
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen registratie voor werk {werkId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon werkregistratie voor werk {werkId} niet ophalen", ex);
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

        public void AddWerkRegistratie(WerkRegistratie registratie)
        {
            if (!IsConnect(connString))
                return;

            string query = @"INSERT INTO volenteer_work_user(user_id, volenteer_work_id)
                        VALUES (@user_id, @werk_id)";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@user_id", registratie.User.UserId);
                    cmd.Parameters.AddWithValue("@werk_id", registratie.VrijwilligersWerk.WerkId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij toevoegen registratie: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon werkregistratie niet toevoegen", ex);
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

        public bool VerwijderWerkRegistratie(int registratieId)
        {
            if (!IsConnect(connString))
                return false;

            string query = "DELETE FROM volenteer_work_user WHERE id = @id";

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
                File.AppendAllText("error.log", $"Fout bij verwijderen registratie {registratieId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kon werkregistratie {registratieId} niet verwijderen", ex);
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

        public List<WerkRegistratie> GetRegistratiesForUser(int userId)
        {
            var registraties = new List<WerkRegistratie>();

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM volenteer_work_user WHERE user_id = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var werk = werkRepository.GetWerkOnId(reader.GetInt32("volenteer_work_id"));
                                var user = userRepository.GetUserOnId(userId);

                                if (werk != null && user != null)
                                {
                                    registraties.Add(WerkRegistratie.LaadVanuitDatabase(
                                        reader.GetInt32("id"),
                                        werk,
                                        user
                                    ));
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij ophalen registraties voor gebruiker {userId}: {ex.Message}" + Environment.NewLine);
                        throw new Exception($"Kon registraties voor gebruiker {userId} niet ophalen", ex);
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

            return registraties;
        }

        public bool BestaatRegistratie(int userId, int werkId)
        {
            if (!IsConnect(connString))
                return false;

            string query = "SELECT COUNT(*) FROM volenteer_work_user WHERE user_id = @userId AND volenteer_work_id = @werkId";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@werkId", werkId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij controleren registratie bestaan: {ex.Message}" + Environment.NewLine);
                throw new Exception("Kon registratie bestaan niet controleren", ex);
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

