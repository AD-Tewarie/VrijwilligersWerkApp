using Domain.Interfaces;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Infrastructure.Interfaces;
using System.Text.Json;
using VrijwilligersWerkApp.Extensions;
using VrijwilligersWerkApp.Helpers;

namespace VrijwilligersWerkApp.Services
{
    public class TestSessionService : ITestSessionService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITestBeheer testBeheer;
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IGebruikersTestRepository testRepository;
        private const string AFFINITEITEN_KEY = "Test_Affiniteiten";
        private const string ANTWOORDEN_KEY = "Test_Antwoorden";
        private const string HUIDIGE_STAP_KEY = "Test_HuidigeStap";
        private const string AANBEVOLEN_WERK_KEY = "TestAanbevolenWerk";

        public TestSessionService(
            IHttpContextAccessor httpContextAccessor,
            ITestBeheer testBeheer,
            IVrijwilligersWerkBeheer werkBeheer,
            IGebruikersTestRepository testRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.testBeheer = testBeheer;
            this.werkBeheer = werkBeheer;
            this.testRepository = testRepository;
        }

        public bool HeeftBestaandeResultaten()
        {
            var session = httpContextAccessor.HttpContext.Session;
            var scores = session.Get<Dictionary<int, int>>("SortedScores");
            var werkIds = session.Get<List<int>>("FilteredJobIds");
            return scores != null && werkIds != null && werkIds.Any();
        }

        public void ResetTest()
        {
            var session = httpContextAccessor.HttpContext.Session;
            session.Remove("SortedScores");
            session.Remove("FilteredJobIds");
            session.Remove(HUIDIGE_STAP_KEY);
            session.Remove(AFFINITEITEN_KEY);
            session.Remove(ANTWOORDEN_KEY);
        }

        public bool VerwerkAntwoord(int antwoord)
        {
            var session = httpContextAccessor.HttpContext.Session;
            var huidigeStap = session.GetInt32(HUIDIGE_STAP_KEY) ?? 0;
            var affiniteiten = session.Get<Dictionary<int, int>>(AFFINITEITEN_KEY) ?? new Dictionary<int, int>();
            var antwoorden = session.Get<Dictionary<int, int>>(ANTWOORDEN_KEY) ?? new Dictionary<int, int>();



            var categorieën = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();

            if (huidigeStap < categorieën.Count)
            {
                var categorie = categorieën[huidigeStap];
                affiniteiten[categorie.Id] = antwoord;
                session.Set(AFFINITEITEN_KEY, affiniteiten);
            }
            else
            {
                var vraagIndex = huidigeStap - categorieën.Count;
                if (vraagIndex < vragen.Count)
                {
                    var vraag = vragen[vraagIndex];
                    antwoorden[vraag.Id] = antwoord;
                    session.Set(ANTWOORDEN_KEY, antwoorden);
                }
            }

            huidigeStap++;
            session.SetInt32(HUIDIGE_STAP_KEY, huidigeStap);

            return huidigeStap >= (categorieën.Count + vragen.Count);
        }

        public (string vraagTekst, bool isKlaar) HaalVolgendeVraag()
        {
            var session = httpContextAccessor.HttpContext.Session;
            var huidigeStap = session.GetInt32(HUIDIGE_STAP_KEY) ?? 0;

            var categorieen = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();
            var totaalVragen = categorieen.Count + vragen.Count;

            if (huidigeStap >= totaalVragen)
            {
                return ("Test voltooid", true);
            }

            // Eerst affiniteitsvragen
            if (huidigeStap < categorieen.Count)
            {
                var categorie = categorieen[huidigeStap];
                return ($"Hoe belangrijk vind je {categorie.Naam}?", false);
            }
            else
            {
                // Daarna normale vragen
                var vraagIndex = huidigeStap - categorieen.Count;
                if (vraagIndex < vragen.Count)
                {
                    return (vragen[vraagIndex].Tekst, false);
                }
            }

            return ("Onverwachte fout", true);
        }

