﻿namespace Domain.Common.Data
{
    public class WerkData
    {
        public int WerkId { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public int MaxCapaciteit { get; set; }
        public string Locatie { get; set; }
        public WerkData(
            int werkId,
            string titel,
            string omschrijving,
            int maxCapaciteit,
            string locatie)
        {
            WerkId = werkId;
            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
            Locatie = locatie;
        }
    }
}
