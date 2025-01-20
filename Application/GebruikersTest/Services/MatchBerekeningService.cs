using System;
using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;
using Domain.GebruikersTest.Services;
using Domain.Common.Interfaces.Repository;
using VrijwilligersWerkModel = Domain.Werk.Models.VrijwilligersWerk;

namespace Application.GebruikersTest.Services
{
    public class MatchBerekeningService : IMatchBerekeningService
    {
        private readonly IWerkScoreService werkScoreService;
        private readonly ICategorieService categorieService;
        private readonly StandaardScoreStrategy scoreStrategy;
        private readonly IGebruikersTestRepository testRepository;

        public MatchBerekeningService(
            IWerkScoreService werkScoreService,
            ICategorieService categorieService,
            StandaardScoreStrategy scoreStrategy,
            IGebruikersTestRepository testRepository)
        {
            this.werkScoreService = werkScoreService ?? throw new ArgumentNullException(nameof(werkScoreService));
            this.categorieService = categorieService ?? throw new ArgumentNullException(nameof(categorieService));
            this.scoreStrategy = scoreStrategy ?? throw new ArgumentNullException(nameof(scoreStrategy));
            this.testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
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
            // Haal alle benodigde data op
            var vragen = testRepository.HaalAlleTestVragenOp()
                .ToDictionary(v => v.Id);
            var categorieën = testRepository.HaalAlleCategorieënOp()
                .ToDictionary(c => c.Id);

            // Converteer IReadOnlyDictionary naar Dictionary voor de scoreStrategy
            var affiniteitenDict = new Dictionary<int, int>(affiniteiten);
            var antwoordenDict = new Dictionary<int, int>(affiniteiten);

            // Bereken scores met de scoreStrategy
            var scores = scoreStrategy.BerekenScores(
                affiniteitenDict,
                antwoordenDict,
                vragen,
                categorieën);

            // Normaliseer scores naar percentages
            var maxScore = 100;
            return scores.ToDictionary(
                kvp => kvp.Key,
                kvp => Math.Min(maxScore, Math.Max(0, kvp.Value))
            );
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