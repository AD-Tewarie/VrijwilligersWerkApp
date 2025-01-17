using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.Vrijwilligerswerk_Test.Models;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Domain.Vrijwilligerswerk_Test.WerkScore;
using Domain.Werk.Models;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestBeheer : ITestBeheer
    {
        private readonly IWerkPresentatieService werkPresentatieService;
        private readonly IWerkScoreService scoreService;
        private readonly ICategorieService categorieService;
        private readonly ITestVraagService testVraagService;
        private readonly IGebruikersTestRepository repository;

        public TestBeheer(
            IWerkPresentatieService werkPresentatieService,
            IWerkScoreService scoreService,
            ICategorieService categorieService,
            ITestVraagService testVraagService,
            IGebruikersTestRepository repository)
        {
            this.werkPresentatieService = werkPresentatieService ?? throw new ArgumentNullException(nameof(werkPresentatieService));
            this.scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            this.categorieService = categorieService ?? throw new ArgumentNullException(nameof(categorieService));
            this.testVraagService = testVraagService ?? throw new ArgumentNullException(nameof(testVraagService));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Dictionary<Categorie, int> BerekenTestScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden)
        {
            ValideerTestInput(affiniteiten, antwoorden);
            var scores = new Dictionary<Categorie, int>();

            foreach (var categorieId in affiniteiten.Keys)
            {
                var categorie = categorieService.GetCategorieOpId(categorieId);
                if (categorie == null) continue;

                var categorieScore = BerekenCategorieScore(categorie, affiniteiten[categorieId], antwoorden);
                scores[categorie] = categorieScore;
            }

            return scores;
        }

        public List<VrijwilligersWerk> ZoekGeschiktWerk(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            List<VrijwilligersWerk> beschikbaarWerk,
            string presentatieType)
        {
            ValideerZoekParameters(affiniteiten, antwoorden, beschikbaarWerk, presentatieType);

            var scores = BerekenTestScores(affiniteiten, antwoorden);
            var werkMetScores = BerekenWerkScores(beschikbaarWerk, scores);

            return FilterWerkOpPresentatieType(werkMetScores, presentatieType)
                .Select(w => w.Werk)
                .ToList();
        }

        public List<Categorie> HaalAlleCategorieënOp()
        {
            return categorieService.HaalAlleCategorieënOp();
        }

        public List<TestVraag> HaalAlleTestVragenOp()
        {
            return testVraagService.HaalAlleTestVragenOp();
        }

        public Categorie GetCategorieOpId(int id)
        {
            ValideerCategorieId(id);
            return categorieService.GetCategorieOpId(id);
        }

        private int BerekenCategorieScore(Categorie categorie, int affiniteit, Dictionary<int, int> antwoorden)
        {
            var vragenVoorCategorie = testVraagService.HaalAlleTestVragenOp()
                .Where(v => v.CategorieId == categorie.Id);

            return vragenVoorCategorie.Sum(vraag =>
                antwoorden.ContainsKey(vraag.Id) ? antwoorden[vraag.Id] * affiniteit : 0);
        }

        private List<WerkMetScore> BerekenWerkScores(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores)
        {
            return werkLijst.Select(werk =>
            {
                var werkScore = scoreService.BerekenWerkScore(werk, scores);
                return new WerkMetScore(werk, werkScore);
            }).ToList();
        }

        private List<WerkMetScore> FilterWerkOpPresentatieType(
            List<WerkMetScore> werkMetScores,
            string presentatieType)
        {
            return werkPresentatieService.FilterWerkOpPresentatieType(werkMetScores, presentatieType);
        }

        private void ValideerTestInput(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden)
        {
            if (affiniteiten == null || !affiniteiten.Any())
                throw new ArgumentException("Affiniteiten mogen niet leeg zijn");

            if (antwoorden == null || !antwoorden.Any())
                throw new ArgumentException("Antwoorden mogen niet leeg zijn");
        }

        private void ValideerCategorieId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Categorie ID moet groter zijn dan 0.");
        }

        private void ValideerZoekParameters(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            List<VrijwilligersWerk> beschikbaarWerk,
            string presentatieType)
        {
            ValideerTestInput(affiniteiten, antwoorden);

            if (beschikbaarWerk == null || !beschikbaarWerk.Any())
                throw new ArgumentException("Beschikbaar werk mag niet leeg zijn");

            if (string.IsNullOrWhiteSpace(presentatieType))
                throw new ArgumentException("Presentatie type mag niet leeg zijn");
        }
    }

}

