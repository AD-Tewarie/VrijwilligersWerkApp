using Domain.GebruikersTest.Models;

namespace Application.GebruikersTest.Interfaces
{
    public interface IMatchBerekeningService
    {
        int BerekenMatchPercentage(Domain.Werk.Models.VrijwilligersWerk werk, TestSessie sessie);
        Dictionary<Domain.Werk.Models.VrijwilligersWerk, int> BerekenMatchPercentages(List<Domain.Werk.Models.VrijwilligersWerk> werken, TestSessie sessie);
    }
}