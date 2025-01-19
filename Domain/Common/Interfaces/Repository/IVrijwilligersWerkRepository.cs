using Domain.Werk.Models;

namespace Domain.Common.Interfaces.Repository
{
    public interface IVrijwilligersWerkRepository
    {
        List<VrijwilligersWerk> GetVrijwilligersWerk();
        VrijwilligersWerk? GetWerkOnId(int id);
        int AddVrijwilligersWerk(VrijwilligersWerk werk);
        bool BewerkVrijwilligersWerk(VrijwilligersWerk updatedWerk);
        bool VerwijderVrijwilligersWerk(int werkId);
        bool BewerkAantalRegistraties(int werkId, int wijziging);
        void VoegWerkCategorieToeAanNieuweWerk(int werkId, int categorieId);
        List<int> GetWerkCategorieënByWerkId(int werkId);
    }
}
