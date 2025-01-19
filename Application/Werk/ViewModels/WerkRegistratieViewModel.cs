namespace Application.Werk.ViewModels
{
    public class WerkRegistratieViewModel
    {
        public int RegistratieId { get; private set; }
        public int WerkId { get; private set; }
        public int GebruikerId { get; private set; }
        public string WerkTitel { get; private set; }
        public string WerkLocatie { get; private set; }

        public WerkRegistratieViewModel(
            int registratieId,
            int werkId,
            int gebruikerId,
            string werkTitel,
            string werkLocatie)
        {
            RegistratieId = registratieId;
            WerkId = werkId;
            GebruikerId = gebruikerId;
            WerkTitel = werkTitel;
            WerkLocatie = werkLocatie;
        }
    }
}