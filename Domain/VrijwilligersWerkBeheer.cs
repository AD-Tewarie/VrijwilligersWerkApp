using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Domain
{
    public class VrijwilligersWerkBeheer : IVrijwilligersWerkBeheer
    {

        private readonly IVrijwilligersWerkRepository repository;
        private readonly IMapper<VrijwilligersWerk, VrijwilligersWerkDTO> mapper;

        public VrijwilligersWerkBeheer(
            IVrijwilligersWerkRepository repository,
            IMapper<VrijwilligersWerk, VrijwilligersWerkDTO> mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void VoegWerkToe(VrijwilligersWerk werk, int categorieId)
        {
            ValideerWerk(werk);
            var werkDto = MapNaarDTO(werk);
            SlaWerkOp(werkDto);

            var werkMetId = VrijwilligersWerk.MaakMetId(
        werkDto.WerkId,
        werk.Titel,
        werk.Omschrijving,
        werk.MaxCapaciteit
    );

            repository.VoegWerkCategorieToeAanNieuweWerk( werkMetId.WerkId, categorieId);
        }

        private void ValideerWerk(VrijwilligersWerk werk)
        {
            if (werk == null)
                throw new ArgumentNullException(nameof(werk));
        }

        private VrijwilligersWerkDTO MapNaarDTO(VrijwilligersWerk werk)
        {
            return mapper.MapToDTO(werk);
        }

        private void SlaWerkOp(VrijwilligersWerkDTO werkDto)
        {
            repository.AddVrijwilligersWerk(werkDto);
        }

        public List<VrijwilligersWerk> BekijkAlleWerk()
        {
            var dtos = repository.GetVrijwilligersWerk();
            return MapWerkLijst(dtos);
        }

        private List<VrijwilligersWerk> MapWerkLijst(IEnumerable<VrijwilligersWerkDTO> dtos)
        {
            return dtos.Select(dto => mapper.MapToDomain(dto)).ToList();
        }

        public void VerwijderWerk(int werkId)
        {
            ValideerWerkId(werkId);
            ControleerWerkBestaat(werkId);
            repository.VerwijderVrijwilligersWerk(werkId);
        }

        private void ValideerWerkId(int werkId)
        {
            if (werkId <= 0)
                throw new ArgumentException("Werk ID moet groter zijn dan 0.");
        }

        private void ControleerWerkBestaat(int werkId)
        {
            var werk = HaalWerkOpID(werkId);
            if (werk == null)
                throw new KeyNotFoundException($"Vrijwilligerswerk met ID {werkId} niet gevonden.");
        }

        public void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving)
        {
            ValideerWerkDetails(werkId, nieuweTitel, nieuweCapaciteit, nieuweBeschrijving);

            var werk = HaalEnControleerWerk(werkId);
            WerkBijwerken(werk, nieuweTitel, nieuweCapaciteit, nieuweBeschrijving);
            WerkOpslaanNaBewerking(werk);
        }

        private void ValideerWerkDetails(int werkId, string titel, int capaciteit, string beschrijving)
        {
            var fouten = new List<string>();

            if (werkId <= 0)
                fouten.Add("Werk ID moet groter zijn dan 0.");
            if (string.IsNullOrWhiteSpace(titel))
                fouten.Add("Titel is verplicht.");
            if (string.IsNullOrWhiteSpace(beschrijving))
                fouten.Add("Beschrijving is verplicht.");
            if (capaciteit <= 0)
                fouten.Add("Capaciteit moet groter zijn dan 0.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

        private VrijwilligersWerk HaalEnControleerWerk(int werkId)
        {
            var werk = HaalWerkOpID(werkId);
            if (werk == null)
                throw new KeyNotFoundException($"Vrijwilligerswerk met ID {werkId} niet gevonden.");
            return werk;
        }

        private void WerkBijwerken(VrijwilligersWerk werk, string titel, int capaciteit, string beschrijving)
        {
            werk.WijzigDetails(titel, beschrijving, capaciteit);
        }

        private void WerkOpslaanNaBewerking(VrijwilligersWerk werk)
        {
            var werkDto = mapper.MapToDTO(werk);
            repository.BewerkVrijwilligersWerk(werkDto);
        }

        public VrijwilligersWerk HaalWerkOpID(int id)
        {
            ValideerWerkId(id);
            var werkDto = repository.GetWerkOnId(id);
            return werkDto == null ? null : mapper.MapToDomain(werkDto);
        }

    }

}

