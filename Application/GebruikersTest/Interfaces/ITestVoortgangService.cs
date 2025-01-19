using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface ITestVoortgangService
    {
        GebruikersTestViewModel MaakNieuweTest(int gebruikerId);
        GebruikersTestViewModel HaalTestOp(int gebruikerId);
        bool BeantwoordVraag(int gebruikerId, int huidigeStap, int antwoord);
        bool HeeftGebruikerActieveTest(int gebruikerId);
        bool BestaatTest(int gebruikerId);
        void ResetTest(int gebruikerId);
    }
}