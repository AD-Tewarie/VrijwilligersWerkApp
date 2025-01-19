﻿namespace Domain.GebruikersTest.Models;

public class Categorie
{
    public int Id { get; private set; }
    public string Naam { get; private set; }

    private Categorie(int id, string naam)
    {
        Id = id;
        Naam = naam;
    }

    public static Categorie Maak(int id, string naam)
    {
        return new Categorie(id, naam);
    }
}
