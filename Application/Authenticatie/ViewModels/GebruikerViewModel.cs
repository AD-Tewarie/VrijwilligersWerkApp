namespace Application.Authenticatie.ViewModels
{
    public class GebruikerViewModel
    {
        public int UserId { get; private set; }
        public string Naam { get; private set; }
        public string AchterNaam { get; private set; }
        public string Email { get; private set; }

        public GebruikerViewModel(
            int userId,
            string naam,
            string achterNaam,
            string email)
        {
            if (string.IsNullOrWhiteSpace(naam))
                throw new ArgumentException("Naam is verplicht.");
            if (string.IsNullOrWhiteSpace(achterNaam))
                throw new ArgumentException("Achternaam is verplicht.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is verplicht.");

            UserId = userId;
            Naam = naam;
            AchterNaam = achterNaam;
            Email = email;
        }
    }
}