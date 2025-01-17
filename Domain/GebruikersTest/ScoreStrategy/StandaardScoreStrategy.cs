using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.ScoreStrategy.Interfaces;
using Domain.Werk.Models;

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
            var werkCategorieen = repository.GetWerkCategorieënByWerkId(werk.WerkId);
            var totaleScore = 0;
            var aantalMatchendeCategorieen = 0;

            foreach (var werkCategorie in werkCategorieen)
            {
                var categorie = repository.GetCategorieOnId(werkCategorie.CategorieId);
                if (categorie != null)
                {
                    var matchendeCategorie = scores.Keys.FirstOrDefault(c => c.Id == categorie.Id);
                    if (matchendeCategorie != null && scores.ContainsKey(matchendeCategorie))
                    {
                        totaleScore += scores[matchendeCategorie];
                        aantalMatchendeCategorieen++;
                    }
                }
            }

            return aantalMatchendeCategorieen > 0
                ? totaleScore / aantalMatchendeCategorieen
                : 0;
        }
    }
}
