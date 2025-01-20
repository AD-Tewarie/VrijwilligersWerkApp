using System.Collections.Generic;

namespace Application.ViewModels.GebruikersTest
{
    public class TestResultaatViewModel
    {
        public int CategorieId { get; }
        public string CategorieNaam { get; }
        public int Score { get; }
        public List<WerkAanbevelingViewModel> Aanbevelingen { get; }

        public TestResultaatViewModel(
            int categorieId,
            string categorieNaam,
            int score,
            List<WerkAanbevelingViewModel> aanbevelingen)
        {
            CategorieId = categorieId;
            CategorieNaam = categorieNaam;
            Score = score;
            Aanbevelingen = aanbevelingen ?? new List<WerkAanbevelingViewModel>();
        }
    }
}