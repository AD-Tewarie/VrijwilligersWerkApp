namespace Application.GebruikersTest.ViewModels
{
    public class WerkAanbevelingViewModel
    {
        public int WerkId { get; private set; }
        public string Titel { get; private set; }
        public string Omschrijving { get; private set; }
        public int MatchPercentage { get; private set; }
        public string PresentatieType { get; private set; }

        public WerkAanbevelingViewModel(
            int werkId,
            string titel,
            string omschrijving,
            int matchPercentage,
            string presentatieType)
        {
            if (string.IsNullOrWhiteSpace(titel))
                throw new ArgumentException("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(omschrijving))
                throw new ArgumentException("Omschrijving is verplicht.");
            if (string.IsNullOrWhiteSpace(presentatieType))
                throw new ArgumentException("PresentatieType is verplicht.");
            if (matchPercentage < 0 || matchPercentage > 100)
                throw new ArgumentException("MatchPercentage moet tussen 0 en 100 liggen.");

            WerkId = werkId;
            Titel = titel;
            Omschrijving = omschrijving;
            MatchPercentage = matchPercentage;
            PresentatieType = presentatieType;
        }
    }
}