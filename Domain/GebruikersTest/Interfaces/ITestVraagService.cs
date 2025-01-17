using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ITestVraagService
    {
        List<TestVraag> HaalAlleTestVragenOp();
        TestVraag GetTestVraagOpId(int id);
    }
}
