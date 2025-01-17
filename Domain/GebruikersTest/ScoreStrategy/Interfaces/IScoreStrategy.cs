using Domain.Common.Interfaces.Repository;
using Domain.Vrijwilligerswerk_Test;
using Domain.Werk.Models;

namespace Domain.GebruikersTest.ScoreStrategy.Interfaces
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
