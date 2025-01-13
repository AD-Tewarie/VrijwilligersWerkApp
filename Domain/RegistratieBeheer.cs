using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Interfaces;

namespace Domain
{
    public class RegistratieBeheer : IRegistratieBeheer
    {
        private readonly IWerkRegistratieRepository registratieRepository;
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IUserBeheer userBeheer;
        private readonly IVrijwilligersWerkRepository werkRepository;
        private readonly IMapper<WerkRegistratie, WerkRegistratieDTO> mapper;

        public RegistratieBeheer(
            IWerkRegistratieRepository registratieRepository,
            IVrijwilligersWerkRepository werkRepository,
            IVrijwilligersWerkBeheer werkBeheer,
            IUserBeheer userBeheer,
            IMapper<WerkRegistratie, WerkRegistratieDTO> mapper)
        {
            this.registratieRepository = registratieRepository ?? throw new ArgumentNullException(nameof(registratieRepository));
            this.werkRepository = werkRepository ?? throw new ArgumentNullException(nameof(werkRepository));
            this.werkBeheer = werkBeheer ?? throw new ArgumentNullException(nameof(werkBeheer));
            this.userBeheer = userBeheer ?? throw new ArgumentNullException(nameof(userBeheer));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void RegistreerGebruikerVoorWerk(int userId, int werkId)
        {
            try
            {
                var (werk, user) = HaalWerkEnGebruikerOp(werkId, userId);
                ValideerRegistratie(werk, user);

                var registratie = MaakRegistratie(werk, user);
                SlaRegistratieOp(registratie);

                // Update het aantal registraties
                var success = werkRepository.BewerkAantalRegistraties(werkId, 1);
                if (!success)
                {
                    throw new Exception("Kon het aantal registraties niet bijwerken.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fout bij registreren voor vrijwilligerswerk: {ex.Message}", ex);
            }
        }

        public void VerwijderRegistratie(int registratieId)
        {
            try
            {
                var registratie = HaalRegistratieOp(registratieId);
                if (registratie == null)
                {
                    throw new Exception("Registratie niet gevonden.");
                }

                var werkId = registratie.VrijwilligersWerk.WerkId;

                // Eerst de registratie verwijderen
                var verwijderSuccess = registratieRepository.VerwijderWerkRegistratie(registratieId);
                if (!verwijderSuccess)
                {
                    throw new Exception("Kon de registratie niet verwijderen.");
                }

                // Dan het aantal registraties updaten
                var updateSuccess = werkRepository.BewerkAantalRegistraties(werkId, -1);
                if (!updateSuccess)
                {
                    // In dit geval zouden we eigenlijk een compenserende actie moeten uitvoeren
                    // omdat de registratie al verwijderd is maar de telling niet updated
                    throw new Exception("Kon het aantal registraties niet bijwerken.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fout bij verwijderen van registratie: {ex.Message}", ex);
            }
        }

        private (VrijwilligersWerk werk, User user) HaalWerkEnGebruikerOp(int werkId, int userId)
        {
            var werk = werkBeheer.HaalWerkOpID(werkId);
            if (werk == null)
                throw new KeyNotFoundException("Vrijwilligerswerk niet gevonden.");

            var user = userBeheer.HaalGebruikerOpID(userId);
            if (user == null)
                throw new KeyNotFoundException("Gebruiker niet gevonden.");

            return (werk, user);
        }

        private void ValideerRegistratie(VrijwilligersWerk werk, User user)
        {
            var bestaandeRegistraties = HaalRegistratiesOp();
            if (bestaandeRegistraties.Any(r =>
                r.VrijwilligersWerk.WerkId == werk.WerkId &&
                r.User.UserId == user.UserId))
            {
                throw new InvalidOperationException("Gebruiker is al geregistreerd voor dit werk.");
            }

            if (werk.AantalRegistraties >= werk.MaxCapaciteit)
            {
                throw new InvalidOperationException("Maximum capaciteit is bereikt.");
            }
        }

        private WerkRegistratie MaakRegistratie(VrijwilligersWerk werk, User user)
        {
            return WerkRegistratie.MaakNieuw(werk, user);
        }

        private void SlaRegistratieOp(WerkRegistratie registratie)
        {
            var registratieDto = mapper.MapToDTO(registratie);
            registratieRepository.AddWerkRegistratie(registratieDto);
        }

        public List<WerkRegistratie> HaalRegistratiesOp()
        {
            var dtos = registratieRepository.GetWerkRegistraties();
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList();
        }

        public WerkRegistratie HaalRegistratieOp(int registratieId)
        {
            var registratieDto = registratieRepository.GetRegistratieOnId(registratieId);
            if (registratieDto == null)
                throw new KeyNotFoundException("Registratie niet gevonden.");

            return mapper.MapToDomain(registratieDto);
        }
    }

}
