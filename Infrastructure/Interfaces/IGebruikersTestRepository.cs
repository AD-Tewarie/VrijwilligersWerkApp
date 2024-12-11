using Infrastructure.DTO.Vrijwilligerswerk_Test;

namespace Infrastructure.Interfaces
{
    public interface IGebruikersTestRepository
    {
        List<CategorieDTO> HaalAlleCategorieënOp();
        public CategorieDTO GetCategorieOnId(int id);
        public List<TestVraagDTO> HaalAlleTestVraagOp();
        public TestVraagDTO GetTestVraagOnId(int id);
        public List<WerkCategorieDTO> GetWerkCategorieënByWerkId(int werkId);


    }

}