﻿using Domain.Common.Data;
using Domain.Common.Exceptions;

namespace Domain.Werk.Models
{
    public class VrijwilligersWerk
    {
        public int WerkId { get; private set; }
        public string Titel { get; private set; }
        public string Omschrijving { get; private set; }
        public int MaxCapaciteit { get; private set; }
        public int AantalRegistraties { get; private set; }
        public string Locatie { get; private set; }

        private VrijwilligersWerk(string titel, string omschrijving, int maxCapaciteit, string locatie)
        {
            ValideerWerk(titel, omschrijving, maxCapaciteit, locatie);
            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
            Locatie = locatie;
            AantalRegistraties = 0;
        }

        public static VrijwilligersWerk MaakNieuw(string titel, string omschrijving, int maxCapaciteit, string locatie)
        {
            return new VrijwilligersWerk(titel, omschrijving, maxCapaciteit, locatie);
        }

        public static VrijwilligersWerk LaadVanuitData(WerkData data)
        {
            var werk = new VrijwilligersWerk(data.Titel, data.Omschrijving, data.MaxCapaciteit, data.Locatie)
            {
                WerkId = data.WerkId,
                AantalRegistraties = data.AantalRegistraties
            };
            return werk;
        }

        public WerkData NaarData()
        {
            return new WerkData(
                WerkId,
                Titel,
                Omschrijving,
                MaxCapaciteit,
                AantalRegistraties,
                Locatie
            );
        }

        public void WijzigDetails(string titel, string omschrijving, int maxCapaciteit, string locatie)
        {
            ValideerWerk(titel, omschrijving, maxCapaciteit, locatie);
            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
            Locatie = locatie;
        }

        private static void ValideerWerk(string titel, string omschrijving, int maxCapaciteit, string locatie)
        {
            var fouten = new Dictionary<string, ICollection<string>>();

            if (string.IsNullOrWhiteSpace(titel))
            {
                fouten["Titel"] = new List<string> { "Titel is verplicht." };
            }

            if (string.IsNullOrWhiteSpace(omschrijving))
            {
                fouten["Omschrijving"] = new List<string> { "Omschrijving is verplicht." };
            }

            if (string.IsNullOrWhiteSpace(locatie))
            {
                fouten["Locatie"] = new List<string> { "Locatie is verplicht." };
            }

            var capaciteitFouten = new List<string>();
            if (maxCapaciteit <= 0)
            {
                capaciteitFouten.Add("Capaciteit moet groter zijn dan 0.");
            }
            if (maxCapaciteit > 100)
            {
                capaciteitFouten.Add("Maximale capaciteit mag niet groter zijn dan 100.");
            }
            if (capaciteitFouten.Any())
            {
                fouten["MaxCapaciteit"] = capaciteitFouten;
            }

            if (fouten.Any())
            {
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
            }
        }

        public void VerhoogAantalRegistraties()
        {
            if (AantalRegistraties >= MaxCapaciteit)
                throw new InvalidOperationException("Maximum capaciteit is bereikt.");

            AantalRegistraties++;
        }

        public void VerlaagAantalRegistraties()
        {
            if (AantalRegistraties <= 0)
                throw new InvalidOperationException("Aantal registraties kan niet negatief worden.");

            AantalRegistraties--;
        }
    }
}
