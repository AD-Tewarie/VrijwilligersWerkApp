using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface ITestCategorieService
    {
        List<CategorieViewModel> HaalAlleCategorieënOp();
    }
}