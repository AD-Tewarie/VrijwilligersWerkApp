﻿using Domain.Common.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Models;
using Domain.Common.Exceptions;
using MySqlConnector;

namespace Infrastructure.Repos_DB;

public class WerkRegistratieRepositoryDB : IWerkRegistratieRepository
{
    private readonly IDatabaseService databaseService;
    private readonly IVrijwilligersWerkRepository werkRepository;
    private readonly IUserRepository userRepository;

    public WerkRegistratieRepositoryDB(
        IDatabaseService databaseService,
        IVrijwilligersWerkRepository werkRepository,
        IUserRepository userRepository)
    {
        this.databaseService = databaseService;
        this.werkRepository = werkRepository;
        this.userRepository = userRepository;
    }

    public void AddWerkRegistratie(WerkRegistratie registratie)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);
        using var transaction = databaseService.BeginTransaction(connection);

        try
        {
            // Check if registration already exists
            var checkCommand = databaseService.CreateCommand(connection, @"
                SELECT COUNT(*) as count
                FROM volenteer_work_user
                WHERE volenteer_work_id = @werk_id
                AND user_id = @user_id");

            checkCommand.Transaction = transaction;
            checkCommand.AddParameter("@werk_id", registratie.VrijwilligersWerk.WerkId);
            checkCommand.AddParameter("@user_id", registratie.User.UserId);

            using var reader = (MySqlDataReader)checkCommand.ExecuteReader();
            if (reader.Read() && reader.GetInt32("count") > 0)
            {
                throw new DomainValidationException("Registratie",
                    new Dictionary<string, ICollection<string>> {
                        { "Registratie", new[] { "Je bent al geregistreerd voor dit werk." } }
                    });
            }
            reader.Close();

            // Add registration
            var insertCommand = databaseService.CreateCommand(connection, @"
                INSERT INTO volenteer_work_user (
                    volenteer_work_id,
                    user_id
                ) VALUES (
                    IF(@werk_id <= 0, 1, @werk_id),
                    IF(@user_id <= 0, 1, @user_id)
                )");

            insertCommand.Transaction = transaction;
            insertCommand.AddParameter("@werk_id", registratie.VrijwilligersWerk.WerkId);
            insertCommand.AddParameter("@user_id", registratie.User.UserId);

            insertCommand.ExecuteNonQuery();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            if (ex is DomainValidationException)
                throw;
            throw new DomainValidationException("Registratie",
                new Dictionary<string, ICollection<string>> {
                    { "Database", new[] { ex.Message } }
                });
        }
    }

    public bool VerwijderWerkRegistratie(int registratieId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection,
            "DELETE FROM volenteer_work_user WHERE id = @id");
        command.AddParameter("@id", registratieId);

        return command.ExecuteNonQuery() > 0;
    }

    public List<WerkRegistratie> GetWerkRegistraties()
    {
        var registraties = new List<WerkRegistratie>();
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(volenteer_work_id <= 0, 1, volenteer_work_id) as volenteer_work_id,
                IF(user_id <= 0, 1, user_id) as user_id
            FROM volenteer_work_user");

        using var reader = (MySqlDataReader)command.ExecuteReader();
        while (reader.Read())
        {
            var registratieId = reader.GetInt32("id");
            var werkId = reader.GetInt32("volenteer_work_id");
            var userId = reader.GetInt32("user_id");

            var werk = werkRepository.GetWerkOnId(werkId);
            var user = userRepository.GetUserOnId(userId);

            if (werk != null && user != null)
            {
                registraties.Add(WerkRegistratie.LaadVanuitDatabase(registratieId, werk, user));
            }
        }

        return registraties;
    }

    public WerkRegistratie? GetRegistratieOnId(int registratieId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                IF(volenteer_work_id <= 0, 1, volenteer_work_id) as volenteer_work_id,
                IF(user_id <= 0, 1, user_id) as user_id
            FROM volenteer_work_user
            WHERE id = @id");
        command.AddParameter("@id", registratieId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            var werkId = reader.GetInt32("volenteer_work_id");
            var userId = reader.GetInt32("user_id");

            var werk = werkRepository.GetWerkOnId(werkId);
            var user = userRepository.GetUserOnId(userId);

            if (werk != null && user != null)
            {
                return WerkRegistratie.LaadVanuitDatabase(registratieId, werk, user);
            }
        }

        return null;
    }

    public WerkRegistratie? GetRegistratieOnWerkId(int werkId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(user_id <= 0, 1, user_id) as user_id
            FROM volenteer_work_user
            WHERE volenteer_work_id = @werk_id
            LIMIT 1");
        command.AddParameter("@werk_id", werkId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            var registratieId = reader.GetInt32("id");
            var userId = reader.GetInt32("user_id");

            var werk = werkRepository.GetWerkOnId(werkId);
            var user = userRepository.GetUserOnId(userId);

            if (werk != null && user != null)
            {
                return WerkRegistratie.LaadVanuitDatabase(registratieId, werk, user);
            }
        }

        return null;
    }

    public bool HeeftGebruikerRegistratie(int werkId, int gebruikerId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT COUNT(*) as count
            FROM volenteer_work_user
            WHERE volenteer_work_id = @werk_id
            AND user_id = @user_id");
        command.AddParameter("@werk_id", werkId);
        command.AddParameter("@user_id", gebruikerId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetInt32("count") > 0;
        }

        return false;
    }

    public int GetRegistratieCountForWerk(int werkId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT COUNT(*) as count
            FROM volenteer_work_user
            WHERE volenteer_work_id = @werk_id");
        command.AddParameter("@werk_id", werkId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetInt32("count");
        }

        return 0;
    }
}
