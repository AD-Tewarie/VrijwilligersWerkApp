namespace Domain.Models
{
    public class WerkRegistratie
    {


        public int RegistratieId { get; private set; }
        public VrijwilligersWerk VrijwilligersWerk { get; private set; }
        public User User { get; private set; }

        private WerkRegistratie(VrijwilligersWerk vrijwilligersWerk, User user)
        {
            ValideerRegistratie(vrijwilligersWerk, user);

            VrijwilligersWerk = vrijwilligersWerk;
            User = user;
          
        }

        public static WerkRegistratie MaakNieuw(VrijwilligersWerk vrijwilligersWerk, User user)
        {
            return new WerkRegistratie(vrijwilligersWerk, user);
        }

        public static WerkRegistratie LaadVanuitDatabase(int registratieId, VrijwilligersWerk vrijwilligersWerk, User user)
        {
            var registratie = new WerkRegistratie(vrijwilligersWerk, user);
            registratie.RegistratieId = registratieId;
            return registratie;
        }

        private static void ValideerRegistratie(VrijwilligersWerk vrijwilligersWerk, User user)
        {
            var fouten = new List<string>();

            if (vrijwilligersWerk == null)
                fouten.Add("Vrijwilligerswerk is verplicht.");

            if (user == null)
                fouten.Add("Gebruiker is verplicht.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

    }
}
