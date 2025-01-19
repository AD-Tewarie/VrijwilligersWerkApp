﻿using Domain.Common.Data;
using Domain.Common.Exceptions;
using Domain.Gebruikers.ValueObjects;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;
using Domain.Gebruikers.Services.WachtwoordStrategy.Data;

namespace Domain.Gebruikers.Models;

public class User
{
    private User(int userId, string naam, string achterNaam, EmailAdres email, WachtwoordData wachtwoordData)
    {
        ValideerNaam(naam);
        ValideerAchterNaam(achterNaam);

        UserId = userId;
        Naam = naam;
        AchterNaam = achterNaam;
        Email = email;
        this.wachtwoordData = wachtwoordData;
    }

    public int UserId { get; }
    public string Naam { get; private set; }
    public string AchterNaam { get; private set; }
    public EmailAdres Email { get; private set; }
    
    private readonly WachtwoordData wachtwoordData;
    public string PasswordHash => wachtwoordData.Hash;
    public string Salt => wachtwoordData.Salt;

    public static User MaakNieuw(string naam, string achterNaam, string email, string hash, string salt)
    {
        return new User(0, naam, achterNaam, new EmailAdres(email), new WachtwoordData(hash, salt));
    }

    public static User LaadVanuitData(UserData data)
    {
        var wachtwoordData = new WachtwoordData(data.PasswordHash, data.Salt);
        return new User(
            data.UserId,
            data.Naam,
            data.AchterNaam,
            new EmailAdres(data.Email),
            wachtwoordData
        );
    }

    public bool ValideerWachtwoord(string wachtwoord, IWachtwoordStrategy wachtwoordStrategy)
    {
        return wachtwoordStrategy.Valideer(wachtwoord, wachtwoordData);
    }

    public WachtwoordData GetWachtwoordData()
    {
        return wachtwoordData;
    }

    public void WijzigEmail(string nieuweEmail)
    {
        Email = new EmailAdres(nieuweEmail);
    }

    private static void ValideerNaam(string naam)
    {
        var fouten = new Dictionary<string, ICollection<string>>();

        if (string.IsNullOrWhiteSpace(naam))
            fouten.Add("Naam", new[] { "Naam is verplicht." });
        else if (naam.Length < 2)
            fouten.Add("Naam", new[] { "Naam moet minimaal 2 karakters lang zijn." });
        else if (naam.Length > 50)
            fouten.Add("Naam", new[] { "Naam mag maximaal 50 karakters lang zijn." });
        else if (!System.Text.RegularExpressions.Regex.IsMatch(naam, "^[a-zA-Z ]+$"))
            fouten.Add("Naam", new[] { "Naam mag alleen letters en spaties bevatten." });

        if (fouten.Any())
            throw new DomainValidationException("Validatie fouten opgetreden", fouten);
    }

    private static void ValideerAchterNaam(string achterNaam)
    {
        var fouten = new Dictionary<string, ICollection<string>>();

        if (string.IsNullOrWhiteSpace(achterNaam))
            fouten.Add("AchterNaam", new[] { "Achternaam is verplicht." });
        else if (achterNaam.Length < 2)
            fouten.Add("AchterNaam", new[] { "Achternaam moet minimaal 2 karakters lang zijn." });
        else if (achterNaam.Length > 50)
            fouten.Add("AchterNaam", new[] { "Achternaam mag maximaal 50 karakters lang zijn." });
        else if (!System.Text.RegularExpressions.Regex.IsMatch(achterNaam, "^[a-zA-Z ]+$"))
            fouten.Add("AchterNaam", new[] { "Achternaam mag alleen letters en spaties bevatten." });

        if (fouten.Any())
            throw new DomainValidationException("Validatie fouten opgetreden", fouten);
    }
}
