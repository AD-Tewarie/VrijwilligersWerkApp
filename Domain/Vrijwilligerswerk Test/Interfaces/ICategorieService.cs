namespace Domain.Vrijwilligerswerk_Test.Interfaces
{
    public interface ICategorieService
    {
        List<Categorie> HaalAlleCategorieënOp();
        Categorie GetCategorieOpId(int id);
    }
}
