using Application.ViewModels;
using Domain.GebruikersTest.Interfaces;
using Domain.Werk.Interfaces;
using Domain.Vrijwilligerswerk_Test;
using Domain.Werk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class VrijwilligersWerkViewMapper : IViewModelMapper<VrijwilligersWerkViewModel, VrijwilligersWerk>
    {
        private readonly ICategorieService categorieService;
        private readonly IRegistratieBeheer registratieBeheer;

        public VrijwilligersWerkViewMapper(
            ICategorieService categorieService,
            IRegistratieBeheer registratieBeheer)
        {
            this.categorieService = categorieService;
            this.registratieBeheer = registratieBeheer;
        }

        public VrijwilligersWerkViewModel MapNaarViewModel(VrijwilligersWerk werk)
        {
            return new VrijwilligersWerkViewModel
            {
                WerkId = werk.WerkId,
                Titel = werk.Titel,
                Omschrijving = werk.Omschrijving,
                MaxCapaciteit = werk.MaxCapaciteit,
                HuidigeRegistraties = HaalAantalRegistratiesOp(werk.WerkId),
                CategorieNamen = HaalCategorieNamenOp(werk.WerkId)
            };
        }

        private int HaalAantalRegistratiesOp(int werkId)
        {
            return registratieBeheer.HaalAantalRegistratiesOp(werkId);
        }

        private string HaalCategorieNamenOp(int werkId)
        {
            var categorieën = categorieService.HaalCategorieënVoorWerkOp(werkId);
            return string.Join(", ", categorieën.Select(c => c.Naam));
        }

        public VrijwilligersWerk MapNaarDomainModel(VrijwilligersWerkViewModel viewModel)
        {
            throw new NotImplementedException("Deze mapper is alleen voor weergave");
        }
    }
}
