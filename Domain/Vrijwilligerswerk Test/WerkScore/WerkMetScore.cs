using Domain.Models;

namespace Domain.Vrijwilligerswerk_Test.PresentatieStrategy
{
    public class WerkMetScore
    {
        public VrijwilligersWerk Werk { get; }
        public int Score { get; }

        public WerkMetScore(VrijwilligersWerk werk, int score)
        {
            Werk = werk;
            Score = score;
        }
    }
}