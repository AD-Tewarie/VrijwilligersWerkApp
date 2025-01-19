using Application.Common;
using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Models;
using System.Collections.Generic;

namespace Application.GebruikersTest.Mappers
{
    public interface IGebruikersTestMapper : IViewModelMapper<CategorieViewModel, Categorie>
    {
        GebruikersTestViewModel MapNaarViewModel(TestSessie sessie, string huidigeVraagTekst, bool isKlaar);

        TestVraagViewModel MapNaarVraagViewModel(
            TestVraag vraag,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null);

        TestVraagViewModel MapNaarAffiniteitsVraagViewModel(
            Categorie categorie,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null);

        GebruikersTestViewModel MapNaarTestViewModel(
            int gebruikerId,
            List<Categorie> categorieën,
            bool isAfgerond = false);

        GebruikersTestViewModel MapNaarTestViewModelMetAntwoorden(
            int gebruikerId,
            List<Categorie> categorieën,
            Dictionary<int, string> antwoorden,
            bool isAfgerond = false);
    }
}