using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Interfaces
{
    public interface IWerkAanbevelingMapper
    {
        WerkAanbevelingViewModel MapNaarViewModel(
            WerkMetScore werkMetScore, 
            int matchPercentage, 
            string presentatieType);
    }
}