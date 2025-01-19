using System;
using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Services
{
    public class WerkAanbevelingService : IWerkAanbevelingService
    {
        private readonly ITestSessieBeheer testSessieBeheer;
        private readonly IWerkVerzamelService werkVerzamelService;
        private readonly IWerkMatchingService werkMatchingService;
        private readonly IWerkPresentatieFilterService werkPresentatieFilterService;
        private readonly IWerkAanbevelingMapper werkAanbevelingMapper;
        private const int STANDAARD_MINIMUM_SCORE = 50;

        public WerkAanbevelingService(
            ITestSessieBeheer testSessieBeheer,
            IWerkVerzamelService werkVerzamelService,
            IWerkMatchingService werkMatchingService,
            IWerkPresentatieFilterService werkPresentatieFilterService,
            IWerkAanbevelingMapper werkAanbevelingMapper)
        {
            this.testSessieBeheer = testSessieBeheer ?? throw new ArgumentNullException(nameof(testSessieBeheer));
            this.werkVerzamelService = werkVerzamelService ?? throw new ArgumentNullException(nameof(werkVerzamelService));
            this.werkMatchingService = werkMatchingService ?? throw new ArgumentNullException(nameof(werkMatchingService));
            this.werkPresentatieFilterService = werkPresentatieFilterService ?? throw new ArgumentNullException(nameof(werkPresentatieFilterService));
            this.werkAanbevelingMapper = werkAanbevelingMapper ?? throw new ArgumentNullException(nameof(werkAanbevelingMapper));
        }

        public List<WerkAanbevelingViewModel> HaalAanbevelingenOp(int gebruikerId, string presentatieType, int minimumScore = STANDAARD_MINIMUM_SCORE)
        {
            // Haal test sessie op via domain service
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            if (sessie == null || !sessie.IsVoltooid)
                return new List<WerkAanbevelingViewModel>();

            // Verzamel beschikbaar werk
            var beschikbaarWerk = werkVerzamelService.VerzamelBeschikbaarWerk();

            // Bereken matches met domain services via WerkMatchingService
            var werkMetScores = werkMatchingService.BerekenWerkMatches(beschikbaarWerk, sessie);

            // Filter op presentatie type met domain service via WerkPresentatieFilterService
            var gefilterdWerk = werkPresentatieFilterService.FilterOpPresentatieType(werkMetScores, presentatieType, minimumScore);

            // Converteer naar view models voor de UI
            return gefilterdWerk
                .Select(werk => werkAanbevelingMapper.MapNaarViewModel(werk, werk.Score, presentatieType))
                .ToList();
        }
    }
}