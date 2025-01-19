using Domain.GebruikersTest.Models;
using Domain.Werk.Models;
using Domain.Common.Interfaces.Repository;

namespace Domain.GebruikersTest.WerkScore;

public class StandaardWerkScoreService : IWerkScoreService
{
    private readonly IGebruikersTestRepository repository;

    public StandaardWerkScoreService(IGebruikersTestRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<WerkMetScore> BerekenScoresVoorWerkLijst(List<VrijwilligersWerk> werkLijst, Dictionary<Categorie, int> scores)
    {
        if (werkLijst == null)
            throw new ArgumentNullException(nameof(werkLijst));
        if (scores == null)
            throw new ArgumentNullException(nameof(scores));

        return werkLijst
            .Select(werk => 
            {
                var score = BerekenWerkScore(werk, scores).score;
                return new WerkMetScore(werk, score);
            })
            .Where(w => w.Score > 0)
            .OrderByDescending(w => w.Score)
            .ToList();
    }

    public (int score, int maximaleScore) BerekenWerkScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores)
    {
        if (werk == null)
            throw new ArgumentNullException(nameof(werk));
        if (scores == null)
            throw new ArgumentNullException(nameof(scores));

        var werkCategorieen = repository.GetWerkCategorieënByWerkId(werk.WerkId);
        if (!werkCategorieen.Any()) return (0, 100);

        var totaleScore = 0;
        var aantalMatchendeCategorieen = 0;

        foreach (var werkCategorie in werkCategorieen)
        {
            var categorie = repository.GetCategorieOnId(werkCategorie.CategorieId);
            if (categorie != null && scores.TryGetValue(categorie, out int score))
            {
                totaleScore += score;
                aantalMatchendeCategorieen++;
            }
        }

        // Bereken gemiddelde score als percentage
        var gemiddeldeScore = aantalMatchendeCategorieen > 0
            ? (int)Math.Round((double)totaleScore / aantalMatchendeCategorieen)
            : 0;

        return (gemiddeldeScore, 100); 
    }
}