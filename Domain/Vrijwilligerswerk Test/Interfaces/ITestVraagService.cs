using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Vrijwilligerswerk_Test.Interfaces
{
    public interface ITestVraagService
    {
        List<TestVraag> HaalAlleTestVragenOp();
        TestVraag GetTestVraagOpId(int id);
    }
}
