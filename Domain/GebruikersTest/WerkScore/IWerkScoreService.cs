using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Domain.Werk.Models;

namespace Domain.Vrijwilligerswerk_Test.WerkScore
{
    public interface IWerkScoreService
    {
        List<WerkMetScore> BerekenScoresVoorWerkLijst(List<VrijwilligersWerk> werkLijst, Dictionary<Categorie, int> scores);
        int BerekenWerkScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores);
    }
}