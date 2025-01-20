using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Services;
using Domain.GebruikersTest.WerkScore;
using Domain.Werk.Models;
using Microsoft.Extensions.Logging;

namespace Application.GebruikersTest.Services
{
    public class WerkMatchingService : IWerkMatchingService
    {
        private readonly IVrijwilligersWerkRepository werkRepository;
        private readonly IGebruikersTestRepository testRepository;
        private readonly ITestBeheer testBeheer;
        private readonly IScoreStrategy scoreStrategy;
        private readonly ILogger<WerkMatchingService> logger;

        public WerkMatchingService(
            IVrijwilligersWerkRepository werkRepository,
            IGebruikersTestRepository testRepository,
            ITestBeheer testBeheer,
            IScoreStrategy scoreStrategy,
            ILogger<WerkMatchingService> logger)
        {
            this.werkRepository = werkRepository;
            this.testRepository = testRepository;
            this.testBeheer = testBeheer;
            this.scoreStrategy = scoreStrategy;
            this.logger = logger;
        }

        public List<WerkMetScore> BerekenWerkMatches(List<VrijwilligersWerk> beschikbaarWerk, TestSessie sessie)
        {
            // Controleer of de sessie geldig is en affiniteiten bevat
            if (!sessie.IsVoltooid || !sessie.Affiniteiten.Any())
            {
                logger.LogWarning($"Sessie niet voltooid of geen affiniteiten voor gebruiker {sessie.GebruikerId}");
                return new List<WerkMetScore>();
            }

            // Haal test gegevens op
            var vragen = testBeheer.HaalAlleTestVragenOp().ToDictionary(v => v.Id);
            var categorieën = testBeheer.HaalAlleCategorieënOp().ToDictionary(c => c.Id);
            
            // Bereken scores voor alle categorieën
            var scores = scoreStrategy.BerekenScores(sessie.Affiniteiten, sessie.Antwoorden, vragen, categorieën);
            logger.LogDebug($"Scores berekend voor {scores.Count} categorieën");

            var werkMetScores = new List<WerkMetScore>();

            // Bereken matches voor elk werk
            foreach (var werk in beschikbaarWerk)
            {
                // Haal categorieën op voor dit werk
                var categorieIds = werkRepository.GetWerkCategorieënByWerkId(werk.WerkId);
                if (!categorieIds.Any())
                {
                    logger.LogDebug($"Werk {werk.WerkId} heeft geen categorieën");
                    continue;
                }

                // Maak werk categorieën aan en bereken score
                var werkCategorieën = categorieIds.Select(id => WerkCategorie.Maak(werk.WerkId, id)).ToList();
                var (score, maxScore) = scoreStrategy.BerekenWerkScore(werk, scores, werkCategorieën, categorieën);
                
                werkMetScores.Add(new WerkMetScore(werk, score));
                logger.LogDebug($"Werk {werk.WerkId} score: {score}/{maxScore}");
            }

            return werkMetScores;
        }
    }
}