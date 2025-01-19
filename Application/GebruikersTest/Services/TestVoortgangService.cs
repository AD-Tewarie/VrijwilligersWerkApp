using System.Linq;
using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Application.GebruikersTest.Mappers;
using Domain.GebruikersTest.Interfaces;

namespace Application.GebruikersTest.Services
{
    public class TestVoortgangService : ITestVoortgangService
    {
        private readonly ITestSessieBeheer testSessieBeheer;
        private readonly ITestBeheer testBeheer;
        private readonly ITestVraagMapper vraagMapper;

        public TestVoortgangService(
            ITestSessieBeheer testSessieBeheer,
            ITestBeheer testBeheer,
            ITestVraagMapper vraagMapper)
        {
            this.testSessieBeheer = testSessieBeheer;
            this.testBeheer = testBeheer;
            this.vraagMapper = vraagMapper;
        }

        public GebruikersTestViewModel MaakNieuweTest(int gebruikerId)
        {
            var sessie = testSessieBeheer.MaakNieuweSessie(gebruikerId);
            return MaakViewModel(sessie);
        }

        public bool HeeftGebruikerActieveTest(int gebruikerId)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            return sessie != null && !sessie.IsVoltooid && (sessie.Antwoorden.Any() || sessie.Affiniteiten.Any());
        }

        public bool BestaatTest(int gebruikerId)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            return sessie != null && sessie.IsVoltooid;
        }

        public GebruikersTestViewModel HaalTestOp(int gebruikerId)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            if (sessie == null)
            {
                return MaakNieuweTest(gebruikerId);
            }
            return MaakViewModel(sessie);
        }

        public bool BeantwoordVraag(int gebruikerId, int vraagId, int antwoord)
        {
            var sessie = testSessieBeheer.HaalOp(gebruikerId);
            var categorieen = testBeheer.HaalAlleCategorieënOp().ToList();

            // Bepaal of het een affiniteitsvraag of reguliere vraag is
            if (sessie.HuidigeStap < categorieen.Count)
            {
                testSessieBeheer.SlaAffiniteitOp(gebruikerId, vraagId, antwoord);
            }
            else
            {
                testSessieBeheer.SlaAntwoordOp(gebruikerId, vraagId, antwoord);
            }

            // Haal de bijgewerkte sessie op
            var bijgewerkteSessie = testSessieBeheer.HaalOp(gebruikerId);
            return bijgewerkteSessie.IsVoltooid;
        }

        public void ResetTest(int gebruikerId)
        {
            testSessieBeheer.Reset(gebruikerId);
        }

        private GebruikersTestViewModel MaakViewModel(Domain.GebruikersTest.Models.TestSessie sessie)
        {
            var (vraagTekst, isKlaar) = testSessieBeheer.HaalVolgendeVraag(sessie);
            var categorieen = testBeheer.HaalAlleCategorieënOp().ToList();
            var vragen = testBeheer.HaalAlleTestVragenOp().ToList();
            var totaalVragen = categorieen.Count + vragen.Count;

            var alleVragen = new List<TestVraagViewModel>();

            // Voeg eerst de affiniteitsvragen toe
            for (int i = 0; i < categorieen.Count; i++)
            {
                var categorie = categorieen[i];
                alleVragen.Add(TestVraagViewModel.MaakVanCategorie(
                    categorie.Id,
                    categorie.Naam,
                    i + 1,
                    totaalVragen,
                    sessie.Affiniteiten.ContainsKey(categorie.Id) ? sessie.Affiniteiten[categorie.Id] : null));
            }

            // Voeg daarna de reguliere vragen toe
            for (int i = 0; i < vragen.Count; i++)
            {
                var vraag = vragen[i];
                alleVragen.Add(vraagMapper.MapNaarViewModel(
                    vraag,
                    categorieen.Count + i + 1,
                    totaalVragen,
                    sessie.Antwoorden.ContainsKey(vraag.Id) ? sessie.Antwoorden[vraag.Id] : null));
            }

            return new GebruikersTestViewModel
            {
                GebruikerId = sessie.GebruikerId,
                HuidigeVraag = vraagTekst,
                IsVoltooid = sessie.IsVoltooid,
                VoortgangPercentage = (sessie.HuidigeStap * 100) / totaalVragen,
                IsLaatsteVraag = sessie.HuidigeStap == totaalVragen - 1,
                HuidigeStap = sessie.HuidigeStap,
                HeeftBestaandeResultaten = sessie.IsVoltooid,
                IsTestNetVoltooid = isKlaar || sessie.IsVoltooid,
                Antwoorden = sessie.Antwoorden,
                Vragen = alleVragen
            };
        }
    }
}