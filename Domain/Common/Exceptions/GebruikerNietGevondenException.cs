using System;

namespace Domain.Common.Exceptions;

public class GebruikerNietGevondenException : DomainValidationException
{
    public GebruikerNietGevondenException(int userId) 
        : base("Gebruiker", $"Gebruiker met ID {userId} niet gevonden.")
    {
        UserId = userId;
    }

    public GebruikerNietGevondenException(string email) 
        : base("Gebruiker", $"Gebruiker met email {email} niet gevonden.")
    {
        Email = email;
    }

    public int? UserId { get; }
    public string? Email { get; }
}