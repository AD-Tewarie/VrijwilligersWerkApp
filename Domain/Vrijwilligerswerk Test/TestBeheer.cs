using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Domain.Vrijwilligerswerk_Test.Mapper;
using Domain.Vrijwilligerswerk_Test.Models;
using Domain.Vrijwilligerswerk_Test.WerkScore;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using Infrastructure.Interfaces;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestBeheer : ITestBeheer
    {
        private readonly IWerkPresentatieService werkPresentatieService;
        private readonly IWerkScoreService scoreService;
        private readonly ICategorieService categorieService;
        private readonly ITestVraagService testVraagService;
        private readonly IGebruikersTestRepository repository;
        private readonly IMapper<Categorie, CategorieDTO> mapper; 

        public TestBeheer(
            IWerkPresentatieService werkPresentatieService,
            IWerkScoreService scoreService,
            ICategorieService categorieService,
            ITestVraagService testVraagService,
            IGebruikersTestRepository repository)
        {
            this.werkPresentatieService = werkPresentatieService;
            this.scoreService = scoreService;
            this.categorieService = categorieService;
            this.testVraagService = testVraagService;
            this.repository = repository;
           
        }

        public Dictionary<Categorie, int> BerekenTestScores(
    Dictionary<int, int> affiniteiten,
    Dictionary<int, int> antwoorden)
        {
            var scores = new Dictionary<Categorie, int>();

            foreach (var categorieId in affiniteiten.Keys)
            {
                var categorie = categorieService.GetCategorieOpId(categorieId);
                if (categorie == null) continue;

                // Bereken score voor deze categorie
                int categorieScore = 0;
                var vragenVoorCategorie = testVraagService.HaalAlleTestVragenOp()
                    .Where(v => v.CategorieId == categorieId);

                foreach (var vraag in vragenVoorCategorie)
                {
                    if (antwoorden.ContainsKey(vraag.Id))
                    {
                        // Vermenigvuldig antwoord met affiniteit voor gewogen score
                        categorieScore += antwoorden[vraag.Id] * affiniteiten[categorieId];
                    }
                }

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
            // Calculate category scores using TestResultaat or a helper method
            var scores = BerekenTestScores(affiniteiten, antwoorden);

            // Enrich jobs with scores
            var werkMetScores = beschikbaarWerk.Select(werk =>
            {
                var werkCategorieen = repository.GetWerkCategorieënByWerkId(werk.WerkId);
                var totaleScore = werkCategorieen.Sum(cat =>
                {
                    var categorie = categorieService.GetCategorieOpId(cat.CategorieId);
                    return scores.ContainsKey(categorie) ? scores[categorie] : 0;
                });

                return new
                {
                    Werk = werk,
                    Score = totaleScore
                };
            }).ToList();

            // Sort and filter based on the presentation type
            var filteredWerk = presentatieType.ToLower() switch
            {
                "top" => werkMetScores.OrderByDescending(w => w.Score).Take(5),
                "minimum" => werkMetScores.Where(w => w.Score >= 50).OrderByDescending(w => w.Score),
                "alles" => werkMetScores.OrderByDescending(w => w.Score),
                _ => throw new ArgumentException($"Onbekend presentatie type: {presentatieType}")
            };

            // Return only the jobs
            return filteredWerk.Select(w => w.Werk).ToList();
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
            return  categorieService.GetCategorieOpId(id);
        }
    }
}

