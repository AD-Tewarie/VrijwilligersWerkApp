using Application.Interfaces;
using Application.Mapper;
using Application.ViewModels;
using Domain.GebruikersTest.Interfaces;

namespace Application.Service
{
    public class GebruikersTestService : IGebruikersTestService
    {
        private readonly ITestBeheer testBeheer;
        private readonly ITestSessieBeheer sessieBeheer;

        public GebruikersTestService(ITestBeheer testBeheer, ITestSessieBeheer sessieBeheer)
        {
            this.testBeheer = testBeheer;
            this.sessieBeheer = sessieBeheer;
        }

        public GebruikersTestViewModel HaalVolgendeVraag(int gebruikerId)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            var categorieën = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();
            var totaalVragen = categorieën.Count + vragen.Count;

            if (sessie.HuidigeVraagNummer >= totaalVragen)
            {
                return null;
            }

            if (sessie.HuidigeVraagNummer < categorieën.Count)
            {
                var categorie = categorieën[sessie.HuidigeVraagNummer];
                var mapper = new CategorieViewMapper(sessie.HuidigeVraagNummer, totaalVragen);
                return mapper.MapNaarViewModel(categorie);
            }
            else
            {
                var vraagIndex = sessie.HuidigeVraagNummer - categorieën.Count;
                if (vraagIndex < vragen.Count)
                {
                    var vraag = vragen[vraagIndex];
                    var mapper = new GebruikersTestViewMapper(sessie.HuidigeVraagNummer, totaalVragen);
                    return mapper.MapNaarViewModel(vraag);
                }
            }

            return null;
        }

        public bool VerwerkAntwoord(int userId, int antwoord)
        {
            var sessie = sessieBeheer.HaalOp(userId);
            var categorieën = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();
            var totaalVragen = categorieën.Count + vragen.Count;

            if (sessie.HuidigeVraagNummer >= totaalVragen)
            {
                return true;
            }

            if (sessie.HuidigeVraagNummer < categorieën.Count)
            {
                var categorie = categorieën[sessie.HuidigeVraagNummer];
                sessie.VoegAntwoordToe(categorie.Id, antwoord);
            }
            else
            {
                var vraagIndex = sessie.HuidigeVraagNummer - categorieën.Count;
                var vraag = vragen[vraagIndex];
                sessie.VoegAntwoordToe(vraag.Id, antwoord);
            }

            sessieBeheer.Opslaan(userId, sessie);
            return sessie.HuidigeVraagNummer >= totaalVragen;
        }



        public bool HeeftBestaandeResultaten(int gebruikerId)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            var categorieën = testBeheer.HaalAlleCategorieënOp();
            var vragen = testBeheer.HaalAlleTestVragenOp();
            var totaalVragen = categorieën.Count + vragen.Count;

            return sessie.HuidigeVraagNummer == totaalVragen;
        }

        public void ResetTest(int gebruikerId)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            sessie.Reset();
            sessieBeheer.Opslaan(gebruikerId, sessie);
        }
    }
}
