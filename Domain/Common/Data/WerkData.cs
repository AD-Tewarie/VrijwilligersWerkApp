using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Data
{
    public class WerkData
    {
        public int WerkId { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public int MaxCapaciteit { get; set; }
        public int AantalRegistraties { get; set; }

        public WerkData(
            int werkId,
            string titel,
            string omschrijving,
            int maxCapaciteit,
            int aantalRegistraties)
        {
            WerkId = werkId;
            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
            AantalRegistraties = aantalRegistraties;
        }
    }
}
