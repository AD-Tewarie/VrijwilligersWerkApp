using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.Interfaces;

namespace Application.GebruikersTest.Services
{
    public class TestCategorieService : ITestCategorieService
    {
        private readonly ITestBeheer testBeheer;

        public TestCategorieService(ITestBeheer testBeheer)
        {
            this.testBeheer = testBeheer ?? throw new ArgumentNullException(nameof(testBeheer));
        }

        public List<CategorieViewModel> HaalAlleCategorieënOp()
        {
            var categorieën = testBeheer.HaalAlleCategorieënOp();
            return categorieën.Select(c => new CategorieViewModel(c.Id, c.Naam)).ToList();
        }
    }
}