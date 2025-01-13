using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Infrastructure.Interfaces;

namespace Domain.Vrijwilligerswerk_Test.ScoreStrategy
{
    public class StandaardScoreStrategy : IScoreStrategy
    {
        
        public Dictionary<Categorie, int> BerekenScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            IGebruikersTestRepository repository)
        {
            var scores = new Dictionary<Categorie, int>();

            foreach (var categorieId in affiniteiten.Keys)
            {
                var categorie = repository.GetCategorieOnId(categorieId);
                if (categorie == null) continue;

                
                var totaleScore = BerekenTotaleScoreVoorCategorie(categorieId, antwoorden, affiniteiten, repository);

                scores[Categorie.Maak(categorie.Id, categorie.Naam)] = totaleScore;
            }

            return scores;
        }

        // Helpermethode om de score voor een specifieke categorie te berekenen
        private int BerekenTotaleScoreVoorCategorie(
            int categorieId,
            Dictionary<int, int> antwoorden,
            Dictionary<int, int> affiniteiten,
            IGebruikersTestRepository repository)
        {
            int totaleScore = 0;

            foreach (var vraagId in antwoorden.Keys)
            {
                var vraag = repository.GetTestVraagOnId(vraagId);
                if (vraag?.CategorieId == categorieId)
                {
                   
                    totaleScore += antwoorden[vraagId] * affiniteiten[categorieId];
                }
            }

            return totaleScore;
        }


        public int BerekenWerkScore(
        VrijwilligersWerk werk,
        Dictionary<Categorie, int> scores,
        IGebruikersTestRepository repository)
        {
            var werkCategorieën = repository.GetWerkCategorieënByWerkId(werk.WerkId);
            var totaleScore = 0;
            

            foreach (var werkCategorie in werkCategorieën)
            {
                if (scores.TryGetValue(Categorie.Maak(werkCategorie.CategorieId, ""), out int score))
                {
                    totaleScore += score;
                }
            }

            return totaleScore;
        }
    }
}
