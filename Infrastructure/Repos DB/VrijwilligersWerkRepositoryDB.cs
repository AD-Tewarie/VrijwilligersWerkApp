﻿using Domain.Common.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Models;
using Domain.Common.Data;
using Domain.Common.Exceptions;
using MySqlConnector;

namespace Infrastructure.Repos_DB
{
    public class VrijwilligersWerkRepositoryDB : IVrijwilligersWerkRepository
    {
        private readonly IDatabaseService databaseService;

        public VrijwilligersWerkRepositoryDB(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public List<VrijwilligersWerk> GetVrijwilligersWerk()
        {
            var werken = new List<VrijwilligersWerk>();
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection, @"
                SELECT
                    id,
                    IF(title IS NULL OR title = '', 'Onbekende titel', title) as title,
                    IF(description IS NULL OR description = '', 'Geen omschrijving beschikbaar', description) as description,
                    IF(max_volenteers <= 0, 1, max_volenteers) as max_volenteers,
                    IF(location IS NULL OR location = '', 'Locatie nog niet bekend', location) as location
                FROM volenteer_work");
            using var reader = (MySqlDataReader)command.ExecuteReader();
            
            while (reader.Read())
            {
                var werkId = reader.GetInt32("id");
                var werkData = new WerkData(
                    werkId,
                    reader.GetString("title"),
                    reader.GetString("description"),
                    reader.GetInt32("max_volenteers"),
                    reader.GetString("location")
                );
                werken.Add(VrijwilligersWerk.LaadVanuitData(werkData));
            }

            return werken;
        }

        public VrijwilligersWerk GetWerkOnId(int id)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection, @"
                SELECT
                    id,
                    IF(title IS NULL OR title = '', 'Onbekende titel', title) as title,
                    IF(description IS NULL OR description = '', 'Geen omschrijving beschikbaar', description) as description,
                    IF(max_volenteers <= 0, 1, max_volenteers) as max_volenteers,
                    IF(location IS NULL OR location = '', 'Locatie nog niet bekend', location) as location
                FROM volenteer_work
                WHERE id = @id");
            command.AddParameter("@id", id);

            using var reader = (MySqlDataReader)command.ExecuteReader();
            if (reader.Read())
            {
                var werkId = reader.GetInt32("id");
                var werkData = new WerkData(
                    werkId,
                    reader.GetString("title"),
                    reader.GetString("description"),
                    reader.GetInt32("max_volenteers"),
                    reader.GetString("location")
                );
                return VrijwilligersWerk.LaadVanuitData(werkData);
            }

            throw new DomainValidationException($"Vrijwilligerswerk met ID {id} niet gevonden.");
        }

        public int AddVrijwilligersWerk(VrijwilligersWerk werk)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var data = werk.NaarData();
            var command = databaseService.CreateCommand(connection,
                @"INSERT INTO volenteer_work (title, description, max_volenteers, location)
                VALUES (
                    IF(@title IS NULL OR @title = '', 'Onbekende titel', @title),
                    IF(@description IS NULL OR @description = '', 'Geen omschrijving beschikbaar', @description),
                    IF(@max_volenteers <= 0, 1, @max_volenteers),
                    IF(@location IS NULL OR @location = '', 'Locatie nog niet bekend', @location)
                );
                SELECT LAST_INSERT_ID();");

            command.AddParameter("@title", data.Titel);
            command.AddParameter("@description", data.Omschrijving);
            command.AddParameter("@max_volenteers", data.MaxCapaciteit);
            command.AddParameter("@location", data.Locatie);

            using var reader = (MySqlDataReader)command.ExecuteReader();
            if (!reader.Read())
            {
                throw new Exception("Failed to get the new work ID");
            }
            return Convert.ToInt32(reader.GetValue(0));
        }

        public void VoegWerkCategorieToeAanNieuweWerk(int werkId, int categorieId)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection,
                "INSERT INTO work_categories (work_id, category_id) VALUES (@workId, @categoryId)");

            command.AddParameter("@workId", werkId);
            command.AddParameter("@categoryId", categorieId);
            command.ExecuteNonQuery();
        }

        public List<int> GetWerkCategorieënByWerkId(int werkId)
        {
            var categorieIds = new List<int>();
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection,
                "SELECT category_id FROM work_categories WHERE work_id = @werkId");
            command.AddParameter("@werkId", werkId);

            using var reader = (MySqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                categorieIds.Add(reader.GetInt32("category_id"));
            }

            return categorieIds;
        }

        public bool BewerkVrijwilligersWerk(VrijwilligersWerk werk)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var data = werk.NaarData();
            var command = databaseService.CreateCommand(connection,
                @"UPDATE volenteer_work 
                SET title = IF(@title IS NULL OR @title = '', 'Onbekende titel', @title),
                    description = IF(@description IS NULL OR @description = '', 'Geen omschrijving beschikbaar', @description),
                    max_volenteers = IF(@max_volenteers <= 0, 1, @max_volenteers),
                    location = IF(@location IS NULL OR @location = '', 'Locatie nog niet bekend', @location)
                WHERE id = @id");

            command.AddParameter("@id", data.WerkId);
            command.AddParameter("@title", data.Titel);
            command.AddParameter("@description", data.Omschrijving);
            command.AddParameter("@max_volenteers", data.MaxCapaciteit);
            command.AddParameter("@location", data.Locatie);

            return command.ExecuteNonQuery() > 0;
        }

        public bool VerwijderVrijwilligersWerk(int werkId)
        {
            using var connection = databaseService.GetConnection();
            databaseService.OpenConnection(connection);

            var command = databaseService.CreateCommand(connection, "DELETE FROM volenteer_work WHERE id = @id");
            command.AddParameter("@id", werkId);

            return command.ExecuteNonQuery() > 0;
        }
    }
}