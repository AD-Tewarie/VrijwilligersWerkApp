using Application.GebruikersTest.Interfaces;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Services
{
    public class WerkPresentatieFilterService : IWerkPresentatieFilterService
    {
        private const int STANDAARD_TOP_AANTAL = 5;
        private const int MINIMUM_RELEVANTE_SCORE = 20; // Score onder 20 wordt als niet relevant beschouwd

        public List<WerkMetScore> FilterOpPresentatieType(
            List<WerkMetScore> werkMetScores,
            string presentatieType,
            int minimumScore)
        {
            if (werkMetScores == null || !werkMetScores.Any())
                return new List<WerkMetScore>();

            // Sorteer eerst alle werk op score
            var gesorteerdeWerk = werkMetScores
                .OrderByDescending(w => w.Score)
                .ToList();

            // Filter op basis van presentatie type
            return presentatieType?.ToLower() switch
            {
                // Top 5 resultaten met minimale relevantie
                "top" => gesorteerdeWerk
                    .Where(w => w.Score >= MINIMUM_RELEVANTE_SCORE)
                    .Take(STANDAARD_TOP_AANTAL)
                    .ToList(),

                // Alle resultaten boven minimum score
                "minimum" => gesorteerdeWerk
                    .Where(w => w.Score >= minimumScore)
                    .ToList(),

                // Alle resultaten met minimale relevantie
                "alle" => gesorteerdeWerk
                    .Where(w => w.Score >= MINIMUM_RELEVANTE_SCORE)
                    .ToList(),

                // Standaard: gebruik minimum score filter
                _ => gesorteerdeWerk
                    .Where(w => w.Score >= minimumScore)
                    .ToList()
            };
        }
    }
}