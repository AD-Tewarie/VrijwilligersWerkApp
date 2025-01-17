using Application.ViewModels;
using Domain.Vrijwilligerswerk_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class CategorieViewMapper : IViewModelMapper<GebruikersTestViewModel, Categorie>
    {
        private readonly int huidigeVraagNummer;
        private readonly int totaalVragen;

        public CategorieViewMapper(int huidigeVraagNummer, int totaalVragen)
        {
            this.huidigeVraagNummer = huidigeVraagNummer;
            this.totaalVragen = totaalVragen;
        }

        public GebruikersTestViewModel MapNaarViewModel(Categorie domainModel)
        {
            return new GebruikersTestViewModel
            {
                VraagTekst = $"Hoe belangrijk vind je {domainModel.Naam}?",
                VraagNummer = huidigeVraagNummer,
                TotaalAantalVragen = totaalVragen
            };
        }

        public Categorie MapNaarDomainModel(GebruikersTestViewModel viewModel)
        {
            throw new NotImplementedException("Deze mapper is alleen voor weergave");
        }

    }
}
