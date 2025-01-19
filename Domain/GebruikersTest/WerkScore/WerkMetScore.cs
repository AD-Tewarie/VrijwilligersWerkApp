using Domain.Werk.Models;

namespace Domain.GebruikersTest.WerkScore
{
    public class WerkMetScore
    {
        public VrijwilligersWerk Werk { get; private set; }
        public int Score { get; private set; }

        public WerkMetScore(VrijwilligersWerk werk, int score)
        {
            Werk = werk ?? throw new ArgumentNullException(nameof(werk));
            Score = score;
        }
    }
}