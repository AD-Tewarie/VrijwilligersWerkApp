using Application.Interfaces;
using Application.ViewModels;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.ScoreStrategy.Interfaces;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Domain.Werk.Interfaces;

namespace Application.Service
{
    public class GebruikersTestResultaatService : IGebruikersTestResultaatService
    {
        private readonly ITestBeheer testBeheer;
        private readonly ITestSessieBeheer sessieBeheer;
        private readonly IScoreStrategy scoreStrategy;
        private readonly IGebruikersTestRepository testRepository;
        private readonly IWerkPresentatieService presentatieService;
        private readonly IVrijwilligersWerkBeheer werkBeheer;

        public GebruikersTestResultaatService(
            ITestBeheer testBeheer,
            ITestSessieBeheer sessieBeheer,
            IScoreStrategy scoreStrategy,
            IGebruikersTestRepository testRepository,
            IWerkPresentatieService presentatieService,
            IVrijwilligersWerkBeheer werkBeheer)
        {
            this.testBeheer = testBeheer ?? throw new ArgumentNullException(nameof(testBeheer));
            this.sessieBeheer = sessieBeheer ?? throw new ArgumentNullException(nameof(sessieBeheer));
            this.scoreStrategy = scoreStrategy ?? throw new ArgumentNullException(nameof(scoreStrategy));
            this.testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            this.presentatieService = presentatieService ?? throw new ArgumentNullException(nameof(presentatieService));
            this.werkBeheer = werkBeheer ?? throw new ArgumentNullException(nameof (werkBeheer));
        }

        public GebruikersTestResultaatViewModel HaalResultaatOp(int userId, string presentatieType = "top")
        {
            var sessie = sessieBeheer.HaalOp(userId);
            if (!HeeftCompleteTest(sessie))
            {
                return null;
            }

            var scores = BerekenScores(sessie);
            var werkMetScores = BerekenWerkScores(scores);
            var gefilterdWerk = presentatieService.FilterWerkOpPresentatieType(
                werkMetScores,
                presentatieType);

            return new GebruikersTestResultaatViewModel
            {
                GesorteerdeScores = scores,
                AanbevolenWerk = gefilterdWerk,
                HuidigePresentatieType = presentatieType
            };
        }

        private bool HeeftCompleteTest(TestSessie sessie)
        {
            var categorieën = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();
            return sessie.HuidigeVraagNummer >= (categorieën.Count + vragen.Count);
        }

        private Dictionary<Categorie, int> BerekenScores(TestSessie sessie)
        {
            return scoreStrategy.BerekenScores(
                sessie.Affiniteiten,
                sessie.Antwoorden,
                testRepository);
        }

        private List<WerkMetScore> BerekenWerkScores(Dictionary<Categorie, int> scores)
        {
            var beschikbaarWerk = werkBeheer.BekijkAlleWerk();
            var werkMetScores = new List<WerkMetScore>();

            foreach (var werk in beschikbaarWerk)
            {
                var score = scoreStrategy.BerekenWerkScore(werk, scores, testRepository);
                werkMetScores.Add(new WerkMetScore(werk, score));
            }

            return werkMetScores;
        }
    }
}
