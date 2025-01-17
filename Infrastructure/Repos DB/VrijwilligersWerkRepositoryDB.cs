using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Extensions.Options;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Models;
using Domain.Common.Data;

namespace Infrastructure.Repos_DB
{
    public class VrijwilligersWerkRepositoryDB : IVrijwilligersWerkRepository
    {
        private readonly string connString;
        private MySqlConnection connection = null;

        public VrijwilligersWerkRepositoryDB(DBSettings settings)
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

        public List<VrijwilligersWerk> GetVrijwilligersWerk()
        {
            var werkLijst = new List<VrijwilligersWerk>();

            if (IsConnect(connString))
            {
                string query = "SELECT * FROM volenteer_work";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var werkData = new WerkData(
                                    reader.GetInt32("id"),
                                    reader.GetString("title"),
                                    reader.GetString("description"),
                                    reader.GetInt32("max_volenteers"),
                                    reader.GetInt32("reg_count")
                                );
                                werkLijst.Add(VrijwilligersWerk.LaadVanuitData(werkData));
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        File.AppendAllText("error.log", $"Fout bij het ophalen van werk: {ex.Message}" + Environment.NewLine);
                        throw new Exception("Kan vrijwilligerswerk niet ophalen", ex);
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
            return werkLijst;
        }

        public void AddVrijwilligersWerk(VrijwilligersWerk werk)
        {
            if (IsConnect(connString))
            {
                string query = @"INSERT INTO volenteer_work(title, description, max_volenteers)
                       VALUES (@title, @description, @max_volenteers);
                       SELECT LAST_INSERT_ID();";

                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        var werkData = werk.NaarData();
                        cmd.Parameters.AddWithValue("@title", werkData.Titel);
                        cmd.Parameters.AddWithValue("@description", werkData.Omschrijving);
                        cmd.Parameters.AddWithValue("@max_volenteers", werkData.MaxCapaciteit);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    File.AppendAllText("error.log", $"Fout bij toevoegen werk: {ex.Message}" + Environment.NewLine);
                    throw new Exception("Kan vrijwilligerswerk niet toevoegen", ex);
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

        public bool BewerkVrijwilligersWerk(VrijwilligersWerk werk)
        {
            if (!IsConnect(connString))
                return false;

            string query = @"UPDATE volenteer_work 
                        SET title = @title, 
                            description = @description, 
                            max_volenteers = @max_volenteers
                        WHERE id = @id";

            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    var werkData = werk.NaarData();
                    cmd.Parameters.AddWithValue("@id", werkData.WerkId);
                    cmd.Parameters.AddWithValue("@title", werkData.Titel);
                    cmd.Parameters.AddWithValue("@description", werkData.Omschrijving);
                    cmd.Parameters.AddWithValue("@max_volenteers", werkData.MaxCapaciteit);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij het bewerken van werk {werk.WerkId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kan vrijwilligerswerk: {werk.WerkId} niet bewerken", ex);
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

        public bool VerwijderVrijwilligersWerk(int werkId)
        {
            if (!IsConnect(connString))
                return false;

            string query = "DELETE FROM volenteer_work WHERE id = @id";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", werkId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij het verwijderen van werk {werkId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kan vrijwilligerswerk: {werkId} niet verwijderen", ex);
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

        public bool BewerkAantalRegistraties(int werkId, int wijziging)
        {
            if (!IsConnect(connString))
                return false;

            string query = @"UPDATE volenteer_work 
                        SET reg_count = GREATEST(0, COALESCE(reg_count, 0) + @wijziging)
                        WHERE id = @id";

            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", werkId);
                    cmd.Parameters.AddWithValue("@wijziging", wijziging);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij bewerken van registraties voor werk {werkId}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kan registratie aantal niet bewerken voor werk {werkId}", ex);
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

        public void VoegWerkCategorieToeAanNieuweWerk(int werkId, int categorieId)
        {
            if (!IsConnect(connString))
                return;

            string query = "INSERT INTO work_categories (work_id, category_id) VALUES (@workId, @categorieId)";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@workId", werkId);
                    cmd.Parameters.AddWithValue("@categorieId", categorieId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij het koppelen van de categorie: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kan categorie niet koppelen", ex);
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

        public VrijwilligersWerk GetWerkOnId(int id)
        {
            if (!IsConnect(connString))
                return null;

            string query = "SELECT * FROM volenteer_work WHERE id = @id";

            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var werkData = new WerkData(
                                reader.GetInt32("id"),
                                reader.GetString("title"),
                                reader.GetString("description"),
                                reader.GetInt32("max_volenteers"),
                                reader.GetInt32("reg_count")
                            );
                            return VrijwilligersWerk.LaadVanuitData(werkData);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                File.AppendAllText("error.log", $"Fout bij ophalen werk {id}: {ex.Message}" + Environment.NewLine);
                throw new Exception($"Kan vrijwilligerswerk {id} niet ophalen", ex);
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
    }
}