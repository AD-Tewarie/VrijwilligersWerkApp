namespace Application.GebruikersTest.ViewModels
{
    public class TestResultaatViewModel
    {
        public int CategorieId { get; private set; }
        public string CategorieNaam { get; private set; }
        public int Score { get; private set; }
        public List<WerkAanbevelingViewModel> Aanbevelingen { get; private set; }

        public TestResultaatViewModel(
            int categorieId,
            string categorieNaam,
            int score,
            List<WerkAanbevelingViewModel> aanbevelingen)
        {
            if (string.IsNullOrWhiteSpace(categorieNaam))
                throw new ArgumentException("CategorieNaam is verplicht.");
            if (score < 0 || score > 100)
                throw new ArgumentException("Score moet tussen 0 en 100 liggen.");
            if (aanbevelingen == null)
                throw new ArgumentException("Aanbevelingen mag niet null zijn.");

            CategorieId = categorieId;
            CategorieNaam = categorieNaam;
            Score = score;
            Aanbevelingen = aanbevelingen;
        }

        public void VoegAanbevelingToe(WerkAanbevelingViewModel aanbeveling)
        {
            if (aanbeveling == null)
                throw new ArgumentNullException(nameof(aanbeveling));

            Aanbevelingen.Add(aanbeveling);
        }

        public void VerwijderAanbeveling(int werkId)
        {
            var aanbeveling = Aanbevelingen.FirstOrDefault(a => a.WerkId == werkId);
            if (aanbeveling != null)
            {
                Aanbevelingen.Remove(aanbeveling);
            }
        }
    }
}