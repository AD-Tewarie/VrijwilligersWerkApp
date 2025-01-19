using System.Collections.Generic;
using System.Linq;
using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Interfaces;

namespace Application.GebruikersTest.Mappers
{
    public class GebruikersTestMapper : IGebruikersTestMapper
    {
        private readonly ITestBeheer testBeheer;

        public GebruikersTestMapper(ITestBeheer testBeheer)
        {
            this.testBeheer = testBeheer;
        }

        public GebruikersTestViewModel MapNaarViewModel(TestSessie sessie, string huidigeVraagTekst, bool isKlaar)
        {
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
                alleVragen.Add(new TestVraagViewModel
                {
                    Id = vraag.Id,
                    Tekst = vraag.Tekst,
                    CategorieId = vraag.CategorieId,
                    VraagNummer = categorieen.Count + i + 1,
                    TotaalVragen = totaalVragen,
                    GekozenAntwoord = sessie.Antwoorden.ContainsKey(vraag.Id) ? sessie.Antwoorden[vraag.Id] : null
                });
            }

            return new GebruikersTestViewModel
            {
                GebruikerId = sessie.GebruikerId,
                HuidigeVraag = huidigeVraagTekst,
                IsVoltooid = sessie.IsVoltooid,
                VoortgangPercentage = (sessie.HuidigeStap * 100) / totaalVragen,
                IsLaatsteVraag = sessie.HuidigeStap == totaalVragen - 1,
                HuidigeStap = sessie.HuidigeStap,
                HeeftBestaandeResultaten = sessie.Antwoorden.Any() || sessie.Affiniteiten.Any(),
                IsTestNetVoltooid = isKlaar,
                Antwoorden = sessie.Antwoorden,
                Vragen = alleVragen
            };
        }

        public TestVraagViewModel MapNaarVraagViewModel(
            TestVraag vraag,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null)
        {
            return new TestVraagViewModel
            {
                Id = vraag.Id,
                Tekst = vraag.Tekst,
                CategorieId = vraag.CategorieId,
                VraagNummer = vraagNummer,
                TotaalVragen = totaalVragen,
                GekozenAntwoord = gekozenAntwoord
            };
        }

        public TestVraagViewModel MapNaarAffiniteitsVraagViewModel(
            Categorie categorie,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null)
        {
            return TestVraagViewModel.MaakVanCategorie(
                categorie.Id,
                categorie.Naam,
                vraagNummer,
                totaalVragen,
                gekozenAntwoord);
        }

        public GebruikersTestViewModel MapNaarTestViewModel(
            int gebruikerId,
            List<Categorie> categorieën,
            bool isAfgerond = false)
        {
            var totaalVragen = categorieën.Count;
            return new GebruikersTestViewModel
            {
                GebruikerId = gebruikerId,
                HeeftBestaandeResultaten = isAfgerond,
                Vragen = categorieën.Select((c, index) => 
                    MapNaarAffiniteitsVraagViewModel(c, index + 1, totaalVragen))
                    .ToList()
            };
        }

        public GebruikersTestViewModel MapNaarTestViewModelMetAntwoorden(
            int gebruikerId,
            List<Categorie> categorieën,
            Dictionary<int, string> antwoorden,
            bool isAfgerond = false)
        {
            var totaalVragen = categorieën.Count;
            var viewModel = new GebruikersTestViewModel
            {
                GebruikerId = gebruikerId,
                HeeftBestaandeResultaten = isAfgerond,
                Vragen = categorieën.Select((c, index) => 
                    MapNaarAffiniteitsVraagViewModel(
                        c, 
                        index + 1, 
                        totaalVragen,
                        antwoorden.ContainsKey(c.Id) && int.TryParse(antwoorden[c.Id], out int antwoord) ? antwoord : null))
                    .ToList()
            };

            foreach (var antwoord in antwoorden)
            {
                if (int.TryParse(antwoord.Value, out int waarde))
                {
                    viewModel.Antwoorden[antwoord.Key] = waarde;
                }
            }

            return viewModel;
        }

        public CategorieViewModel MapNaarViewModel(Categorie domainModel)
        {
            return new CategorieViewModel(
                domainModel.Id,
                domainModel.Naam
            );
        }

        public Categorie MapNaarDomainModel(CategorieViewModel viewModel)
        {
            throw new System.NotImplementedException(
                "Mapping van ViewModel naar Domain model is niet ondersteund voor Categorie"
            );
        }
    }
}