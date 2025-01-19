using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Interfaces
{
    public interface IWerkPresentatieFilterService
    {
        List<WerkMetScore> FilterOpPresentatieType(
            List<WerkMetScore> werkMetScores, 
            string presentatieType,
            int minimumScore);
    }
}