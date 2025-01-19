namespace Application.Werk.ViewModels
{
    public class WerkDetailsViewModel
    {
        public int WerkId { get; private set; }
        public string Titel { get; private set; }
        public string Omschrijving { get; private set; }
        public int MaxCapaciteit { get; private set; }
        public int AantalRegistraties { get; private set; }
        public string Locatie { get; private set; }
        public bool IsVolzet => AantalRegistraties >= MaxCapaciteit;

        public WerkDetailsViewModel(
            int werkId,
            string titel,
            string omschrijving,
            int maxCapaciteit,
            int aantalRegistraties,
            string locatie)
        {
            WerkId = werkId;
            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
            AantalRegistraties = aantalRegistraties;
            Locatie = locatie;
        }
    }
}