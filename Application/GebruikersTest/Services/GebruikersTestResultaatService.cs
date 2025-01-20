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
    public class GebruikersTestResultaatService : ITestResultaatService
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
            this.testSessieBeheer = testSessieBeheer ?? throw new ArgumentNullException(nameof(testSessieBeheer));
            this.testBeheer = testBeheer ?? throw new ArgumentNullException(nameof(testBeheer));
            this.werkAanbevelingService = werkAanbevelingService ?? throw new ArgumentNullException(nameof(werkAanbevelingService));
            this.werkRepository = werkRepository ?? throw new ArgumentNullException(nameof(werkRepository));
            this.scoreStrategy = scoreStrategy ?? throw new ArgumentNullException(nameof(scoreStrategy));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public GebruikersTestResultaatViewModel HaalResultatenOp(int gebruikerId, string presentatieType = "top", int? minimumScore = null)
        {
            // Haal sessie op en valideer
            var sessie = testSessieBeheer.HaalOp(gebruikerId);

            if (!sessie.IsVoltooid || !sessie.Affiniteiten.Any())
            {
                logger.LogWarning($"Ongeldige sessie voor gebruiker {gebruikerId}");
                return new GebruikersTestResultaatViewModel
                {
                    GebruikerId = gebruikerId,
                    HuidigePresentatieType = presentatieType
                };
            }

            // Haal benodigde data op
            var vragen = testBeheer.HaalAlleTestVragenOp().ToDictionary(v => v.Id);
            var categorieën = testBeheer.HaalAlleCategorieënOp().ToDictionary(c => c.Id);

            // Bereken scores voor alle categorieën
            var categorieScores = scoreStrategy.BerekenScores(
                sessie.Affiniteiten,
                sessie.Antwoorden,
                vragen,
                categorieën);

            // Haal alle beschikbare werk op en cache categorieën
            var alleWerk = werkRepository.GetVrijwilligersWerk();
            var werkScoreCache = new Dictionary<int, (int score, int maxScore)>();

            // Bereken scores voor al het werk
            var aanbevolenWerk = alleWerk
                .Select(werk =>
                {
                    var werkCategorieën = werkRepository
                        .GetWerkCategorieënByWerkId(werk.WerkId)
                        .Select(id => WerkCategorie.Maak(werk.WerkId, id))
                        .ToList();

                    if (!werkCategorieën.Any())
                    {
                        return null;
                    }

                    // Bereken totale score voor dit werk
                    var (score, maxScore) = scoreStrategy.BerekenWerkScore(
                        werk,
                        categorieScores,
                        werkCategorieën,
                        categorieën);

                    return score > 0 ? new WerkMetScore(werk, score) : null;
                })
                .Where(w => w != null)
                .OrderByDescending(w => w.Score)
                .ToList();

            // Filter op basis van presentatieType
            aanbevolenWerk = presentatieType.ToLower() switch
            {
                "top" => aanbevolenWerk.Take(5).ToList(),
                "minimum" => aanbevolenWerk.Where(w => w.Score >= (minimumScore ?? 50)).ToList(),
                "alle" => aanbevolenWerk.Where(w => w.Score >= 20).ToList(),
                _ => aanbevolenWerk
            };

            // Maak viewmodel met alle data
            return new GebruikersTestResultaatViewModel(
                gebruikerId,
                categorieScores,
                aanbevolenWerk,
                presentatieType)
            {
                CategorieScores = categorieScores.Select(s => new CategorieResultaat
                {
                    CategorieId = s.Key.Id,
                    CategorieNaam = s.Key.Naam,
                    Score = s.Value
                }).ToList()
            };
        }
    }
}