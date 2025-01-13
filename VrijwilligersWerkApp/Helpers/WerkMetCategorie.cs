using Domain.Models;

namespace VrijwilligersWerkApp.Helpers
{
    public class WerkMetCategorie
    {
        public VrijwilligersWerk Werk { get; set; }
        public string CategorieNaam { get; set; }
        public int HuidigeRegistraties { get; set; }
    }
}
