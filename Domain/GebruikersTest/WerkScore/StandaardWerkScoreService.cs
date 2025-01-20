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

        // Bereken scores voor alle werk items
        var werkMetScores = werkLijst
            .Select(werk =>
            {
                var (score, maxScore) = BerekenWerkScore(werk, scores);
                // Alleen filteren op volledig irrelevante matches (score = 0)
                return new WerkMetScore(werk, score);
            })
            .OrderByDescending(w => w.Score)
            .ToList();

        return werkMetScores;
    }

    public (int score, int maximaleScore) BerekenWerkScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores)
    {
        if (werk == null)
            throw new ArgumentNullException(nameof(werk));
        if (scores == null)
            throw new ArgumentNullException(nameof(scores));

        var werkCategorieen = repository.GetWerkCategorieënByWerkId(werk.WerkId);
        
        // Als er geen categorieën zijn, kan er geen match zijn
        if (!werkCategorieen.Any())
            return (0, 100);

        var totaleGewogenScore = 0.0;
        var totaalGewicht = 0.0;
        var maximaleScorePerCategorie = 100;

        foreach (var werkCategorie in werkCategorieen)
        {
            var categorie = repository.GetCategorieOnId(werkCategorie.CategorieId);
            if (categorie != null && scores.TryGetValue(categorie, out int categorieScore))
            {
                // Bepaal gewicht op basis van categorie prioriteit (kan later uitgebreid worden)
                var gewicht = 1.0;
                
                // Voeg gewogen score toe
                totaleGewogenScore += categorieScore * gewicht;
                totaalGewicht += gewicht;
            }
        }

        // Als er geen matches zijn gevonden
        if (totaalGewicht == 0)
            return (0, maximaleScorePerCategorie);

        // Bereken eindscore als gewogen gemiddelde
        var eindScore = (int)Math.Round(totaleGewogenScore / totaalGewicht);
        
        // Zorg dat score tussen 0 en maximaleScorePerCategorie blijft
        eindScore = Math.Min(maximaleScorePerCategorie, Math.Max(0, eindScore));

        return (eindScore, maximaleScorePerCategorie);
    }
}