using System;
using System.Collections.Generic;
using System.Linq;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Services;
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

                var totaleScore = BerekenTotaleScoreVoorCategorie(
                    categorieId,
                    antwoorden,
                    affiniteiten,
                    vragen);

                // Normaliseer score naar 0-100 schaal
                scores[categorie] = Math.Min(100, Math.Max(0, totaleScore));
            }

            return scores;
        }

        private int BerekenTotaleScoreVoorCategorie(
            int categorieId,
            Dictionary<int, int> antwoorden,
            Dictionary<int, int> affiniteiten,
            Dictionary<int, TestVraag> vragen)
        {
            // Haal affiniteit voor deze categorie op
            var affiniteit = affiniteiten[categorieId];
            
            // Filter vragen voor deze categorie
            var categorieVragen = vragen.Values
                .Where(v => v.CategorieId == categorieId)
                .ToList();

            if (!categorieVragen.Any())
                return 0;

            // Verzamel alle antwoorden voor deze categorie
            var antwoordenInCategorie = categorieVragen
                .Where(v => antwoorden.ContainsKey(v.Id))
                .Select(v => antwoorden[v.Id])
                .ToList();

            if (!antwoordenInCategorie.Any())
                return 0;

            // Bereken gewogen gemiddelde score (0-5 schaal)
            var gemiddeldeScore = antwoordenInCategorie.Average();
            
            // Pas affiniteitsweging toe (1-5 schaal)
            var gewogenScore = gemiddeldeScore * (affiniteit / 3.0); // Normaliseer affiniteit (3 is gemiddeld)
            
            // Converteer naar 0-100 schaal
            var basisScore = (int)Math.Round((gewogenScore / 5.0) * 100);
            
            // Controleer op consistentie voor kleine bonus/malus
            var standaardDeviatie = Math.Sqrt(
                antwoordenInCategorie.Average(a => Math.Pow(a - gemiddeldeScore, 2))
            );
            
            var consistentieBonus = 0;
            if (standaardDeviatie <= 0.5) // Zeer consistent
            {
                if (gemiddeldeScore >= 4)
                {
                    consistentieBonus = 10; // 10% bonus voor consistent hoge scores
                }
                else if (gemiddeldeScore <= 2)
                {
                    consistentieBonus = -5; // 5% malus voor consistent lage scores
                }
            }
            
            // Bereken eindscore met consistentiebonus
            var eindScore = basisScore + consistentieBonus;
            
            // Zorg dat score tussen 0 en 100 blijft
            return Math.Min(100, Math.Max(0, eindScore));
        }

        public (int score, int maximaleScore) BerekenWerkScore(
            VrijwilligersWerk werk,
            Dictionary<Categorie, int> scores,
            List<WerkCategorie> werkCategorieën,
            Dictionary<int, Categorie> categorieën)
        {
            // Bereken totale score voor dit werk
            var totaleScore = 0;
            var aantalMatchendeCategorieen = 0;

            // Tel scores op voor elke categorie die bij dit werk hoort
            foreach (var werkCategorie in werkCategorieën.Where(wc => wc.WerkId == werk.WerkId))
            {
                if (categorieën.TryGetValue(werkCategorie.CategorieId, out var categorie) &&
                    scores.ContainsKey(categorie))
                {
                    totaleScore += scores[categorie];
                    aantalMatchendeCategorieen++;
                }
            }

            // Bereken maximale score (100 punten per categorie)
            var maximaleScore = aantalMatchendeCategorieen * 100;

            return (totaleScore, maximaleScore);
        }
    }
}