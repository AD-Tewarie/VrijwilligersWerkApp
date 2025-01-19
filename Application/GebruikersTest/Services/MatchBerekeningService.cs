using System;
using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;
using VrijwilligersWerkModel = Domain.Werk.Models.VrijwilligersWerk;

namespace Application.GebruikersTest.Services
{
    public class MatchBerekeningService : IMatchBerekeningService
    {
        private readonly IWerkScoreService werkScoreService;
        private readonly ICategorieService categorieService;

        public MatchBerekeningService(
            IWerkScoreService werkScoreService,
            Domain.GebruikersTest.Interfaces.ICategorieService categorieService)
        {
            this.werkScoreService = werkScoreService ?? throw new ArgumentNullException(nameof(werkScoreService));
            this.categorieService = categorieService ?? throw new ArgumentNullException(nameof(categorieService));
        }

        public int BerekenMatchPercentage(VrijwilligersWerkModel werk, TestSessie sessie)
        {
            if (werk == null)
                throw new ArgumentNullException(nameof(werk));
            if (sessie == null)
                throw new ArgumentNullException(nameof(sessie));
            if (!sessie.Antwoorden.Any())
                throw new InvalidOperationException("Kan geen match berekenen voor een test zonder antwoorden.");

            var scores = ConverteerAffiniteiten(sessie.Affiniteiten);
            var (score, maxScore) = werkScoreService.BerekenWerkScore(werk, scores);
            return BerekenPercentage(score, maxScore);
        }

        public Dictionary<VrijwilligersWerkModel, int> BerekenMatchPercentages(List<VrijwilligersWerkModel> werken, TestSessie sessie)
        {
            if (werken == null)
                throw new ArgumentNullException(nameof(werken));
            if (sessie == null)
                throw new ArgumentNullException(nameof(sessie));
            if (!sessie.Antwoorden.Any())
                throw new InvalidOperationException("Kan geen matches berekenen voor een test zonder antwoorden.");

            var scores = ConverteerAffiniteiten(sessie.Affiniteiten);
            var resultaten = new Dictionary<VrijwilligersWerkModel, int>();
            foreach (var werk in werken)
            {
                var (score, maxScore) = werkScoreService.BerekenWerkScore(werk, scores);
                resultaten.Add(werk, BerekenPercentage(score, maxScore));
            }

            return resultaten;
        }

        private Dictionary<Categorie, int> ConverteerAffiniteiten(IReadOnlyDictionary<int, int> affiniteiten)
        {
            var scores = new Dictionary<Categorie, int>();
            foreach (var affiniteit in affiniteiten)
            {
                var categorie = categorieService.GetCategorieOpId(affiniteit.Key);
                if (categorie != null)
                {
                    scores[categorie] = affiniteit.Value;
                }
            }
            return scores;
        }

        private int BerekenPercentage(int score, int maxScore)
        {
            if (maxScore == 0)
                return 0;

            var percentage = (int)Math.Round((double)score / maxScore * 100);
            return Math.Min(100, Math.Max(0, percentage)); // Zorg dat het percentage tussen 0 en 100 ligt
        }
    }
}