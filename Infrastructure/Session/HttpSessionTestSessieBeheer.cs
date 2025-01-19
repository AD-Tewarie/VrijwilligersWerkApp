using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Infrastructure.Session.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Domain.Common.Interfaces.Repository;

namespace Infrastructure.Session
{
    public class HttpSessionTestSessieBeheer : ITestSessieBeheer
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IGebruikersTestRepository gebruikersTestRepository;
        private const string SESSIE_KEY_PREFIX = "TestSessie_";

        public HttpSessionTestSessieBeheer(
            IHttpContextAccessor httpContextAccessor,
            IGebruikersTestRepository gebruikersTestRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.gebruikersTestRepository = gebruikersTestRepository;
        }

        private string GetSessionKey(int gebruikerId) => $"{SESSIE_KEY_PREFIX}{gebruikerId}";

        public TestSessie MaakNieuweSessie(int gebruikerId)
        {
            var sessie = TestSessie.Start(gebruikerId);
            Opslaan(gebruikerId, sessie);
            return sessie;
        }

        public TestSessie HaalOp(int gebruikerId)
        {
            var sessieJson = httpContextAccessor.HttpContext?.Session.GetString(GetSessionKey(gebruikerId));
            if (string.IsNullOrEmpty(sessieJson))
            {
                return MaakNieuweSessie(gebruikerId);
            }

            try
            {
                var sessieData = JsonSerializer.Deserialize<TestSessieData>(sessieJson);
                return sessieData?.NaarDomeinModel() ?? MaakNieuweSessie(gebruikerId);
            }
            catch
            {
                return MaakNieuweSessie(gebruikerId);
            }
        }

        public void SlaAntwoordOp(int gebruikerId, int vraagId, int antwoord)
        {
            var sessie = HaalOp(gebruikerId);
            sessie.VoegAntwoordToe(vraagId, antwoord);
            sessie.VerhoogStap();

            var categorieen = gebruikersTestRepository.HaalAlleCategorieënOp();
            var vragen = gebruikersTestRepository.HaalAlleTestVragenOp();
            var totaalVragen = categorieen.Count + vragen.Count;

            if (sessie.HuidigeStap >= totaalVragen)
            {
                sessie.RondAf();
            }

            Opslaan(gebruikerId, sessie);
        }

        public void SlaAffiniteitOp(int gebruikerId, int categorieId, int score)
        {
            var sessie = HaalOp(gebruikerId);
            sessie.ZetAffiniteit(categorieId, score);
            sessie.VerhoogStap();

            var categorieen = gebruikersTestRepository.HaalAlleCategorieënOp();
            var vragen = gebruikersTestRepository.HaalAlleTestVragenOp();
            var totaalVragen = categorieen.Count + vragen.Count;

            if (sessie.HuidigeStap >= totaalVragen)
            {
                sessie.RondAf();
            }

            Opslaan(gebruikerId, sessie);
        }

        public bool IsVoltooid(int gebruikerId)
        {
            var sessie = HaalOp(gebruikerId);
            return sessie.IsVoltooid;
        }

        public void Reset(int gebruikerId)
        {
            Verwijder(gebruikerId);
            MaakNieuweSessie(gebruikerId);
        }

        public (string vraagTekst, bool isKlaar) HaalVolgendeVraag(TestSessie sessie)
        {
            var categorieen = gebruikersTestRepository.HaalAlleCategorieënOp();
            var vragen = gebruikersTestRepository.HaalAlleTestVragenOp();
            var totaalVragen = categorieen.Count + vragen.Count;

            if (sessie.HuidigeStap >= totaalVragen)
            {
                sessie.RondAf();
                Opslaan(sessie.GebruikerId, sessie);
                return ("Test voltooid", true);
            }

            if (sessie.HuidigeStap < categorieen.Count)
            {
                var categorie = categorieen[sessie.HuidigeStap];
                return ($"Hoe belangrijk vind je {categorie.Naam}?", false);
            }
            else
            {
                var vraagIndex = sessie.HuidigeStap - categorieen.Count;
                if (vraagIndex < vragen.Count)
                {
                    return (vragen[vraagIndex].Tekst, false);
                }
            }

            return ("Onverwachte fout", true);
        }

        public int BerekenVoortgang(TestSessie sessie)
        {
            var totaalVragen = gebruikersTestRepository.HaalAlleCategorieënOp().Count +
                              gebruikersTestRepository.HaalAlleTestVragenOp().Count;

            return (int)((double)sessie.HuidigeStap / totaalVragen * 100);
        }

        public void Opslaan(int gebruikerId, TestSessie sessie)
        {
            var sessieData = new TestSessieData(sessie);
            var sessieJson = JsonSerializer.Serialize(sessieData);
            httpContextAccessor.HttpContext?.Session.SetString(GetSessionKey(gebruikerId), sessieJson);
        }

        public void Verwijder(int gebruikerId)
        {
            httpContextAccessor.HttpContext?.Session.Remove(GetSessionKey(gebruikerId));
        }
    }
}