using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVrijwilligersWerkService
    {
        List<VrijwilligersWerkViewModel> HaalAlleWerkenOp();
        bool RegistreerVoorWerk(int werkId, int gebruikerId);
        bool VerwijderRegistratie(int registratieId);

    }
}
