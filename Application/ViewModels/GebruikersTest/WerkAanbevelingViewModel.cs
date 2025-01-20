namespace Application.ViewModels.GebruikersTest
{
    public class WerkAanbevelingViewModel
    {
        public int WerkId { get; }
        public string WerkTitel { get; }
        public string WerkBeschrijving { get; }
        public int MatchPercentage { get; }
        public string WerkLocatie { get; }

        public WerkAanbevelingViewModel(
            int werkId,
            string werkTitel,
            string werkBeschrijving,
            int matchPercentage,
            string werkLocatie)
        {
            WerkId = werkId;
            WerkTitel = werkTitel;
            WerkBeschrijving = werkBeschrijving;
            MatchPercentage = matchPercentage;
            WerkLocatie = werkLocatie;
        }
    }
}