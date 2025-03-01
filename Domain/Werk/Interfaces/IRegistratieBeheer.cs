﻿﻿using Domain.Werk.Models;

namespace Domain.Werk.Interfaces
{
    public interface IRegistratieBeheer
    {
        void RegistreerGebruikerVoorWerk(int userId, int werkId);
        void VerwijderRegistratie(int registratieId);
        List<WerkRegistratie> HaalRegistratiesOp();
        WerkRegistratie HaalRegistratieOp(int registratieId);
        int HaalAantalRegistratiesOp(int werkId);
        bool HeeftGebruikerRegistratie(int werkId, int gebruikerId);
        WerkRegistratie GetRegistratieByWerkAndUser(int werkId, int gebruikerId);
    }
}
