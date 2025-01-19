using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Domain.Werk.Interfaces;

namespace Application.Werk.Services
{
    public class WerkBeheerService : IWerkBeheerService
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;

        public WerkBeheerService(IVrijwilligersWerkBeheer werkBeheer)
        {
            this.werkBeheer = werkBeheer ?? throw new ArgumentNullException(nameof(werkBeheer));
        }

        public void VoegWerkToe(WerkAanmaakViewModel model)
        {
            werkBeheer.VoegWerkToe(
                model.Titel,
                model.Omschrijving,
                model.MaxCapaciteit,
                model.Locatie,
                model.CategorieId);
        }

        public void BewerkWerk(int werkId, WerkAanmaakViewModel model)
        {
            werkBeheer.BewerkWerk(
                werkId,
                model.Titel,
                model.MaxCapaciteit,
                model.Omschrijving,
                model.Locatie);
        }

        public void VerwijderWerk(int werkId)
        {
            werkBeheer.VerwijderWerk(werkId);
        }

        public List<WerkAanbiedingOverzichtViewModel> HaalBeschikbareWerkAanbiedingenOp()
        {
            var werkAanbiedingen = werkBeheer.BekijkAlleWerk();
            return werkAanbiedingen.Select(werk => new WerkAanbiedingOverzichtViewModel
            {
                WerkId = werk.WerkId,
                Titel = werk.Titel,
                Omschrijving = werk.Omschrijving,
                MaxCapaciteit = werk.MaxCapaciteit,
                AantalRegistraties = werk.AantalRegistraties,
                Locatie = werk.Locatie
            }).ToList();
        }

        public WerkDetailsViewModel HaalWerkDetailsOp(int werkId)
        {
            var werk = werkBeheer.HaalWerkOpID(werkId);
            return new WerkDetailsViewModel(
                werk.WerkId,
                werk.Titel,
                werk.Omschrijving,
                werk.MaxCapaciteit,
                werk.AantalRegistraties,
                werk.Locatie);
        }
    }
}