using Domain.Vrijwilligerswerk_Test;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ICategorieService
    {
        List<Categorie> HaalAlleCategorieënOp();
        List<Categorie> HaalCategorieënVoorWerkOp(int werkId);
        Categorie GetCategorieOpId(int id);


    }
}
