using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Interfaces;

namespace Application.GebruikersTest.Mappers
{
    public class TestVraagMapper : ITestVraagMapper
    {
        private readonly ICategorieService categorieService;

        public TestVraagMapper(ICategorieService categorieService)
        {
            this.categorieService = categorieService;
        }

        public TestVraagViewModel MapNaarViewModel(
            TestVraag vraag,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null)
        {
            var categorie = categorieService.GetCategorieOpId(vraag.CategorieId);
            return new TestVraagViewModel
            {
                Id = vraag.Id,
                Tekst = vraag.Tekst,
                CategorieId = vraag.CategorieId,
                CategorieName = categorie.Naam,
                VraagNummer = vraagNummer,
                TotaalVragen = totaalVragen,
                GekozenAntwoord = gekozenAntwoord
            };
        }

        public TestVraagViewModel MapNaarAffiniteitsViewModel(
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
    }
}