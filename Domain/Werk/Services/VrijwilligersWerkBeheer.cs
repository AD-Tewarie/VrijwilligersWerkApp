using Domain.Common.Data;
using Domain.Common.Exceptions;
using Domain.Common.Interfaces.Repository;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;

namespace Domain.Werk.Services
{
    public class VrijwilligersWerkBeheer : IVrijwilligersWerkBeheer
    {
        private readonly IVrijwilligersWerkRepository repository;

        public VrijwilligersWerkBeheer(IVrijwilligersWerkRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void VoegWerkToe(string titel, string omschrijving, int maxCapaciteit, int categorieId)
        {
            ValideerWerkDetails(titel, maxCapaciteit, omschrijving);

            var werk = VrijwilligersWerk.MaakNieuw(titel, omschrijving, maxCapaciteit);
            repository.AddVrijwilligersWerk(werk);
            repository.VoegWerkCategorieToeAanNieuweWerk(werk.WerkId, categorieId);
        }

        public List<VrijwilligersWerk> BekijkAlleWerk()
        {
            return repository.GetVrijwilligersWerk();
        }

        public void VerwijderWerk(int werkId)
        {
            ValideerWerkId(werkId);
            HaalEnControleerWerk(werkId);
            repository.VerwijderVrijwilligersWerk(werkId);
        }

        public void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving)
        {
            ValideerWerkDetails(nieuweTitel, nieuweCapaciteit, nieuweBeschrijving);
            var werk = HaalEnControleerWerk(werkId);

            werk.WijzigDetails(nieuweTitel, nieuweBeschrijving, nieuweCapaciteit);
            repository.BewerkVrijwilligersWerk(werk);
        }

        public VrijwilligersWerk HaalWerkOpID(int id)
        {
            ValideerWerkId(id);
            return HaalEnControleerWerk(id);
        }

        private void ValideerWerkDetails(string titel, int capaciteit, string beschrijving)
        {
            var fouten = new List<string>();

            if (string.IsNullOrWhiteSpace(titel))
                fouten.Add("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(beschrijving))
                fouten.Add("Beschrijving is verplicht.");
            if (capaciteit <= 0)
                fouten.Add("Capaciteit moet groter zijn dan 0.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

        private void ValideerWerkId(int werkId)
        {
            if (werkId <= 0)
                throw new ArgumentException("Werk ID moet groter zijn dan 0.");
        }

        private VrijwilligersWerk HaalEnControleerWerk(int werkId)
        {
            var werk = HaalWerkOpID(werkId);
            if (werk == null)
                throw new KeyNotFoundException($"Vrijwilligerswerk met ID {werkId} niet gevonden.");
            return werk;
        }
    }

}

