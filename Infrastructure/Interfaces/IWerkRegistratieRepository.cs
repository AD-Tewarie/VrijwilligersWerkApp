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
        public List<WerkRegistratieDTO> GetWerkRegistraties();
        public WerkRegistratieDTO GetRegistratieOnId(int id);
        public WerkRegistratieDTO GetRegistratieOnWerkId(int checkId);
        public void AddWerkRegistratie(WerkRegistratieDTO registratieDTO);
        public bool VerwijderWerkRegistratie(int registratieId);

    }
}
