using Application.GebruikersTest.Interfaces;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Services
{
    public class WerkPresentatieFilterService : IWerkPresentatieFilterService
    {
        private const int STANDAARD_TOP_AANTAL = 5;

        public List<WerkMetScore> FilterOpPresentatieType(
            List<WerkMetScore> werkMetScores, 
            string presentatieType, 
            int minimumScore)
        {
            if (werkMetScores == null || !werkMetScores.Any())
                return new List<WerkMetScore>();

            // Filter eerst op minimale score
            var gefilterdOpScore = werkMetScores
                .Where(w => w.Score >= minimumScore)
                .OrderByDescending(w => w.Score)
                .ToList();

            if (string.IsNullOrWhiteSpace(presentatieType))
                return gefilterdOpScore;

            // Filter op basis van presentatie type
            return presentatieType.ToLower() switch
            {
                "top" => gefilterdOpScore
                    .Take(STANDAARD_TOP_AANTAL)
                    .ToList(),

                "minimum" => gefilterdOpScore,

                "alle" => werkMetScores
                    .OrderByDescending(w => w.Score)
                    .ToList(),

                _ => gefilterdOpScore
            };
        }
    }
}