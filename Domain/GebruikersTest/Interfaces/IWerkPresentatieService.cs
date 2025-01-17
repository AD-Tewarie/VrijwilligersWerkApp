using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;

namespace Domain.GebruikersTest.Interfaces
{
    public interface IWerkPresentatieService
    {
        List<WerkMetScore> FilterWerkOpPresentatieType(List<WerkMetScore> werkMetScores, string presentatieType);

    }
}