        private bool IsTestKlaar(int huidigeStap)
        {
            var totaalVragen = testBeheer.HaalAlleCategorieënOp().Count +
                              testBeheer.HaalAlleTestVragenOp().Count;
            var isKlaar = huidigeStap >= totaalVragen;

            return isKlaar;
        }

        public (Dictionary<Categorie, int> scores, List<WerkMetScore> werkMetScores) BerekenResultaten(string presentatieType = "top")
        {
            try
            {
                var session = httpContextAccessor.HttpContext.Session;
                var affiniteiten = session.Get<Dictionary<int, int>>(AFFINITEITEN_KEY);
                var antwoorden = session.Get<Dictionary<int, int>>(ANTWOORDEN_KEY);

                if (affiniteiten == null || antwoorden == null)
                {
                    throw new InvalidOperationException("Geen test resultaten beschikbaar.");
                }

                // Bereken scores per categorie
                var scores = testBeheer.BerekenTestScores(affiniteiten, antwoorden);
                var beschikbaarWerk = werkBeheer.BekijkAlleWerk();

                // Debug logging
                Console.WriteLine($"Aantal categorieën met scores: {scores.Count}");
                foreach (var score in scores)
                {
                    Console.WriteLine($"Categorie {score.Key.Id}: {score.Key.Naam} - Score: {score.Value}");
                }

                // Calculate scores for all work
                var werkMetScores = new List<WerkMetScore>();
                foreach (var werk in beschikbaarWerk)
                {
                    // Haal alle categorieën op voor dit werk
                    var werkCategorieen = testRepository.GetWerkCategorieënByWerkId(werk.WerkId);
                    Console.WriteLine($"Werk {werk.WerkId} ({werk.Titel}) heeft {werkCategorieen.Count} categorieën");

                    var totaleScore = 0;
                    var aantalMatchendeCategorieen = 0;

                    foreach (var werkCategorie in werkCategorieen)
                    {
                        // Zoek de bijbehorende categorie
                        var categorie = testBeheer.GetCategorieOpId(werkCategorie.CategorieId);
                        if (categorie != null)
                        {
                            // Zoek de score voor deze categorie
                            var matchendeCategorie = scores.Keys.FirstOrDefault(c => c.Id == categorie.Id);
                            if (matchendeCategorie != null && scores.ContainsKey(matchendeCategorie))
                            {
                                totaleScore += scores[matchendeCategorie];
                                aantalMatchendeCategorieen++;
                                Console.WriteLine($"Categorie {categorie.Id} gevonden voor werk {werk.WerkId}, score: {scores[matchendeCategorie]}");
                            }
                        }
                    }

                    // Bereken gemiddelde score als er matchende categorieën zijn
                    if (aantalMatchendeCategorieen > 0)
                    {
                        var gemiddeldeScore = totaleScore / aantalMatchendeCategorieen;
                        Console.WriteLine($"Werk {werk.WerkId} finale score: {gemiddeldeScore} (gebaseerd op {aantalMatchendeCategorieen} categorieën)");
                        werkMetScores.Add(new WerkMetScore(werk, gemiddeldeScore));
                    }
                    else
                    {
                        Console.WriteLine($"Geen matchende categorieën gevonden voor werk {werk.WerkId}");
                        werkMetScores.Add(new WerkMetScore(werk, 0));
                    }
                }

                // Sort and filter based on presentation type
                var gefilterdWerk = presentatieType.ToLower() switch
                {
                    "top" => werkMetScores.OrderByDescending(w => w.Score).Take(5).ToList(),
                    "minimum" => werkMetScores.Where(w => w.Score >= 50).OrderByDescending(w => w.Score).ToList(),
                    _ => werkMetScores.OrderByDescending(w => w.Score).ToList()
                };

                return (scores, gefilterdWerk);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BerekenResultaten: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }






        public List<int> GetWerkCategorieën(int werkId)
        {
            return testRepository.GetWerkCategorieënByWerkId(werkId)
                             .Select(cat => cat.CategorieId)
                             .ToList();
        }
    }
}
