﻿using Domain.Common.Exceptions;
using Domain.Common.Interfaces.Repository;
using Domain.Gebruikers.Interfaces;
using Domain.Gebruikers.Models;
using Domain.Gebruikers.Services.WachtwoordStrategy.Data;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;

namespace Domain.Gebruikers.Services;

public class UserBeheer : IUserBeheer
{
    private readonly IUserRepository repository;
    private readonly IWachtwoordStrategy wachtwoordStrategy;

    public UserBeheer(
        IUserRepository repository,
        IWachtwoordStrategy wachtwoordStrategy)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.wachtwoordStrategy = wachtwoordStrategy ?? throw new ArgumentNullException(nameof(wachtwoordStrategy));
    }

    public void VoegGebruikerToe(string naam, string achterNaam, string email, string wachtwoord)
    {
        ValideerUniekeEmail(email);
        ValideerGebruikerGegevens(naam, achterNaam, email, wachtwoord);

        var wachtwoordData = wachtwoordStrategy.Hash(wachtwoord);
        var user = User.MaakNieuw(naam, achterNaam, email, wachtwoordData.Hash, wachtwoordData.Salt);
        repository.AddUser(user);
    }

    public List<User> HaalAlleGebruikersOp()
    {
        return repository.GetUsers();
    }

    public User HaalGebruikerOpID(int userId)
    {
        ValideerGebruikerID(userId);
        var user = repository.GetUserOnId(userId);

        if (user == null)
            throw new KeyNotFoundException($"Gebruiker met ID {userId} niet gevonden.");

        return user;
    }

    public User HaalGebruikerOpEmail(string email)
    {
        ValideerEmail(email);
        var user = repository.GetUserByEmail(email);

        if (user == null)
            throw new KeyNotFoundException($"Gebruiker met email {email} niet gevonden.");

        return user;
    }

    public bool ValideerGebruiker(string email, string wachtwoord)
    {
        ValideerInlogGegevens(email, wachtwoord);
        var user = repository.GetUserByEmail(email);
        
        if (user == null)
            return false;

        return user.ValideerWachtwoord(wachtwoord, wachtwoordStrategy);
    }

    public void VerwijderGebruiker(int userId)
    {
        ValideerGebruikerID(userId);
        var user = HaalGebruikerOpID(userId); // This already checks if user exists
        repository.VerwijderUser(userId);
    }

    private void ValideerGebruikerGegevens(string naam, string achterNaam, string email, string wachtwoord)
    {
        var fouten = new Dictionary<string, ICollection<string>>();

        if (string.IsNullOrWhiteSpace(naam))
            fouten.Add("Naam", new[] { "Naam is verplicht." });
        if (string.IsNullOrWhiteSpace(achterNaam))
            fouten.Add("AchterNaam", new[] { "Achternaam is verplicht." });
        if (string.IsNullOrWhiteSpace(email))
            fouten.Add("Email", new[] { "Email is verplicht." });
        if (string.IsNullOrWhiteSpace(wachtwoord))
            fouten.Add("Wachtwoord", new[] { "Wachtwoord is verplicht." });
        else if (wachtwoord.Length < 8)
            fouten.Add("Wachtwoord", new[] { "Wachtwoord moet minimaal 8 karakters lang zijn." });

        if (fouten.Any())
            throw new DomainValidationException("Validatie fouten opgetreden", fouten);
    }

    private void ValideerUniekeEmail(string email)
    {
        var existingUser = repository.GetUserByEmail(email);
        if (existingUser != null)
        {
            throw new DomainValidationException("Email", "Er bestaat al een gebruiker met dit emailadres.");
        }
    }

    private void ValideerGebruikerID(int userId)
    {
        if (userId <= 0)
            throw new DomainValidationException("UserId", "Gebruiker ID moet groter zijn dan 0.");
    }

    private void ValideerEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainValidationException("Email", "Email mag niet leeg zijn.");
    }

    private void ValideerInlogGegevens(string email, string wachtwoord)
    {
        var fouten = new Dictionary<string, ICollection<string>>();

        if (string.IsNullOrWhiteSpace(email))
            fouten.Add("Email", new[] { "Email is verplicht." });
        if (string.IsNullOrWhiteSpace(wachtwoord))
            fouten.Add("Wachtwoord", new[] { "Wachtwoord is verplicht." });

        if (fouten.Any())
            throw new DomainValidationException("Validatie fouten opgetreden", fouten);
    }
}
