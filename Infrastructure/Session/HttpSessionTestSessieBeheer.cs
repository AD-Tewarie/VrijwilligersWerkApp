using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Domain.Common.Interfaces.Repository;
using Infrastructure.Session.Extensions;

namespace Infrastructure.Session
{
    public class HttpSessionTestSessieBeheer : ITestSessieBeheer
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IGebruikersTestRepository gebruikersTestRepository;
        private readonly ILogger<HttpSessionTestSessieBeheer> logger;

        private const string AFFINITEITEN_KEY = "Test_Affiniteiten_{0}";
        private const string ANTWOORDEN_KEY = "Test_Antwoorden_{0}";
        private const string HUIDIGE_STAP_KEY = "Test_HuidigeStap_{0}";
        private const string IS_VOLTOOID_KEY = "Test_IsVoltooid_{0}";

        public HttpSessionTestSessieBeheer(
            IHttpContextAccessor httpContextAccessor,
            IGebruikersTestRepository gebruikersTestRepository,
            ILogger<HttpSessionTestSessieBeheer> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.gebruikersTestRepository = gebruikersTestRepository;
            this.logger = logger;
        }

        private string GetKey(string key, int gebruikerId) => string.Format(key, gebruikerId);

        public TestSessie MaakNieuweSessie(int gebruikerId)
        {
            var sessie = TestSessie.Start(gebruikerId);
            Opslaan(gebruikerId, sessie);
            return sessie;
        }

        public TestSessie HaalOp(int gebruikerId)
        {
            var session = httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                logger.LogWarning("Geen HTTP context beschikbaar voor gebruiker {GebruikerId}", gebruikerId);
                return MaakNieuweSessie(gebruikerId);
            }

            try
            {
                var affiniteiten = session.Get<Dictionary<int, int>>(GetKey(AFFINITEITEN_KEY, gebruikerId)) ?? new Dictionary<int, int>();
                var antwoorden = session.Get<Dictionary<int, int>>(GetKey(ANTWOORDEN_KEY, gebruikerId)) ?? new Dictionary<int, int>();
                var huidigeStap = session.GetInt32(GetKey(HUIDIGE_STAP_KEY, gebruikerId)) ?? 0;
                var isVoltooid = session.GetString(GetKey(IS_VOLTOOID_KEY, gebruikerId)) == "true";

                var sessie = TestSessie.Start(gebruikerId);

                foreach (var affiniteit in affiniteiten)
                {
                    sessie.ZetAffiniteit(affiniteit.Key, affiniteit.Value);
                }

                foreach (var antwoord in antwoorden)
                {
                    sessie.VoegAntwoordToe(antwoord.Key, antwoord.Value);
                }

                for (int i = 0; i < huidigeStap; i++)
                {
                    sessie.VerhoogStap();
                }

                if (isVoltooid)
                {
                    sessie.RondAf();
                }

                return sessie;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij ophalen sessie voor gebruiker {GebruikerId}", gebruikerId);
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
            var session = httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                logger.LogWarning("Geen HTTP context beschikbaar voor opslaan sessie van gebruiker {GebruikerId}", gebruikerId);
                return;
            }

            try
            {
                session.Set(GetKey(AFFINITEITEN_KEY, gebruikerId), sessie.Affiniteiten);
                session.Set(GetKey(ANTWOORDEN_KEY, gebruikerId), sessie.Antwoorden);
                session.SetInt32(GetKey(HUIDIGE_STAP_KEY, gebruikerId), sessie.HuidigeStap);
                session.SetString(GetKey(IS_VOLTOOID_KEY, gebruikerId), sessie.IsVoltooid.ToString().ToLower());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij opslaan sessie voor gebruiker {GebruikerId}", gebruikerId);
            }
        }

        public void Verwijder(int gebruikerId)
        {
            var session = httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                logger.LogWarning("Geen HTTP context beschikbaar voor verwijderen sessie van gebruiker {GebruikerId}", gebruikerId);
                return;
            }

            session.Remove(GetKey(AFFINITEITEN_KEY, gebruikerId));
            session.Remove(GetKey(ANTWOORDEN_KEY, gebruikerId));
            session.Remove(GetKey(HUIDIGE_STAP_KEY, gebruikerId));
            session.Remove(GetKey(IS_VOLTOOID_KEY, gebruikerId));
        }
    }
}