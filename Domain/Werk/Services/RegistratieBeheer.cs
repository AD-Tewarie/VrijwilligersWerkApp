using Domain.Common.Interfaces.Repository;
using Domain.Gebruikers.Interfaces;
using Domain.Gebruikers.Models;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;

namespace Domain.Werk.Services
{
    public class RegistratieBeheer : IRegistratieBeheer
    {
        private readonly IWerkRegistratieRepository registratieRepository;
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IUserBeheer userBeheer;
        private readonly IVrijwilligersWerkRepository werkRepository;

        public RegistratieBeheer(
            IWerkRegistratieRepository registratieRepository,
            IVrijwilligersWerkRepository werkRepository,
            IVrijwilligersWerkBeheer werkBeheer,
            IUserBeheer userBeheer)
        {
            this.registratieRepository = registratieRepository ?? throw new ArgumentNullException(nameof(registratieRepository));
            this.werkRepository = werkRepository ?? throw new ArgumentNullException(nameof(werkRepository));
            this.werkBeheer = werkBeheer ?? throw new ArgumentNullException(nameof(werkBeheer));
            this.userBeheer = userBeheer ?? throw new ArgumentNullException(nameof(userBeheer));
        }

        public void RegistreerGebruikerVoorWerk(int userId, int werkId)
        {
            var (werk, user) = HaalEnValideerWerkEnGebruiker(werkId, userId);
            ValideerRegistratie(werk, user);

            var registratie = WerkRegistratie.MaakNieuw(werk, user);
            registratieRepository.AddWerkRegistratie(registratie);

            var success = werkRepository.BewerkAantalRegistraties(werkId, 1);
            if (!success)
            {
                throw new InvalidOperationException("Kon het aantal registraties niet bijwerken.");
            }
        }

        public void VerwijderRegistratie(int registratieId)
        {
            var registratie = HaalRegistratieOp(registratieId);
            var werkId = registratie.VrijwilligersWerk.WerkId;

            var verwijderSuccess = registratieRepository.VerwijderWerkRegistratie(registratieId);
            if (!verwijderSuccess)
            {
                throw new InvalidOperationException("Kon de registratie niet verwijderen.");
            }

            var updateSuccess = werkRepository.BewerkAantalRegistraties(werkId, -1);
            if (!updateSuccess)
            {
                throw new InvalidOperationException("Kon het aantal registraties niet bijwerken.");
            }
        }

        public int HaalAantalRegistratiesOp(int werkId)
        {
            ValideerWerkId(werkId);
            return registratieRepository.GetRegistratieCountForWerk(werkId);
        }

        public List<WerkRegistratie> HaalRegistratiesOp()
        {
            return registratieRepository.GetWerkRegistraties();
        }

        public WerkRegistratie HaalRegistratieOp(int registratieId)
        {
            ValideerRegistratieId(registratieId);
            var registratie = registratieRepository.GetRegistratieOnId(registratieId);

            if (registratie == null)
                throw new KeyNotFoundException($"Registratie met ID {registratieId} niet gevonden.");

            return registratie;
        }

        private (VrijwilligersWerk werk, User user) HaalEnValideerWerkEnGebruiker(int werkId, int userId)
        {
            ValideerWerkId(werkId);
            ValideerUserId(userId);

            var werk = werkBeheer.HaalWerkOpID(werkId);
            var user = userBeheer.HaalGebruikerOpID(userId);

            if (werk == null)
                throw new KeyNotFoundException($"Vrijwilligerswerk met ID {werkId} niet gevonden.");
            if (user == null)
                throw new KeyNotFoundException($"Gebruiker met ID {userId} niet gevonden.");

            return (werk, user);
        }

        private void ValideerRegistratie(VrijwilligersWerk werk, User user)
        {
            if (BestaatRegistratie(werk.WerkId, user.UserId))
                throw new InvalidOperationException("Gebruiker is al geregistreerd voor dit werk.");

            if (werk.AantalRegistraties >= werk.MaxCapaciteit)
                throw new InvalidOperationException("Maximum capaciteit is bereikt.");
        }

        private bool BestaatRegistratie(int werkId, int userId)
        {
            return HaalRegistratiesOp()
                .Any(r => r.VrijwilligersWerk.WerkId == werkId && r.User.UserId == userId);
        }

        private void ValideerWerkId(int werkId)
        {
            if (werkId <= 0)
                throw new ArgumentException("Werk ID moet groter zijn dan 0.");
        }

        private void ValideerUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0.");
        }

        private void ValideerRegistratieId(int registratieId)
        {
            if (registratieId <= 0)
                throw new ArgumentException("Registratie ID moet groter zijn dan 0.");
        }
    }
}
