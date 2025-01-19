using Domain.GebruikersTest.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ICategorieService
    {
        List<Categorie> HaalAlleCategorieënOp();
        List<Categorie> HaalCategorieënVoorWerkOp(int werkId);
        Categorie GetCategorieOpId(int id);


    }
}
