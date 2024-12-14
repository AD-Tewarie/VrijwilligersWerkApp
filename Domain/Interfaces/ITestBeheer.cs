using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Interfaces
{
    public interface ITestBeheer
    {
        Dictionary<Categorie, int> BerekenTestScores(Dictionary<int, int> affiniteiten, Dictionary<int, int> antwoorden);
        List<VrijwilligersWerk> FilterWerk(List<VrijwilligersWerk> werk, Dictionary<Categorie, int> gesorteerdeScores);
        Categorie GetCategorieOnId(int id);
        List<VraagCategorie> HaalAlleVraagCategorieënOp();
        List<VrijwilligersWerk> HaalAlleWerkOp();
        public List<Categorie> HaalAlleCategorieënOp();
    }
}