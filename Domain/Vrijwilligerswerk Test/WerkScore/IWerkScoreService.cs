using Domain.Models;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;

namespace Domain.Vrijwilligerswerk_Test.WerkScore
{
    public interface IWerkScoreService
    {
        List<WerkMetScore> BerekenScoresVoorWerkLijst(List<VrijwilligersWerk> werkLijst, Dictionary<Categorie, int> scores);
        int BerekenWerkScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores);
    }
}