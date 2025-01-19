using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface ITestResultaatMapper
    {
        GebruikersTestResultaatViewModel MapNaarViewModel(
            int gebruikerId,
            List<WerkAanbevelingViewModel> aanbevelingen,
            string presentatieType);
    }
}