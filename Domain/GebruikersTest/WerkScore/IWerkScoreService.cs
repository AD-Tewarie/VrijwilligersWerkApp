using System.Collections.Generic;
using Domain.GebruikersTest.Models;
using VrijwilligersWerkModel = Domain.Werk.Models.VrijwilligersWerk;

namespace Domain.GebruikersTest.WerkScore
{
    public interface IWerkScoreService
    {
        (int score, int maximaleScore) BerekenWerkScore(VrijwilligersWerkModel werk, Dictionary<Categorie, int> scores);
    }
}