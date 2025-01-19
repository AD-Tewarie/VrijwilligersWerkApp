using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;
using Domain.Werk.Models;

namespace Application.GebruikersTest.Services
{
    public class GebruikersTestResultaatService : ITestResultaatService, IGebruikersTestResultaatService
    {
        private readonly ITestSessieBeheer testSessieBeheer;
        private readonly ITestBeheer testBeheer;
        private readonly IWerkAanbevelingService werkAanbevelingService;
        private readonly IVrijwilligersWerkRepository werkRepository;

        public GebruikersTestResultaatService(
            ITestSessieBeheer testSessieBeheer,
            ITestBeheer testBeheer,
            IWerkAanbevelingService werkAanbevelingService,
            IVrijwilligersWerkRepository werkRepository)
        {
            this.testSessieBeheer = testSessieBeheer;
            this.testBeheer = testBeheer;
            this.werkAanbevelingService = werkAanbevelingService;
            this.werkRepository = werkRepository;
        }

        public GebruikersTestResultaatViewModel HaalResultatenOp(int gebruikerId, string presentatieType = "top", int? minimumScore = null)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            if (!sessie.IsVoltooid)
            {
                return new GebruikersTestResultaatViewModel
                {
                    GebruikerId = gebruikerId,
                    HuidigePresentatieType = presentatieType
                };
            }

            var categorieen = testBeheer.HaalAlleCategorieënOp();
            var categorieScores = new Dictionary<Categorie, int>();

            foreach (var categorie in categorieen)
            {
                if (sessie.Affiniteiten.TryGetValue(categorie.Id, out int score))
                {
                    categorieScores[categorie] = score * 20; // Convert 1-5 scale to percentage
                }
            }

            var aanbevelingen = werkAanbevelingService.HaalAanbevelingenOp(gebruikerId, presentatieType, minimumScore ?? 50);
            var aanbevolenWerk = new List<WerkMetScore>();

            foreach (var aanbeveling in aanbevelingen)
            {
                var werk = werkRepository.GetWerkOnId(aanbeveling.WerkId);
                if (werk != null)
                {
                    aanbevolenWerk.Add(new WerkMetScore(werk, aanbeveling.MatchPercentage));
                }
            }

            return new GebruikersTestResultaatViewModel(
                gebruikerId,
                categorieScores,
                aanbevolenWerk,
                presentatieType);
        }

        public List<TestResultaatViewModel> HaalTestResultatenOp(int gebruikerId)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            if (!sessie.IsVoltooid)
            {
                return new List<TestResultaatViewModel>();
            }

            var categorieen = testBeheer.HaalAlleCategorieënOp();
            var aanbevelingen = werkAanbevelingService.HaalAanbevelingenOp(gebruikerId, "alle", 50);

            return categorieen
                .Where(c => sessie.Affiniteiten.ContainsKey(c.Id))
                .Select(c => new TestResultaatViewModel(
                    c.Id,
                    c.Naam,
                    sessie.Affiniteiten[c.Id] * 20, 
                    new List<WerkAanbevelingViewModel>() 
                ))
                .ToList();
        }
    }
}