using Application.Authenticatie.Interfaces;
using Application.Authenticatie.ViewModels;
using Domain.Gebruikers.Interfaces;
using Domain.Gebruikers.Models;

namespace Application.Authenticatie.Services
{
    public class AuthenticatieService : IAuthenticatieService
    {
        private readonly IUserBeheer userBeheer;

        public AuthenticatieService(IUserBeheer userBeheer)
        {
            this.userBeheer = userBeheer ?? throw new ArgumentNullException(nameof(userBeheer));
        }

        public bool Login(string email, string wachtwoord)
        {
            return userBeheer.ValideerGebruiker(email, wachtwoord);
        }

        public void Registreer(string naam, string achternaam, string email, string wachtwoord)
        {
            userBeheer.VoegGebruikerToe(naam, achternaam, email, wachtwoord);
        }

        public bool BestaatEmail(string email)
        {
            try
            {
                var gebruiker = userBeheer.HaalGebruikerOpEmail(email);
                return gebruiker != null;
            }
            catch
            {
                return false;
            }
        }

        public GebruikerViewModel HaalGebruikerOpEmail(string email)
        {
            var gebruiker = userBeheer.HaalGebruikerOpEmail(email);
            if (gebruiker == null)
            {
                throw new KeyNotFoundException($"Gebruiker met email {email} niet gevonden.");
            }

            return new GebruikerViewModel(
                gebruiker.UserId,
                gebruiker.Naam,
                gebruiker.AchterNaam,
                gebruiker.Email
            );
        }
    }
}