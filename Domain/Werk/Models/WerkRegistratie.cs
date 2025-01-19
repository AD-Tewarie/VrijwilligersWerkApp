﻿using Domain.Common.Exceptions;
using Domain.Gebruikers.Models;

namespace Domain.Werk.Models
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

        public static WerkRegistratie Laad(int registratieId, VrijwilligersWerk vrijwilligersWerk, User user)
        {
            var registratie = new WerkRegistratie(vrijwilligersWerk, user);
            registratie.RegistratieId = registratieId;
            return registratie;
        }

        private static void ValideerRegistratie(VrijwilligersWerk vrijwilligersWerk, User user)
        {
            var fouten = new Dictionary<string, ICollection<string>>();

            if (vrijwilligersWerk == null)
            {
                fouten["VrijwilligersWerk"] = new List<string> { "Vrijwilligerswerk is verplicht." };
            }

            if (user == null)
            {
                fouten["User"] = new List<string> { "Gebruiker is verplicht." };
            }

            if (fouten.Any())
            {
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
            }
        }
    }
}
