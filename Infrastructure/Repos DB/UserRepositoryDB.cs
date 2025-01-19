﻿using Domain.Common.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.Gebruikers.Models;
using Domain.Common.Data;
using Domain.Common.Exceptions;
using MySqlConnector;

namespace Infrastructure.Repos_DB;

public class UserRepositoryDB : IUserRepository
{
    private readonly IDatabaseService databaseService;

    public UserRepositoryDB(IDatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public void AddUser(User user)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection,
            @"INSERT INTO user (
                email,
                password,
                salt,
                first_name,
                last_name
            ) VALUES (
                IF(@email IS NULL OR @email = '', 'geen.email@opgegeven.nl', @email),
                IF(@password_hash IS NULL OR @password_hash = '', '', @password_hash),
                IF(@salt IS NULL OR @salt = '', '', @salt),
                IF(@first_name IS NULL OR @first_name = '', 'Onbekend', @first_name),
                IF(@last_name IS NULL OR @last_name = '', 'Onbekend', @last_name)
            )");

        var wachtwoordData = user.GetWachtwoordData();
        command.AddParameter("@email", user.Email.ToString());
        command.AddParameter("@password_hash", wachtwoordData.Hash);
        command.AddParameter("@salt", wachtwoordData.Salt);
        command.AddParameter("@first_name", user.Naam);
        command.AddParameter("@last_name", user.AchterNaam);

        command.ExecuteNonQuery();
    }

    public bool VerwijderUser(int userId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection,
            "DELETE FROM user WHERE id = @id");
        command.AddParameter("@id", userId);

        return command.ExecuteNonQuery() > 0;
    }

    public User GetUserOnId(int userId)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(email IS NULL OR email = '', 'geen.email@opgegeven.nl', email) as email,
                IF(password IS NULL OR password = '', '', password) as password,
                IF(salt IS NULL OR salt = '', '', salt) as salt,
                IF(first_name IS NULL OR first_name = '', 'Onbekend', first_name) as first_name,
                IF(last_name IS NULL OR last_name = '', 'Onbekend', last_name) as last_name
            FROM user
            WHERE id = @id");
        command.AddParameter("@id", userId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            var userData = new UserData(
                reader.GetInt32("id"),
                reader.GetString("first_name"),
                reader.GetString("last_name"),
                reader.GetString("email"),
                reader.GetString("password"),
                reader.GetString("salt")
            );
            return User.LaadVanuitData(userData);
        }

        throw new GebruikerNietGevondenException(userId);
    }

    public User? GetUserByEmail(string email)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(email IS NULL OR email = '', 'geen.email@opgegeven.nl', email) as email,
                IF(password IS NULL OR password = '', '', password) as password,
                IF(salt IS NULL OR salt = '', '', salt) as salt,
                IF(first_name IS NULL OR first_name = '', 'Onbekend', first_name) as first_name,
                IF(last_name IS NULL OR last_name = '', 'Onbekend', last_name) as last_name
            FROM user
            WHERE email = @email");
        command.AddParameter("@email", email);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            var userData = new UserData(
                reader.GetInt32("id"),
                reader.GetString("first_name"),
                reader.GetString("last_name"),
                reader.GetString("email"),
                reader.GetString("password"),
                reader.GetString("salt")
            );
            return User.LaadVanuitData(userData);
        }

        return null;
    }

    public List<User> GetUsers()
    {
        var users = new List<User>();

        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(email IS NULL OR email = '', 'geen.email@opgegeven.nl', email) as email,
                IF(password IS NULL OR password = '', '', password) as password,
                IF(salt IS NULL OR salt = '', '', salt) as salt,
                IF(first_name IS NULL OR first_name = '', 'Onbekend', first_name) as first_name,
                IF(last_name IS NULL OR last_name = '', 'Onbekend', last_name) as last_name
            FROM user");

        using var reader = (MySqlDataReader)command.ExecuteReader();
        while (reader.Read())
        {
            var userData = new UserData(
                reader.GetInt32("id"),
                reader.GetString("first_name"),
                reader.GetString("last_name"),
                reader.GetString("email"),
                reader.GetString("password"),
                reader.GetString("salt")
            );
            users.Add(User.LaadVanuitData(userData));
        }

        return users;
    }
}
