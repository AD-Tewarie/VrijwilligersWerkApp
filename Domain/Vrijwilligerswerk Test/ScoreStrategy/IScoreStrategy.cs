using Domain.Models;
using Infrastructure.Interfaces;

namespace Domain.Vrijwilligerswerk_Test.Interfaces
{
    public interface IScoreStrategy
    {
        Dictionary<Categorie, int> BerekenScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            IGebruikersTestRepository repository);

        int BerekenWerkScore(
            VrijwilligersWerk werk,
            Dictionary<Categorie, int> scores,
            IGebruikersTestRepository repository);
    }
}
