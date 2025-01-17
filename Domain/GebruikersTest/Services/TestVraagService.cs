using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.Vrijwilligerswerk_Test.Models;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestVraagService : ITestVraagService
    {
        private readonly IGebruikersTestRepository repository;

        public TestVraagService(IGebruikersTestRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<TestVraag> HaalAlleTestVragenOp()
        {
            return repository.HaalAlleTestVragenOp();
        }

        public TestVraag GetTestVraagOpId(int id)
        {
            ValideerTestVraagId(id);
            var testVraag = repository.GetTestVraagOnId(id);

            if (testVraag == null)
                throw new KeyNotFoundException($"TestVraag met ID {id} niet gevonden.");

            return testVraag;
        }

        private void ValideerTestVraagId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("TestVraag ID moet groter zijn dan 0.");
        }
    }
}
