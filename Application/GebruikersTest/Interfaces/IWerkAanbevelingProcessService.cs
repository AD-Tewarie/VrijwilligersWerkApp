using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Interfaces
{
    public interface IWerkAanbevelingProcessService
    {
        List<WerkMetScore> BerekenAanbevelingen(TestSessie sessie, string presentatieType);
    }
}