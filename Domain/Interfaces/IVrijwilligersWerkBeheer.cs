﻿using Domain.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IVrijwilligersWerkBeheer
    {
        void VoegWerkToe(VrijwilligersWerk werk, int categorieId);

        List<VrijwilligersWerk> BekijkAlleWerk();
        void VerwijderWerk(int werkId);
        void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving);

        VrijwilligersWerk HaalwerkOpID(int id);

        
    }


}
