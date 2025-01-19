using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface ITestResultaatService
    {
        GebruikersTestResultaatViewModel HaalResultatenOp(int gebruikerId, string presentatieType = "top", int? minimumScore = null);
    }
}