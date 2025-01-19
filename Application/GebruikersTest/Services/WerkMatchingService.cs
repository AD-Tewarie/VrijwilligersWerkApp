using Application.GebruikersTest.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;
using Domain.Werk.Models;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Services;

namespace Application.GebruikersTest.Services
{
    public class WerkMatchingService : IWerkMatchingService
    {
        private readonly IVrijwilligersWerkRepository werkRepository;
        private readonly IGebruikersTestRepository testRepository;
        private readonly ITestBeheer testBeheer;
        private readonly IScoreStrategy scoreStrategy;

        public WerkMatchingService(
            IVrijwilligersWerkRepository werkRepository,
            IGebruikersTestRepository testRepository,
            ITestBeheer testBeheer)
        {
            this.werkRepository = werkRepository;
            this.testRepository = testRepository;
            this.testBeheer = testBeheer;
            this.scoreStrategy = new StandaardScoreStrategy();
        }

        public List<WerkMetScore> BerekenWerkMatches(List<VrijwilligersWerk> beschikbaarWerk, TestSessie sessie)
        {
            if (!sessie.IsVoltooid || !sessie.Affiniteiten.Any())
                return new List<WerkMetScore>();

            var vragen = testBeheer.HaalAlleTestVragenOp().ToDictionary(v => v.Id);
            var categorieën = testBeheer.HaalAlleCategorieënOp().ToDictionary(c => c.Id);
            var scores = scoreStrategy.BerekenScores(sessie.Affiniteiten, sessie.Antwoorden, vragen, categorieën);

            var werkMetScores = new List<WerkMetScore>();

            foreach (var werk in beschikbaarWerk)
            {
                var categorieIds = werkRepository.GetWerkCategorieënByWerkId(werk.WerkId);
                if (!categorieIds.Any())
                    continue;

                var werkCategorieën = categorieIds.Select(id => WerkCategorie.Maak(werk.WerkId, id)).ToList();
                var (score, maxScore) = scoreStrategy.BerekenWerkScore(werk, scores, werkCategorieën, categorieën);
                if (maxScore > 0)
                {
                    var totaleScore = (score * 100) / maxScore;
                    werkMetScores.Add(new WerkMetScore(werk, totaleScore));
                }
            }

            return werkMetScores;
        }
    }
}