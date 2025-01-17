using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Domain.Vrijwilligerswerk_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
   public class GebruikersTestResultaatViewModel
    {
        public Dictionary<Categorie, int> GesorteerdeScores { get; set; }
        public List<WerkMetScore> AanbevolenWerk { get; set; }
        public string HuidigePresentatieType { get; set; } = "top";
    }
}
