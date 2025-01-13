using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IVrijwilligersWerkRepository
    {
        List<VrijwilligersWerkDTO> GetVrijwilligersWerk();
        VrijwilligersWerkDTO GetWerkOnId(int id);
        void AddVrijwilligersWerk(VrijwilligersWerkDTO werkDTO);
        bool BewerkVrijwilligersWerk(VrijwilligersWerkDTO updatedWerk);
        bool VerwijderVrijwilligersWerk(int werkId);
        bool BewerkAantalRegistraties(int werkId, int wijziging);
        void VoegWerkCategorieToeAanNieuweWerk(int werkId, int categorieId);
    }
}
