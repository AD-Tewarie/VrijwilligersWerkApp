using System.Collections.Generic;
using System.Linq;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Interfaces;
using Domain.Werk.Models;

namespace Domain.GebruikersTest.Services
{
    public class StandaardScoreStrategy : IScoreStrategy
    {
        public Dictionary<Categorie, int> BerekenScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            Dictionary<int, TestVraag> vragen,
            Dictionary<int, Categorie> categorieën)
        {
            var scores = new Dictionary<Categorie, int>();

            foreach (var categorieId in categorieën.Keys)
            {
                if (!categorieën.TryGetValue(categorieId, out var categorie) ||
                    !affiniteiten.TryGetValue(categorieId, out var affiniteit))
                    continue;

                var categorieVragen = vragen.Values
                    .Where(v => v.CategorieId == categorieId)
                    .ToList();

                var totaleScore = 0;
                var aantalVragen = 0;

                foreach (var vraag in categorieVragen)
                {
                    if (antwoorden.ContainsKey(vraag.Id))
                    {
                        totaleScore += antwoorden[vraag.Id] * affiniteit;
                        aantalVragen++;
                    }
                }

                
                scores[categorie] = aantalVragen > 0 ? (totaleScore * 100) / (aantalVragen * 25) : 0;
            }

            return scores;
        }

        public (int score, int maximaleScore) BerekenWerkScore(
            VrijwilligersWerk werk,
            Dictionary<Categorie, int> scores,
            List<WerkCategorie> werkCategorieën,
            Dictionary<int, Categorie> categorieën)
        {
            var totaleScore = 0;
            var aantalMatchendeCategorieen = 0;

            foreach (var werkCategorie in werkCategorieën.Where(wc => wc.WerkId == werk.WerkId))
            {
                if (categorieën.TryGetValue(werkCategorie.CategorieId, out var categorie) &&
                    scores.ContainsKey(categorie))
                {
                    totaleScore += scores[categorie];
                    aantalMatchendeCategorieen++;
                }
            }

            var maximaleScore = aantalMatchendeCategorieen * 100;
            return (totaleScore, maximaleScore);
        }
    }
}