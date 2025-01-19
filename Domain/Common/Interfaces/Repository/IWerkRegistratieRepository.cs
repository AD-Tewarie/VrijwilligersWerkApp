using Domain.Werk.Models;

namespace Domain.Common.Interfaces.Repository
{
    public interface IWerkRegistratieRepository
    {
        List<WerkRegistratie> GetWerkRegistraties();
        WerkRegistratie? GetRegistratieOnId(int id);
        void AddWerkRegistratie(WerkRegistratie registratieDTO);
        bool VerwijderWerkRegistratie(int registratieId);
        WerkRegistratie? GetRegistratieOnWerkId(int werkId);
        int GetRegistratieCountForWerk(int werkId);
        bool HeeftGebruikerRegistratie(int werkId, int gebruikerId);
    }
}
