using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;

namespace VrijwilligersWerkApp.Services
{
    public interface ITestSessionService
    {
        bool HeeftBestaandeResultaten();
        void ResetTest();
        bool VerwerkAntwoord(int antwoord);
        (string vraagTekst, bool isKlaar) HaalVolgendeVraag();
        (Dictionary<Categorie, int> scores, List<WerkMetScore> werkMetScores) BerekenResultaten(string presentatieType = "top");
        List<int> GetWerkCategorieën(int werkId);
    }
}
