using Domain.Vrijwilligerswerk_Test;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Common.Interfaces.Repository
{
    public interface IGebruikersTestRepository
    {
        List<Categorie> HaalAlleCategorieënOp();
        public Categorie GetCategorieOnId(int id);
        public List<TestVraag> HaalAlleTestVragenOp();
        public TestVraag GetTestVraagOnId(int id);
        public List<WerkCategorie> GetWerkCategorieënByWerkId(int werkId);


    }

}