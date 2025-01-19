using Domain.GebruikersTest.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ITestResultaatBeheer
    {
        bool HeeftBestaandeResultaten(int gebruikerId);
        void BewaarResultaten(int gebruikerId, TestResultaat resultaat);
        TestResultaat? HaalResultatenOp(int gebruikerId);
    }
}