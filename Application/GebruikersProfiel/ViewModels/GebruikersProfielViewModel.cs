using Application.Werk.ViewModels;

namespace Application.GebruikersProfiel.ViewModels
{
    public class GebruikersProfielViewModel
    {
        public int UserId { get; private set; }
        public string Naam { get; private set; }
        public string AchterNaam { get; private set; }
        public string Email { get; private set; }
        public List<WerkRegistratieViewModel> Registraties { get; private set; }

        public GebruikersProfielViewModel(
            int userId,
            string naam,
            string achterNaam,
            string email,
            List<WerkRegistratieViewModel> registraties)
        {
            if (string.IsNullOrWhiteSpace(naam))
                throw new ArgumentException("Naam is verplicht.");
            if (string.IsNullOrWhiteSpace(achterNaam))
                throw new ArgumentException("Achternaam is verplicht.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is verplicht.");
            if (registraties == null)
                throw new ArgumentException("Registraties mag niet null zijn.");

            UserId = userId;
            Naam = naam;
            AchterNaam = achterNaam;
            Email = email;
            Registraties = registraties;
        }
    }
}