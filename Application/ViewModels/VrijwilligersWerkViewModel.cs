using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class VrijwilligersWerkViewModel
    {
        public int WerkId { get; set; }
        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public int MaxCapaciteit { get; set; }
        public int HuidigeRegistraties { get; set; }
        public string CategorieNamen { get; set; }

        // Bereken automatisch of het werk vol is
        public bool IsVol => HuidigeRegistraties >= MaxCapaciteit;
    }
}
