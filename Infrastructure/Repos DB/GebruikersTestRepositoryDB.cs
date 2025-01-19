﻿using Domain.Common.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Models;
using Domain.Common.Exceptions;
using MySqlConnector;

namespace Infrastructure.Repos_DB;

public class GebruikersTestRepositoryDB : IGebruikersTestRepository
{
    private readonly IDatabaseService databaseService;

    public GebruikersTestRepositoryDB(IDatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public List<Categorie> HaalAlleCategorieënOp()
    {
        var categorieën = new List<Categorie>();
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(name IS NULL OR name = '', 'Onbekende categorie', name) as name
            FROM categories");

        using var reader = (MySqlDataReader)command.ExecuteReader();
        while (reader.Read())
        {
            categorieën.Add(Categorie.Maak(
                reader.GetInt32("id"),
                reader.GetString("name")
            ));
        }

        return categorieën;
    }

    public Categorie? GetCategorieOnId(int id)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(name IS NULL OR name = '', 'Onbekende categorie', name) as name
            FROM categories
            WHERE id = @id");
        command.AddParameter("@id", id);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            return Categorie.Maak(
                reader.GetInt32("id"),
                reader.GetString("name")
            );
        }

        return null;
    }

    public List<TestVraag> HaalAlleTestVragenOp()
    {
        var vragen = new List<TestVraag>();
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(text IS NULL OR text = '', 'Geen vraag beschikbaar', text) as text,
                IF(category_id <= 0, 1, category_id) as category_id
            FROM questions");

        using var reader = (MySqlDataReader)command.ExecuteReader();
        while (reader.Read())
        {
            vragen.Add(TestVraag.Maak(
                reader.GetInt32("id"),
                reader.GetString("text"),
                reader.GetInt32("category_id")
            ));
        }

        return vragen;
    }

    public TestVraag? GetTestVraagOnId(int id)
    {
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                id,
                IF(text IS NULL OR text = '', 'Geen vraag beschikbaar', text) as text,
                IF(category_id <= 0, 1, category_id) as category_id
            FROM questions
            WHERE id = @id");
        command.AddParameter("@id", id);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        if (reader.Read())
        {
            return TestVraag.Maak(
                reader.GetInt32("id"),
                reader.GetString("text"),
                reader.GetInt32("category_id")
            );
        }

        return null;
    }

    public List<WerkCategorie> GetWerkCategorieënByWerkId(int werkId)
    {
        var categorieën = new List<WerkCategorie>();
        using var connection = databaseService.GetConnection();
        databaseService.OpenConnection(connection);

        var command = databaseService.CreateCommand(connection, @"
            SELECT
                work_id,
                category_id
            FROM work_categories
            WHERE work_id = @werk_id");
        command.AddParameter("@werk_id", werkId);

        using var reader = (MySqlDataReader)command.ExecuteReader();
        while (reader.Read())
        {
            categorieën.Add(WerkCategorie.Maak(
                reader.GetInt32("work_id"),
                reader.GetInt32("category_id")
            ));
        }

        return categorieën;
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
}
