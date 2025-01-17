using Application.ViewModels;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Application.Mapper
{
    public class GebruikersTestViewMapper : IViewModelMapper<GebruikersTestViewModel, TestVraag>
    {
        private readonly int huidigeVraagNummer;
        private readonly int totaalVragen;

        public GebruikersTestViewMapper(int huidigeVraagNummer, int totaalVragen)
        {
            huidigeVraagNummer = huidigeVraagNummer;
            totaalVragen = totaalVragen;
        }

        public GebruikersTestViewModel MapNaarViewModel(TestVraag vraag)
        {
            return new GebruikersTestViewModel
            {
                VraagTekst = vraag.Tekst,
                VraagNummer = huidigeVraagNummer,
                TotaalAantalVragen = totaalVragen
            };
        }

        public TestVraag MapNaarDomainModel(GebruikersTestViewModel viewModel)
        {
            throw new NotImplementedException("Deze mapper is alleen voor weergave");
        }
    }
}
