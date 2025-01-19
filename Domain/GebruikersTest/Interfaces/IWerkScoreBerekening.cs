using Domain.GebruikersTest.Models;
using Domain.Werk.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface IWerkScoreBerekening
    {
        int BerekenScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores);
    }
}