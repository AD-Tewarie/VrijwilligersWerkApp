using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Interfaces
{
    public interface ITestBeheer
    {
        List<VrijwilligersWerk> ZoekGeschiktWerk(
       Dictionary<int, int> affiniteiten,
       Dictionary<int, int> antwoorden,
       List<VrijwilligersWerk> beschikbaarWerk,
       string presentatieType);

        Dictionary<Categorie, int> BerekenTestScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden);

        List<Categorie> HaalAlleCategorieënOp();
        List<TestVraag> HaalAlleTestVragenOp();
        Categorie GetCategorieOpId(int id);
    }
}