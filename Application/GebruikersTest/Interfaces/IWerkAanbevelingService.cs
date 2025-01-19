using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface IWerkAanbevelingService
    {
        List<WerkAanbevelingViewModel> HaalAanbevelingenOp(int gebruikerId, string presentatieType, int minimumScore = 50);
    }
}