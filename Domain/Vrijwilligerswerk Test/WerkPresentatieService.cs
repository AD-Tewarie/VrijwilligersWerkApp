using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Domain.Vrijwilligerswerk_Test.WerkScore;

namespace Domain.Vrijwilligerswerk_Test
{
    public class WerkPresentatieService : IWerkPresentatieService
    {
        private readonly IWerkScoreService scoreService;

        public WerkPresentatieService(IWerkScoreService scoreService)
        {
            this.scoreService = scoreService;
        }

        public List<VrijwilligersWerk> PresenteerTopResultaten(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores,
            int aantal)
        {
            ValideerInput(werkLijst);
            var werkenMetScore = scoreService.BerekenScoresVoorWerkLijst(werkLijst, scores);

            return werkenMetScore
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)  // Ensure sorting
                .Take(aantal)
                .Select(x => x.Werk)
                .ToList();
        }

        public List<VrijwilligersWerk> PresenteerMinimumResultaten(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores,
            int minimumScore)
        {
            ValideerInput(werkLijst);
            var werkenMetScore = scoreService.BerekenScoresVoorWerkLijst(werkLijst, scores);

            return werkenMetScore
                .Where(x => x.Score >= minimumScore)
                .OrderByDescending(x => x.Score)  // Add sorting here
                .Select(x => x.Werk)
                .ToList();
        }

        public List<VrijwilligersWerk> PresenteerAlleResultaten(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores)
        {
            ValideerInput(werkLijst);
            var werkenMetScore = scoreService.BerekenScoresVoorWerkLijst(werkLijst, scores);

            return werkenMetScore
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)  // Add sorting here
                .Select(x => x.Werk)
                .ToList();
        }

        private void ValideerInput(List<VrijwilligersWerk> werkLijst)
        {
            if (werkLijst == null || !werkLijst.Any())
            {
                throw new ArgumentException("Werkenlijst mag niet leeg zijn");
            }
        }
    }
}