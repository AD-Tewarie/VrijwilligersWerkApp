using Domain.Common.Data;
using Domain.Common.Exceptions;
using Domain.Gebruikers.Services.WachtwoordStrategy.Data;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;

namespace Domain.Gebruikers.Models
{
    public class User
    {
        private readonly IWachtwoordStrategy wachtwoordStrategy;
        private WachtwoordData WachtwoordData { get; set; }

        public int UserId { get; private set; }
        public string Naam { get; private set; }
        public string AchterNaam { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash => WachtwoordData.Hash;
        public string Salt => WachtwoordData.Salt;

        private User(
            string naam,
            string achterNaam,
            string email,
            IWachtwoordStrategy wachtwoordStrategy)
        {
            this.wachtwoordStrategy = wachtwoordStrategy;
            WijzigDetails(naam, achterNaam, email);
        }

        public static User MaakNieuw(
            string naam,
            string achterNaam,
            string email,
            string wachtwoord,
            IWachtwoordStrategy wachtwoordStrategy)
        {
            var user = new User(naam, achterNaam, email, wachtwoordStrategy);
            user.SetWachtwoord(wachtwoord);
            return user;
        }

        public static User LaadVanuitData(
            UserData data,
            IWachtwoordStrategy wachtwoordStrategy)
        {
            var user = new User(data.Naam, data.AchterNaam, data.Email, wachtwoordStrategy);
            user.WachtwoordData = new WachtwoordData(data.PasswordHash, data.Salt);
            user.UserId = data.UserId;
            return user;
        }

        public UserData NaarData()
        {
            return new UserData(
                UserId,
                Naam,
                AchterNaam,
                Email,
                PasswordHash,
                Salt
            );
        }

        public void WijzigDetails(string naam, string achterNaam, string email)
        {
            ValideerGebruiker(naam, achterNaam, email);

            Naam = naam;
            AchterNaam = achterNaam;
            Email = email;
        }

        private static void ValideerGebruiker(string naam, string achterNaam, string email)
        {
            var fouten = new List<string>();

            if (string.IsNullOrWhiteSpace(naam))
                fouten.Add("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(achterNaam))
                fouten.Add("Achternaam is verplicht.");

            if (string.IsNullOrWhiteSpace(email))
                fouten.Add("Email is verplicht.");

            if (!IsGeldigEmailAdres(email))
                fouten.Add("Email is niet geldig.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

        private static bool IsGeldigEmailAdres(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void SetWachtwoord(string wachtwoord)
        {
            if (string.IsNullOrWhiteSpace(wachtwoord))
                throw new ArgumentException("Wachtwoord mag niet leeg zijn.");

            WachtwoordData = wachtwoordStrategy.Hash(wachtwoord);
        }

        public bool ValideerWachtwoord(string wachtwoord)
        {
            return wachtwoordStrategy.Valideer(wachtwoord, WachtwoordData);
        }
    }
}
