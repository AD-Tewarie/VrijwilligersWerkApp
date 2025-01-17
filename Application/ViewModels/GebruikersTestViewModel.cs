using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class GebruikersTestViewModel
    {
        public string VraagTekst { get; set; }
        public int VraagNummer { get; set; }
        public int TotaalAantalVragen { get; set; }
        public int? VorigeAntwoord { get; set; }
        public bool IsKlaar => VraagNummer >= TotaalAantalVragen;

        // Bereken voortgang als property
        public int VoortgangPercentage =>
            TotaalAantalVragen > 0 ?
            (int)Math.Round((double)VraagNummer / TotaalAantalVragen * 100) :
            0;
    }
}
