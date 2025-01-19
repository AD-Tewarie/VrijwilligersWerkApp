namespace Domain.Werk.Interfaces
{
    public interface IWerkCategorieRepository
    {
        List<string> HaalCategorieënOpVoorWerk(int werkId);
        void VoegCategorieToe(int werkId, int categorieId);
    }
}