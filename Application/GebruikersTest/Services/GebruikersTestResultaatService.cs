using System;
using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Services;
using Domain.GebruikersTest.WerkScore;
using Microsoft.Extensions.Logging;

namespace Application.GebruikersTest.Services
{
    public class GebruikersTestResultaatService : ITestResultaatService, IGebruikersTestResultaatService
    {
        private readonly ITestSessieBeheer testSessieBeheer;
        private readonly ITestBeheer testBeheer;
        private readonly IWerkAanbevelingService werkAanbevelingService;
        private readonly IVrijwilligersWerkRepository werkRepository;
        private readonly IScoreStrategy scoreStrategy;
        private readonly ILogger<GebruikersTestResultaatService> logger;

        public GebruikersTestResultaatService(
            ITestSessieBeheer testSessieBeheer,
            ITestBeheer testBeheer,
            IWerkAanbevelingService werkAanbevelingService,
            IVrijwilligersWerkRepository werkRepository,
            IScoreStrategy scoreStrategy,
            ILogger<GebruikersTestResultaatService> logger)
        {
            this.testSessieBeheer = testSessieBeheer;
            this.testBeheer = testBeheer;
            this.werkAanbevelingService = werkAanbevelingService;
            this.werkRepository = werkRepository;
            this.scoreStrategy = scoreStrategy;
            this.logger = logger;
        }

        public List<TestResultaatViewModel> HaalTestResultatenOp(int gebruikerId)
        {
            // Haal sessie op en valideer
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            logger.LogDebug($"Ophalen resultaten voor gebruiker {gebruikerId}. Sessie voltooid: {sessie.IsVoltooid}");

            if (!sessie.IsVoltooid || !sessie.Affiniteiten.Any())
            {
                logger.LogWarning($"Geen geldige sessie gevonden voor gebruiker {gebruikerId}");
                return new List<TestResultaatViewModel>();
            }

            // Haal benodigde data op
            var vragen = testBeheer.HaalAlleTestVragenOp().ToDictionary(v => v.Id);
            var categorieën = testBeheer.HaalAlleCategorieënOp().ToDictionary(c => c.Id);
            var alleWerk = werkRepository.GetVrijwilligersWerk();

            // Gebruik scoreStrategy voor score berekeningen
            var scores = scoreStrategy.BerekenScores(
                sessie.Affiniteiten,
                sessie.Antwoorden,
                vragen,
                categorieën);

            // Haal werkaanbevelingen op per categorie
            var resultaten = new List<TestResultaatViewModel>();
            foreach (var (categorie, score) in scores)
            {
                // Filter werk voor deze categorie
                var werkVoorCategorie = alleWerk.Where(werk => 
                    werkRepository.GetWerkCategorieënByWerkId(werk.WerkId).Contains(categorie.Id)).ToList();

                var aanbevelingen = werkVoorCategorie.Select(werk =>
                {
                    var werkCategorieën = werkRepository
                        .GetWerkCategorieënByWerkId(werk.WerkId)
                        .Select(id => WerkCategorie.Maak(werk.WerkId, id))
                        .ToList();

                    var (werkScore, _) = scoreStrategy.BerekenWerkScore(werk, scores, werkCategorieën, categorieën);

                    return new WerkAanbevelingViewModel(
                        werk.WerkId,
                        werk.Titel,
                        werk.Omschrijving,
                        werkScore,
                        werk.Locatie);
                }).ToList();

                resultaten.Add(new TestResultaatViewModel(
                    categorie.Id,
                    categorie.Naam,
                    score,
                    aanbevelingen));
            }

            return resultaten;
        }

        public GebruikersTestResultaatViewModel HaalResultatenOp(int gebruikerId, string presentatieType = "top", int? minimumScore = null)
        {
            // Haal sessie op en valideer
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            logger.LogDebug($"Ophalen resultaten voor gebruiker {gebruikerId}. Sessie voltooid: {sessie.IsVoltooid}");

            if (!sessie.IsVoltooid || !sessie.Affiniteiten.Any())
            {
                logger.LogWarning($"Geen geldige sessie gevonden voor gebruiker {gebruikerId}");
                return new GebruikersTestResultaatViewModel
                {
                    GebruikerId = gebruikerId,
                    HuidigePresentatieType = presentatieType
                };
            }

            // Haal benodigde data op
            var vragen = testBeheer.HaalAlleTestVragenOp().ToDictionary(v => v.Id);
            var categorieën = testBeheer.HaalAlleCategorieënOp().ToDictionary(c => c.Id);

            // Bereken scores voor alle categorieën (scores zijn al genormaliseerd door StandaardScoreStrategy)
            var categorieScores = scoreStrategy.BerekenScores(
                sessie.Affiniteiten,
                sessie.Antwoorden,
                vragen,
                categorieën);

            logger.LogDebug($"Scores berekend voor {categorieScores.Count} categorieën");

            // Cache voor werk scores om herberekening te voorkomen
            var werkScoreCache = new Dictionary<int, (int score, int maxScore)>();
            
            // Haal alle beschikbare werk op
            var alleWerk = werkRepository.GetVrijwilligersWerk();
            var werkCategorieënCache = new Dictionary<int, List<WerkCategorie>>();

            // Bereken scores voor al het werk één keer en cache de resultaten
            var aanbevolenWerk = alleWerk
                .Select(werk =>
                {
                    // Gebruik gecachede werkCategorieën of haal ze op
                    if (!werkCategorieënCache.TryGetValue(werk.WerkId, out var werkCategorieën))
                    {
                        werkCategorieën = werkRepository
                            .GetWerkCategorieënByWerkId(werk.WerkId)
                            .Select(id => WerkCategorie.Maak(werk.WerkId, id))
                            .ToList();
                        werkCategorieënCache[werk.WerkId] = werkCategorieën;
                    }

                    if (!werkCategorieën.Any()) return null;

                    // Gebruik gecachede score of bereken nieuwe
                    if (!werkScoreCache.TryGetValue(werk.WerkId, out var scoreResult))
                    {
                        scoreResult = scoreStrategy.BerekenWerkScore(
                            werk,
                            categorieScores,
                            werkCategorieën,
                            categorieën);
                        werkScoreCache[werk.WerkId] = scoreResult;
                    }

                    var (score, maxScore) = scoreResult;
                    
                    // Filter op basis van minimumScore als opgegeven
                    if (minimumScore.HasValue && score < minimumScore.Value)
                        return null;

                    if (score > 0)
                    {
                        logger.LogDebug($"Werk {werk.WerkId} score: {score}/{maxScore}");
                        return new WerkMetScore(werk, score);
                    }

                    return null;
                })
                .Where(w => w != null)
                .OrderByDescending(w => w.Score)
                .ToList();

            // Pas presentatieType filtering toe
            switch (presentatieType.ToLower())
            {
                case "top":
                    aanbevolenWerk = aanbevolenWerk.Take(5).ToList();
                    break;
                case "minimum":
                    aanbevolenWerk = aanbevolenWerk.Where(w => w.Score >= (minimumScore ?? 20)).ToList();
                    break;
                case "alle":
                    aanbevolenWerk = aanbevolenWerk.Where(w => w.Score >= 20).ToList();
                    break;
                default:
                    logger.LogWarning($"Onbekend presentatieType: {presentatieType}, gebruik standaard 'alle'");
                    aanbevolenWerk = aanbevolenWerk.Where(w => w.Score >= 20).ToList();
                    break;
            }

            return new GebruikersTestResultaatViewModel(
                gebruikerId,
                categorieScores,
                aanbevolenWerk,
                presentatieType);
        }
    }
}