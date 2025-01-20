using Domain.Common.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.Gebruikers.ValueObjects;

public class EmailAdres
{
    private readonly string value;
    
    public EmailAdres(string value)
    {
        if (!IsGeldig(value))
            throw new DomainValidationException("Email", "Ongeldig email formaat");
        this.value = value.ToLower(); 
    }

    private static bool IsGeldig(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) 
            return false;
        
        try
        {
            // More strict email validation using regex
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => value;

    public static implicit operator string(EmailAdres email) => email.ToString();

    public override bool Equals(object? obj)
    {
        if (obj is EmailAdres other)
            return string.Equals(value, other.value, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode()
    {
        return value.ToLower().GetHashCode();
    }
}