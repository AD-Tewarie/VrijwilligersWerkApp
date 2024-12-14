using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IVrijwilligersWerkRepository
    {
        public List<VrijwilligersWerkDTO> GetVrijwilligersWerk();
        public VrijwilligersWerkDTO GetWerkOnId(int id);
        public void AddVrijwilligersWerk(VrijwilligersWerkDTO werkDTO);
        public bool BewerkVrijwilligersWerk(VrijwilligersWerkDTO updatedWerk);
        public bool BewerkAantalRegistraties(int werkId, int wijziging);
        public bool VerwijderVrijwilligersWerk(int werkId);
        public void VoegWerkCategorieToeAanNieuweWerk(int werkId, int categorieId);
    }
}
