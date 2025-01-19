using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Models;

namespace Application.GebruikersTest.Mappers
{
    public interface ITestVraagMapper
    {
        TestVraagViewModel MapNaarViewModel(TestVraag vraag, int vraagNummer, int totaalVragen, int? gekozenAntwoord = null);
        TestVraagViewModel MapNaarAffiniteitsViewModel(Categorie categorie, int vraagNummer, int totaalVragen, int? gekozenAntwoord = null);
    }
}