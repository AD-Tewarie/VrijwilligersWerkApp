namespace Domain.Models
{
    public class VrijwilligersWerk
    {


        public int WerkId { get; private set; }  
        public string Titel { get; private set; }
        public string Omschrijving { get; private set; }
        public int MaxCapaciteit { get; private set; }
        public int AantalRegistraties { get; private set; }

        private VrijwilligersWerk(string titel, string omschrijving, int maxCapaciteit)
        {
            WijzigDetails(titel, omschrijving, maxCapaciteit);
        }

        public static VrijwilligersWerk MaakNieuw(string titel, string omschrijving, int maxCapaciteit)
        {
            return new VrijwilligersWerk(titel, omschrijving, maxCapaciteit);
        }

        public static VrijwilligersWerk MaakMetId(int werkId, string titel, string omschrijving, int maxCapaciteit)
        {
            var werk = new VrijwilligersWerk(titel, omschrijving, maxCapaciteit);
            werk.WerkId = werkId;
            return werk;
        }

        public static VrijwilligersWerk LaadVanuitDatabase(int werkId, string titel, string omschrijving,
            int maxCapaciteit, int aantalRegistraties)
        {
            var werk = new VrijwilligersWerk(titel, omschrijving, maxCapaciteit);
            werk.WerkId = werkId;
            werk.AantalRegistraties = aantalRegistraties;
            return werk;
        }

        // validatie 

        private static void ValideerWerk(string titel, string omschrijving, int maxCapaciteit)
        {
            var fouten = new List<string>();

            if (string.IsNullOrWhiteSpace(titel))
                fouten.Add("Titel is verplicht.");

            if (string.IsNullOrWhiteSpace(omschrijving))
                fouten.Add("Omschrijving is verplicht.");

            if (maxCapaciteit <= 0)
                fouten.Add("Capaciteit moet groter zijn dan 0.");

            if (maxCapaciteit > 100)
                fouten.Add("Maximale capaciteit mag niet groter zijn dan 100.");

            if (fouten.Any())
            {
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
            }
        }


        // logic
        public void WijzigDetails(string titel, string omschrijving, int maxCapaciteit)
        {
            ValideerWerk(titel, omschrijving, maxCapaciteit);

            Titel = titel;
            Omschrijving = omschrijving;
            MaxCapaciteit = maxCapaciteit;
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
