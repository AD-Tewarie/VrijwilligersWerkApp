using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IWerkRegistratieRepository
    {
        List<WerkRegistratieDTO> GetWerkRegistraties();
        WerkRegistratieDTO GetRegistratieOnId(int id);
        void AddWerkRegistratie(WerkRegistratieDTO registratieDTO);
        bool VerwijderWerkRegistratie(int registratieId);
        WerkRegistratieDTO GetRegistratieOnWerkId(int werkId);
        int GetRegistratieCountForWerk(int werkId);


    }
}
