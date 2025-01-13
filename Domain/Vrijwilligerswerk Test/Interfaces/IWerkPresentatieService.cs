using Domain.Models;

namespace Domain.Vrijwilligerswerk_Test.Interfaces
{
    public interface IWerkPresentatieService
    {
        List<VrijwilligersWerk> PresenteerTopResultaten(
        List<VrijwilligersWerk> werkLijst,
        Dictionary<Categorie, int> scores,
        int aantal);

        List<VrijwilligersWerk> PresenteerMinimumResultaten(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores,
            int minimumScore);

        List<VrijwilligersWerk> PresenteerAlleResultaten(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores);


    }
}
