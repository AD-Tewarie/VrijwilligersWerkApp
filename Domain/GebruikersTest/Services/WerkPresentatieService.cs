using Domain.GebruikersTest.Interfaces;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;

namespace Domain.Vrijwilligerswerk_Test
{
    public class WerkPresentatieService : IWerkPresentatieService
    {
        public List<WerkMetScore> FilterWerkOpPresentatieType(
        List<WerkMetScore> werkMetScores,
        string presentatieType)
        {
            return presentatieType.ToLower() switch
            {
                "top" => werkMetScores.OrderByDescending(w => w.Score).Take(5).ToList(),
                "minimum" => werkMetScores.Where(w => w.Score >= 50)
                    .OrderByDescending(w => w.Score).ToList(),
                _ => werkMetScores.OrderByDescending(w => w.Score).ToList()
            };
        }
    }
}